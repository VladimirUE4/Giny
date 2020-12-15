using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayHumanoidInformations : GameRolePlayNamedActorInformations  
    { 
        public const ushort Id = 1689;
        public override ushort TypeId => Id;

        public HumanInformations humanoidInfo;
        public int accountId;

        public GameRolePlayHumanoidInformations()
        {
        }
        public GameRolePlayHumanoidInformations(HumanInformations humanoidInfo,int accountId)
        {
            this.humanoidInfo = humanoidInfo;
            this.accountId = accountId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)humanoidInfo.TypeId);
            humanoidInfo.Serialize(writer);
            if (accountId < 0)
            {
                throw new Exception("Forbidden value (" + accountId + ") on element accountId.");
            }

            writer.WriteInt((int)accountId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            uint _id1 = (uint)reader.ReadUShort();
            humanoidInfo = ProtocolTypeManager.GetInstance<HumanInformations>((short)_id1);
            humanoidInfo.Deserialize(reader);
            accountId = (int)reader.ReadInt();
            if (accountId < 0)
            {
                throw new Exception("Forbidden value (" + accountId + ") on element of GameRolePlayHumanoidInformations.accountId.");
            }

        }


    }
}








