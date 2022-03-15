using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ObtainedItemWithBonusMessage : ObtainedItemMessage  
    { 
        public  const ushort Id = 7390;
        public override ushort MessageId => Id;

        public int bonusQuantity;

        public ObtainedItemWithBonusMessage()
        {
        }
        public ObtainedItemWithBonusMessage(int bonusQuantity)
        {
            this.bonusQuantity = bonusQuantity;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (bonusQuantity < 0)
            {
                throw new Exception("Forbidden value (" + bonusQuantity + ") on element bonusQuantity.");
            }

            writer.WriteVarInt((int)bonusQuantity);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            bonusQuantity = (int)reader.ReadVarUhInt();
            if (bonusQuantity < 0)
            {
                throw new Exception("Forbidden value (" + bonusQuantity + ") on element of ObtainedItemWithBonusMessage.bonusQuantity.");
            }

        }


    }
}








