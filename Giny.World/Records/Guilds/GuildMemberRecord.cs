using Giny.Protocol.Enums;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Network;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Guilds
{
    [ProtoContract]
    public class GuildMemberRecord
    {
        public bool Connected => WorldServer.Instance.IsOnline(CharacterId);

        [ProtoMember(1)]
        public long CharacterId
        {
            get;
            set;
        }
        [ProtoMember(2)]
        public short Rank
        {
            get;
            set;
        }
        [ProtoMember(3)]
        public long GivenExperience
        {
            get;
            set;
        }
        [ProtoMember(4)]
        public byte ExperienceGivenPercent
        {
            get;
            set;
        }
        [ProtoMember(5)]
        public GuildRightsBitEnum Rights
        {
            get;
            set;
        }
        [ProtoMember(6)]
        public short MoodSmileyId
        {
            get;
            set;
        }

        public GuildMemberRecord()
        {

        }
        public GuildMemberRecord(Character character, bool owner)
        {
            CharacterId = character.Id;
            ExperienceGivenPercent = 0;
            GivenExperience = 0;
            MoodSmileyId = 0;
            Rank = (short)(owner ? 1 : 0);
            Rights = owner ? GuildRightsBitEnum.GUILD_RIGHT_BOSS : GuildRightsBitEnum.GUILD_RIGHT_NONE;
        }
    }
}
