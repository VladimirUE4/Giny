using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeMountsStableBornAddMessage : ExchangeMountsStableAddMessage  
    { 
        public  const ushort Id = 1340;
        public override ushort MessageId => Id;


        public ExchangeMountsStableBornAddMessage()
        {
        }
        public ExchangeMountsStableBornAddMessage(MountClientData[] mountDescription)
        {
            this.mountDescription = mountDescription;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }


    }
}








