using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeCraftResultWithObjectDescMessage : ExchangeCraftResultMessage  
    { 
        public  const ushort Id = 118;
        public override ushort MessageId => Id;

        public ObjectItemNotInContainer objectInfo;

        public ExchangeCraftResultWithObjectDescMessage()
        {
        }
        public ExchangeCraftResultWithObjectDescMessage(ObjectItemNotInContainer objectInfo)
        {
            this.objectInfo = objectInfo;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            objectInfo.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectInfo = new ObjectItemNotInContainer();
            objectInfo.Deserialize(reader);
        }


    }
}








