using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeBidHouseGenericItemAddedMessage : NetworkMessage  
    { 
        public new const ushort Id = 9923;
        public override ushort MessageId => Id;

        public short objGenericId;

        public ExchangeBidHouseGenericItemAddedMessage()
        {
        }
        public ExchangeBidHouseGenericItemAddedMessage(short objGenericId)
        {
            this.objGenericId = objGenericId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (objGenericId < 0)
            {
                throw new Exception("Forbidden value (" + objGenericId + ") on element objGenericId.");
            }

            writer.WriteVarShort((short)objGenericId);
        }
        public override void Deserialize(IDataReader reader)
        {
            objGenericId = (short)reader.ReadVarUhShort();
            if (objGenericId < 0)
            {
                throw new Exception("Forbidden value (" + objGenericId + ") on element of ExchangeBidHouseGenericItemAddedMessage.objGenericId.");
            }

        }


    }
}








