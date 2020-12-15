using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class FriendJoinRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 4156;
        public override ushort MessageId => Id;

        public string name;

        public FriendJoinRequestMessage()
        {
        }
        public FriendJoinRequestMessage(string name)
        {
            this.name = name;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF((string)name);
        }
        public override void Deserialize(IDataReader reader)
        {
            name = (string)reader.ReadUTF();
        }


    }
}








