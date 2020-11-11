using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayGroupMonsterInformations : GameRolePlayActorInformations  
    { 
        public const ushort Id = 9493;
        public override ushort TypeId => Id;

        public GroupMonsterStaticInformations staticInfos;

        public byte lootShare;
        public byte alignmentSide;
        public bool keyRingBonus;
        public bool hasHardcoreDrop;
        public bool hasAVARewardToken;

        public GameRolePlayGroupMonsterInformations()
        {
        }
        public GameRolePlayGroupMonsterInformations(byte lootShare,byte alignmentSide,bool keyRingBonus,bool hasHardcoreDrop,bool hasAVARewardToken)
        {
            this.lootShare = lootShare;
            this.alignmentSide = alignmentSide;
            this.keyRingBonus = keyRingBonus;
            this.hasHardcoreDrop = hasHardcoreDrop;
            this.hasAVARewardToken = hasAVARewardToken;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            byte _box0 = 0;
            _box0 = BooleanByteWrapper.SetFlag(_box0,0,keyRingBonus);
            _box0 = BooleanByteWrapper.SetFlag(_box0,1,hasHardcoreDrop);
            _box0 = BooleanByteWrapper.SetFlag(_box0,2,hasAVARewardToken);
            writer.WriteByte((byte)_box0);
            writer.WriteShort((short)staticInfos.TypeId);
            staticInfos.Serialize(writer);
            if (lootShare < -1 || lootShare > 8)
            {
                throw new Exception("Forbidden value (" + lootShare + ") on element lootShare.");
            }

            writer.WriteByte((byte)lootShare);
            writer.WriteByte((byte)alignmentSide);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            byte _box0 = reader.ReadByte();
            keyRingBonus = BooleanByteWrapper.GetFlag(_box0,0);
            hasHardcoreDrop = BooleanByteWrapper.GetFlag(_box0,1);
            hasAVARewardToken = BooleanByteWrapper.GetFlag(_box0,2);
            uint _id1 = (uint)reader.ReadUShort();
            staticInfos = ProtocolTypeManager.GetInstance<GroupMonsterStaticInformations>((short)_id1);
            staticInfos.Deserialize(reader);
            lootShare = (byte)reader.ReadByte();
            if (lootShare < -1 || lootShare > 8)
            {
                throw new Exception("Forbidden value (" + lootShare + ") on element of GameRolePlayGroupMonsterInformations.lootShare.");
            }

            alignmentSide = (byte)reader.ReadByte();
        }


    }
}








