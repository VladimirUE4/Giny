using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangePlayerMultiCraftRequestMessage : ExchangeRequestMessage  
    { 
        public  const ushort Id = 6947;
        public override ushort MessageId => Id;

        public long target;
        public int skillId;

        public ExchangePlayerMultiCraftRequestMessage()
        {
        }
        public ExchangePlayerMultiCraftRequestMessage(long target,int skillId)
        {
            this.target = target;
            this.skillId = skillId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (target < 0 || target > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + target + ") on element target.");
            }

            writer.WriteVarLong((long)target);
            if (skillId < 0)
            {
                throw new Exception("Forbidden value (" + skillId + ") on element skillId.");
            }

            writer.WriteVarInt((int)skillId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            target = (long)reader.ReadVarUhLong();
            if (target < 0 || target > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + target + ") on element of ExchangePlayerMultiCraftRequestMessage.target.");
            }

            skillId = (int)reader.ReadVarUhInt();
            if (skillId < 0)
            {
                throw new Exception("Forbidden value (" + skillId + ") on element of ExchangePlayerMultiCraftRequestMessage.skillId.");
            }

        }


    }
}








