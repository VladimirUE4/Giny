using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ObjectUseMultipleMessage : ObjectUseMessage  
    { 
        public new const ushort Id = 4836;
        public override ushort MessageId => Id;

        public int quantity;

        public ObjectUseMultipleMessage()
        {
        }
        public ObjectUseMultipleMessage(int quantity)
        {
            this.quantity = quantity;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (quantity < 0)
            {
                throw new Exception("Forbidden value (" + quantity + ") on element quantity.");
            }

            writer.WriteVarInt((int)quantity);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            quantity = (int)reader.ReadVarUhInt();
            if (quantity < 0)
            {
                throw new Exception("Forbidden value (" + quantity + ") on element of ObjectUseMultipleMessage.quantity.");
            }

        }


    }
}








