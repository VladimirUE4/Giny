using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTeamLightInformations : AbstractFightTeamInformations  
    { 
        public const ushort Id = 166;
        public override ushort TypeId => Id;

        public byte teamMembersCount;
        public int meanLevel;
        public bool hasFriend;
        public bool hasGuildMember;
        public bool hasAllianceMember;
        public bool hasGroupMember;
        public bool hasMyTaxCollector;

        public FightTeamLightInformations()
        {
        }
        public FightTeamLightInformations(byte teamMembersCount,int meanLevel,bool hasFriend,bool hasGuildMember,bool hasAllianceMember,bool hasGroupMember,bool hasMyTaxCollector)
        {
            this.teamMembersCount = teamMembersCount;
            this.meanLevel = meanLevel;
            this.hasFriend = hasFriend;
            this.hasGuildMember = hasGuildMember;
            this.hasAllianceMember = hasAllianceMember;
            this.hasGroupMember = hasGroupMember;
            this.hasMyTaxCollector = hasMyTaxCollector;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            byte _box0 = 0;
            _box0 = BooleanByteWrapper.SetFlag(_box0,0,hasFriend);
            _box0 = BooleanByteWrapper.SetFlag(_box0,1,hasGuildMember);
            _box0 = BooleanByteWrapper.SetFlag(_box0,2,hasAllianceMember);
            _box0 = BooleanByteWrapper.SetFlag(_box0,3,hasGroupMember);
            _box0 = BooleanByteWrapper.SetFlag(_box0,4,hasMyTaxCollector);
            writer.WriteByte((byte)_box0);
            if (teamMembersCount < 0)
            {
                throw new Exception("Forbidden value (" + teamMembersCount + ") on element teamMembersCount.");
            }

            writer.WriteByte((byte)teamMembersCount);
            if (meanLevel < 0)
            {
                throw new Exception("Forbidden value (" + meanLevel + ") on element meanLevel.");
            }

            writer.WriteVarInt((int)meanLevel);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            byte _box0 = reader.ReadByte();
            hasFriend = BooleanByteWrapper.GetFlag(_box0,0);
            hasGuildMember = BooleanByteWrapper.GetFlag(_box0,1);
            hasAllianceMember = BooleanByteWrapper.GetFlag(_box0,2);
            hasGroupMember = BooleanByteWrapper.GetFlag(_box0,3);
            hasMyTaxCollector = BooleanByteWrapper.GetFlag(_box0,4);
            teamMembersCount = (byte)reader.ReadByte();
            if (teamMembersCount < 0)
            {
                throw new Exception("Forbidden value (" + teamMembersCount + ") on element of FightTeamLightInformations.teamMembersCount.");
            }

            meanLevel = (int)reader.ReadVarUhInt();
            if (meanLevel < 0)
            {
                throw new Exception("Forbidden value (" + meanLevel + ") on element of FightTeamLightInformations.meanLevel.");
            }

        }


    }
}








