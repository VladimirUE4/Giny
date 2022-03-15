using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyRestrictedMessage : AbstractPartyMessage  
    { 
        public  const ushort Id = 6433;
        public override ushort MessageId => Id;

        public bool restricted;

        public PartyRestrictedMessage()
        {
        }
        public PartyRestrictedMessage(bool restricted)
        {
            this.restricted = restricted;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean((bool)restricted);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            restricted = (bool)reader.ReadBoolean();
        }


    }
}








