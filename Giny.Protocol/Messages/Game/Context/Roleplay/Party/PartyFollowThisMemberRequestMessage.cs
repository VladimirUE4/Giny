using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyFollowThisMemberRequestMessage : PartyFollowMemberRequestMessage  
    { 
        public new const ushort Id = 4725;
        public override ushort MessageId => Id;

        public bool enabled;

        public PartyFollowThisMemberRequestMessage()
        {
        }
        public PartyFollowThisMemberRequestMessage(bool enabled)
        {
            this.enabled = enabled;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean((bool)enabled);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            enabled = (bool)reader.ReadBoolean();
        }


    }
}








