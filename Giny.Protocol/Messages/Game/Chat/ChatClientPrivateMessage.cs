using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ChatClientPrivateMessage : ChatAbstractClientMessage  
    { 
        public new const ushort Id = 7158;
        public override ushort MessageId => Id;

        public string receiver;

        public ChatClientPrivateMessage()
        {
        }
        public ChatClientPrivateMessage(string receiver)
        {
            this.receiver = receiver;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)receiver);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            receiver = (string)reader.ReadUTF();
        }


    }
}








