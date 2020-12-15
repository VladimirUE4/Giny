using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeShopStockMovementUpdatedMessage : NetworkMessage  
    { 
        public new const ushort Id = 3878;
        public override ushort MessageId => Id;

        public ObjectItemToSell objectInfo;

        public ExchangeShopStockMovementUpdatedMessage()
        {
        }
        public ExchangeShopStockMovementUpdatedMessage(ObjectItemToSell objectInfo)
        {
            this.objectInfo = objectInfo;
        }
        public override void Serialize(IDataWriter writer)
        {
            objectInfo.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            objectInfo = new ObjectItemToSell();
            objectInfo.Deserialize(reader);
        }


    }
}








