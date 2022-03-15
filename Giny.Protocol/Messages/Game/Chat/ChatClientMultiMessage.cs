using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ChatClientMultiMessage : ChatAbstractClientMessage  
    { 
        public  const ushort Id = 1382;
        public override ushort MessageId => Id;

        public byte channel;

        public ChatClientMultiMessage()
        {
        }
        public ChatClientMultiMessage(byte channel)
        {
            this.channel = channel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte((byte)channel);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            channel = (byte)reader.ReadByte();
            if (channel < 0)
            {
                throw new Exception("Forbidden value (" + channel + ") on element of ChatClientMultiMessage.channel.");
            }

        }


    }
}








