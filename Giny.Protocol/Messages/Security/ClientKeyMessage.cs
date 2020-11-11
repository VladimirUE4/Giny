using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ClientKeyMessage : NetworkMessage  
    { 
        public new const ushort Id = 6268;
        public override ushort MessageId => Id;

        public string key;

        public ClientKeyMessage()
        {
        }
        public ClientKeyMessage(string key)
        {
            this.key = key;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF((string)key);
        }
        public override void Deserialize(IDataReader reader)
        {
            key = (string)reader.ReadUTF();
        }


    }
}








