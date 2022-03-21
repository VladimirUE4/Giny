using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class AllianceMotdSetErrorMessage : SocialNoticeSetErrorMessage  
    { 
        public  const ushort Id = 9723;
        public override ushort MessageId => Id;


        public AllianceMotdSetErrorMessage()
        {
        }
        public AllianceMotdSetErrorMessage(byte reason)
        {
            this.reason = reason;
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








