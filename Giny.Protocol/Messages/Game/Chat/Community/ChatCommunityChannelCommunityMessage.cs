using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ChatCommunityChannelCommunityMessage : NetworkMessage  
    { 
        public new const ushort Id = 7105;
        public override ushort MessageId => Id;

        public short communityId;

        public ChatCommunityChannelCommunityMessage()
        {
        }
        public ChatCommunityChannelCommunityMessage(short communityId)
        {
            this.communityId = communityId;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)communityId);
        }
        public override void Deserialize(IDataReader reader)
        {
            communityId = (short)reader.ReadShort();
        }


    }
}








