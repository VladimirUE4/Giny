﻿using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using Giny.Core.Network.Messages;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights;
using System;
using System.Collections.Concurrent;
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
        public ConcurrentDictionary<long, Character> Members
        {
            get;
            private set;
        }
        public ConcurrentDictionary<long, Character> Guests
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
            this.Members = new ConcurrentDictionary<long, Character>();
            this.Guests = new ConcurrentDictionary<long, Character>();
            this.Leader = leader;
            this.Id = Id;
            this.PartyName = "";
        }

        public PartyMemberInformations[] GetPartyMembersInformations()
        {
            return Members.Values.Select(x => x.GetPartyMemberInformations()).ToArray();
        }

        public PartyInvitationMemberInformations[] GetPartyInvitationMembersInformations()
        {
            return Members.Values.Select(x => x.GetPartyInvitationMemberInformations()).ToArray();
        }

        public PartyGuestInformations[] GetPartyGuestsInformations()
        {
            return Guests.Values.Select(x => x.GetPartyGuestInformations(this)).ToArray();
        }

        public void Create(Character source, Character invited)
        {
            if (!Members.ContainsKey(source.Id))
            {
                Members.TryAdd(source.Id, source);
                source.Party = this;
                this.Leader = source;
                Party.SendPartyJoinMessage(source);

                source.Client.Send(new PartyUpdateMessage()
                {
                    memberInformations = source.GetPartyMemberInformations(),
                    partyId = Id,
                });

                OnInvited(invited, source);
            }
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
            if (Guests.ContainsKey(character.Id))
            {
                SendMembers(new PartyRefuseInvitationNotificationMessage()
                {
                    partyId = Id,
                    guestId = character.Id,
                });

                RemoveGuest(character);
            }
        }

        public void CancelInvitation(Character canceller, long guestId)
        {
            if (canceller == Leader)
            {
                Character guest = Guests.TryGetValue(guestId);

                if (guest != null)
                {
                    guest.Client.Send(new PartyInvitationCancelledForGuestMessage()
                    {
                        partyId = Id,
                        cancelerId = canceller.Id,
                    });

                    RemoveGuest(guest);

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
                this.Leader = Members.Values.FirstOrDefault(x => x.Id != this.Leader.Id);

                SendMembers(new PartyLeaderUpdateMessage()
                {
                    partyId = Id,
                    partyLeaderId = Leader.Id,
                });
            }
            else
            {
                if (Members.ContainsKey(character.Id))
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

            foreach (Character character in Members.Values)
            {
                character.Party = null;
            }
            foreach (Character character in Guests.Values)
            {
                character.GuestedParties.Remove(this);
            }

            PartyManager.Instance.Remove(this);
        }

        public void AddMember(Character character)
        {
            if (!Members.ContainsKey(character.Id))
            {
                Members.TryAdd(character.Id, character);
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
            if (Members.ContainsKey(character.Id))
            {
                Members.TryRemove(character.Id);
                character.Party = null;
                character.Client.Send(new PartyLeaveMessage()
                {
                    partyId = Id
                });
            }
        }

        public void AddGuest(Character character)
        {
            if (!Guests.ContainsKey(character.Id) && !Members.ContainsKey(character.Id))
            {
                Guests.TryAdd(character.Id, character);
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
            if (Guests.ContainsKey(character.Id))
            {
                Guests.TryRemove(character.Id);
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
            return this.Members.TryGetValue(characterId);
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
            foreach (var member in Members)
            {
                member.Value.Client.Send(message);
            }
        }
        public void SendGuests(NetworkMessage message)
        {
            foreach (var member in Guests)
            {
                member.Value.Client.Send(message);
            }
        }

        public void SendToAll(NetworkMessage message)
        {
            SendMembers(message);
            SendGuests(message);
        }

        public void OnInitiateFight(Character character, Fight fight)
        {
            SendMembers(new PartyMemberInStandardFightMessage()
            {
                memberAccountId = character.Client.Account.Id,
                fightId = (short)fight.Id,
                fightMap = character.Map.GetMapCoordinatesExtended(),
                memberId = character.Id,
                memberName = character.Name,
                partyId = Id,
                reason = 0,
                timeBeforeFightStart = fight.GetPlacementTimeLeft(),
            });
        }
    }
}
