using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeObjectModifiedMessage : ExchangeObjectMessage  
    { 
        public new const ushort Id = 2283;
        public override ushort MessageId => Id;

        public ObjectItem @object;

        public ExchangeObjectModifiedMessage()
        {
        }
        public ExchangeObjectModifiedMessage(ObjectItem @object)
        {
            this.@object = @object;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            @object.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            @object = new ObjectItem();
            @object.Deserialize(reader);
        }


    }
}








