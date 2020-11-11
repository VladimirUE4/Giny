using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class FriendDeleteResultMessage : NetworkMessage  
    { 
        public new const ushort Id = 4241;
        public override ushort MessageId => Id;

        public bool success;
        public string name;

        public FriendDeleteResultMessage()
        {
        }
        public FriendDeleteResultMessage(bool success,string name)
        {
            this.success = success;
            this.name = name;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean((bool)success);
            writer.WriteUTF((string)name);
        }
        public override void Deserialize(IDataReader reader)
        {
            success = (bool)reader.ReadBoolean();
            name = (string)reader.ReadUTF();
        }


    }
}








