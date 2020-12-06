using Giny.Core.DesignPattern;
using Giny.Core.Network.Messages;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Parties
{
    public abstract class Party
    {
        public int Id
        {
            get;
            private set;
        }
        private bool Restricted
        {
            get;
            set;
        }
        public abstract PartyTypeEnum Type
        {
            get;
        }
        public abstract byte MaxParticipants
        {
            get;
        }
        public Character Leader
        {
            get;
            private set;
        }
        public string PartyName
        {
            get;
            private set;
        }
        public List<Character> Members
        {
            get;
            private set;
        }
        public List<Character> Guests
        {
            get;
            private set;
        }
        public bool IsFull
        {
            get
            {
                if (Count < MaxParticipants)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public int Count
        {
            get
            {
                return Members.Count() + Guests.Count();
            }
        }
        public Party(int partyId, Character leader)
        {
            this.Members = new List<Character>();
            this.Guests = new List<Character>();
            this.Leader = leader;
            this.Id = Id;
            this.PartyName = "";
        }

        public PartyMemberInformations[] GetPartyMembersInformations()
        {
            return Array.ConvertAll(Members.ToArray(), x => x.GetPartyMemberInformations());
        }

        public PartyInvitationMemberInformations[] GetPartyInvitationMembersInformations()
        {
            return Array.ConvertAll(Members.ToArray(), x => x.GetPartyInvitationMemberInformations());
        }

        public PartyGuestInformations[] GetPartyGuestsInformations()
        {
            return Array.ConvertAll(Guests.ToArray(), x => x.GetPartyGuestInformations(this));
        }

        public void Create(Character Creator, Character Invited)
        {
            Members.Add(Creator);
            Creator.Party = this;
            this.Leader = Creator;
            Party.SendPartyJoinMessage(Creator);

            Creator.Client.Send(new PartyUpdateMessage()
            {
                memberInformations = Creator.GetPartyMemberInformations(),
                partyId = Id,
            });

            OnInvited(Invited, Creator);
        }

        public void OnInvited(Character invited, Character invitor)
        {
            invited.Client.Send(new PartyInvitationMessage()
            {
                partyId = Id,
                fromId = invitor.Id,
                fromName = invitor.Name,
                maxParticipants = MaxParticipants,
                partyName = PartyName,
                partyType = (byte)Type,
                toId = invited.Id,
            });

            this.AddGuest(invited);

        }
        public void AcceptInvitation(Character character)
        {
            if (IsFull)
            {
                character.Client.Send(new PartyCannotJoinErrorMessage()
                {
                    partyId = Id,
                    reason = (byte)PartyJoinErrorEnum.PARTY_JOIN_ERROR_PARTY_FULL
                });
                return;
            }
            if (character.HasParty)
            {
                character.Party.Leave(character);
            }
            AddMember(character);
        }

        public void RefuseInvation(Character character)
        {
            if (Guests.Contains(character))
            {
                SendMembers(new PartyRefuseInvitationNotificationMessage()
                {
                    partyId = Id,
                    guestId = character.Id,
                });

                RemoveGuest(character);
            }
        }

        public void CancelInvitation(Character canceller, int guestId)
        {
            if (canceller == Leader)
            {
                Character Guest = Guests.Find(x => x.Id == guestId);

                if (Guest != null)
                {
                    Guest.Client.Send(new PartyInvitationCancelledForGuestMessage()
                    {
                        partyId = Id,
                        cancelerId = canceller.Id,
                    });

                    RemoveGuest(Guest);
                    if (Count > 1)
                    {
                        SendMembers(new PartyCancelInvitationNotificationMessage()
                        {
                            partyId = Id,
                            cancelerId = canceller.Id,
                            guestId = guestId,
                        });
                    }
                }
            }
        }

        public void Abdicate(Character character = null)
        {
            if (Count <= 1)
                return;
            if (Leader == character)
                return;
            if (character == null)
            {
                this.Leader = Members.First(x => x.Id != this.Leader.Id);

                SendMembers(new PartyLeaderUpdateMessage()
                {
                    partyId = Id,
                    partyLeaderId = Leader.Id,
                });
            }
            else
            {
                if (Members.Contains(character))
                {
                    this.Leader = character;

                    SendMembers(new PartyLeaderUpdateMessage()
                    {
                        partyId = Id,
                        partyLeaderId = Leader.Id,
                    });
                }
            }
        }

        public bool Leave(Character character)
        {
            RemoveMember(character);

            if (!VerifiyIntegrity())
            {
                return false;
            }

            if (character == this.Leader && Count <= 1)
            {
                this.Abdicate();
            }

            SendMembers(new PartyMemberRemoveMessage()
            {
                partyId = Id,
                leavingPlayerId = character.Id,
            });

            return true;
        }



        public void Delete()
        {
            SendToAll(new PartyLeaveMessage()
            {
                partyId = Id,
            });

            foreach (Character character in Members)
            {
                character.Party = null;
            }
            foreach (Character character in Guests)
            {
                character.GuestedParties.Remove(this);
            }

            PartyManager.Instance.Remove(this);
        }

        public void AddMember(Character character)
        {
            if (!Members.Contains(character))
            {
                Members.Add(character);
                character.Party = this;
                RemoveGuest(character);
                Party.SendPartyJoinMessage(character);

                character.Client.Send(new PartyUpdateMessage()
                {
                    partyId = Id,
                    memberInformations = character.GetPartyMemberInformations(),
                });

                UpdateMember(character);
                RemoveGuest(character);


            }
        }
        public void UpdateMember(Character character)
        {
            if (character.Party != this)
                return;

            SendMembers(new PartyNewMemberMessage()
            {
                partyId = Id,
                memberInformations = character.GetPartyMemberInformations()
            });
        }
        public void RemoveMember(Character character)
        {
            if (Members.Contains(character))
            {
                Members.Remove(character);
                character.Party = null;
                character.Client.Send(new PartyLeaveMessage()
                {
                    partyId = Id
                });
            }
        }

        public void AddGuest(Character character)
        {
            if (!Guests.Contains(character) && !Members.Contains(character))
            {
                Guests.Add(character);
                character.GuestedParties.Add(this);

                SendMembers(new PartyNewGuestMessage()
                {
                    partyId = Id,
                    guest = character.GetPartyGuestInformations(this),
                });
            }
        }

        public void RemoveGuest(Character character)
        {
            if (Guests.Contains(character))
            {
                Guests.Remove(character);
                character.GuestedParties.Remove(this);
                VerifiyIntegrity();
            }
        }
        private bool VerifiyIntegrity()
        {
            if (Count <= 1)
            {
                this.Delete();
                return false;
            }
            else
            {
                return true;
            }
        }
        public void Kick(Character kicked, Character kicker)
        {
            if (kicker == Leader)
            {
                RemoveMember(kicked);

                kicked.Client.Send(new PartyKickedByMessage()
                {
                    kickerId = kicker.Id,
                    partyId = Id,
                });

                SendMembers(new PartyMemberEjectedMessage()
                {
                    partyId = Id,
                    kickerId = kicker.Id,
                    leavingPlayerId = kicked.Id,
                });

                VerifiyIntegrity();
            }
        }

        public Character GetMember(long characterId)
        {
            return this.Members.Find(x => x.Id == characterId);
        }

        public static void SendPartyJoinMessage(Character character)
        {
            character.Client.Send(new PartyJoinMessage()
            {
                guests = character.Party.GetPartyGuestsInformations(),
                maxParticipants = character.Party.MaxParticipants,
                members = character.Party.GetPartyMembersInformations(),
                partyId = character.Party.Id,
                restricted = character.Party.Restricted,
                partyName = character.Party.PartyName,
                partyLeaderId = character.Party.Leader.Id,
                partyType = (byte)character.Party.Type,
            });
        }




        public void SendMembers(NetworkMessage message)
        {
            foreach (Character character in Members)
            {
                character.Client.Send(message);
            }
        }
        public void SendGuests(NetworkMessage message)
        {
            foreach (Character character in Guests)
            {
                character.Client.Send(message);
            }
        }

        public void SendToAll(NetworkMessage message)
        {
            SendMembers(message);
            SendGuests(message);
        }
    }
}
