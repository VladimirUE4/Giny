using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeObjectRemovedFromBagMessage : ExchangeObjectMessage  
    { 
        public new const ushort Id = 1268;
        public override ushort MessageId => Id;

        public int objectUID;

        public ExchangeObjectRemovedFromBagMessage()
        {
        }
        public ExchangeObjectRemovedFromBagMessage(int objectUID)
        {
            this.objectUID = objectUID;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (objectUID < 0)
            {
                throw new Exception("Forbidden value (" + objectUID + ") on element objectUID.");
            }

            writer.WriteVarInt((int)objectUID);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectUID = (int)reader.ReadVarUhInt();
            if (objectUID < 0)
            {
                throw new Exception("Forbidden value (" + objectUID + ") on element of ExchangeObjectRemovedFromBagMessage.objectUID.");
            }

        }


    }
}








