using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ChannelEnablingChangeMessage : NetworkMessage  
    { 
        public new const ushort Id = 3541;
        public override ushort MessageId => Id;

        public byte channel;
        public bool enable;

        public ChannelEnablingChangeMessage()
        {
        }
        public ChannelEnablingChangeMessage(byte channel,bool enable)
        {
            this.channel = channel;
            this.enable = enable;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte((byte)channel);
            writer.WriteBoolean((bool)enable);
        }
        public override void Deserialize(IDataReader reader)
        {
            channel = (byte)reader.ReadByte();
            if (channel < 0)
            {
                throw new Exception("Forbidden value (" + channel + ") on element of ChannelEnablingChangeMessage.channel.");
            }

            enable = (bool)reader.ReadBoolean();
        }


    }
}








