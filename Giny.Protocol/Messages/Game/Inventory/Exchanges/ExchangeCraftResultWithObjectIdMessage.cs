using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeCraftResultWithObjectIdMessage : ExchangeCraftResultMessage  
    { 
        public new const ushort Id = 9309;
        public override ushort MessageId => Id;

        public short objectGenericId;

        public ExchangeCraftResultWithObjectIdMessage()
        {
        }
        public ExchangeCraftResultWithObjectIdMessage(short objectGenericId)
        {
            this.objectGenericId = objectGenericId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (objectGenericId < 0)
            {
                throw new Exception("Forbidden value (" + objectGenericId + ") on element objectGenericId.");
            }

            writer.WriteVarShort((short)objectGenericId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectGenericId = (short)reader.ReadVarUhShort();
            if (objectGenericId < 0)
            {
                throw new Exception("Forbidden value (" + objectGenericId + ") on element of ExchangeCraftResultWithObjectIdMessage.objectGenericId.");
            }

        }


    }
}








