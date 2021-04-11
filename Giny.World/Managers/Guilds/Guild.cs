using Giny.Core.Extensions;
using Giny.Core.Network.Messages;
using Giny.ORM;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Experiences;
using Giny.World.Network;
using Giny.World.Records.Characters;
using Giny.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Guilds
{
    public class Guild
    {
        public long Id => Record.Id;

        public GuildRecord Record
        {
            get;
            private set;
        }
        private SynchronizedCollection<Character> OnlineMembers
        {
            get;
            set;
        }

        public byte Level => ExperienceManager.Instance.GetGuildLevel(Experience);

        public long ExperienceLowerBound => ExperienceManager.Instance.GetGuildXPForLevel(Level);

        public long ExperienceUpperBound => ExperienceManager.Instance.GetGuildXPForNextLevel(Level);



        public long Experience
        {
            get
            {
                return Record.Experience;
            }
            set
            {
                Record.Experience = value;
            }
        }

        public Guild(GuildRecord record)
        {
            this.Record = record;
            this.OnlineMembers = new SynchronizedCollection<Character>();
        }

        public bool Join(Character character, bool owner)
        {
            if (Record.Members.Count == GuildsManager.MaxMemberCount)
            {
                return false;
            }

            GuildMemberRecord memberRecord = new GuildMemberRecord(character, owner);
            Record.Members.Add(memberRecord);
            Record.UpdateElement();
            OnlineMembers.Add(character);
            character.OnGuildJoined(this, memberRecord);
            return true;
        }

        public void OnConnected(Character character)
        {
            OnlineMembers.Add(character);
            RefreshMotd(character);
        }
        public void OnDisconnected(Character character)
        {
            OnlineMembers.Remove(character);
        }

        public void SetMotd(Character source, string content)
        {
            if (content.Length > GuildsManager.MotdMaxLength)
            {
                return;
            }

            Record.Motd = new GuildMotd()
            {
                Content = content,
                MemberId = source.Id,
                Timestamp = DateTime.Now.GetUnixTimeStamp(),
                MemberName = source.Name,
            };

            Record.UpdateElement();

            RefreshMotd();

        }
        public void RefreshMotd()
        {
            foreach (var character in OnlineMembers)
            {
                RefreshMotd(character);
            }
        }
        public void RefreshMotd(Character member)
        {
            member.Client.Send(new GuildMotdMessage()
            {
                content = Record.Motd.Content,
                memberId = Record.Motd.MemberId,
                memberName = Record.Motd.MemberName,
                timestamp = Record.Motd.Timestamp,
            });
        }
        public GuildInformations GetGuildInformations()
        {
            return new GuildInformations()
            {
                guildEmblem = Record.Emblem.ToGuildEmblem(),
                guildId = (int)Id,
                guildLevel = Level,
                guildName = Record.Name,
            };
        }
        public void Send(NetworkMessage message)
        {
            foreach (var character in OnlineMembers)
            {
                character.Client.Send(message);
            }
        }
        public GuildInformationsGeneralMessage GetGuildInformationsGeneralMessage()
        {
            return new GuildInformationsGeneralMessage()
            {
                abandonnedPaddock = false,
                creationDate = Record.CreationDate.GetUnixTimeStamp(),
                experience = Record.Experience,
                expLevelFloor = ExperienceLowerBound,
                expNextLevelFloor = ExperienceUpperBound,
                level = Level,
                nbConnectedMembers = (short)OnlineMembers.Count,
                nbTotalMembers = (short)Record.Members.Count,
            };
        }
        public GuildInformationsMembersMessage GetGuildInformationsMembersMessage()
        {
            return new GuildInformationsMembersMessage()
            {
                members = Record.Members.Select(x => x.ToGuildMember()).ToArray(),
            };
        }
    }
}
