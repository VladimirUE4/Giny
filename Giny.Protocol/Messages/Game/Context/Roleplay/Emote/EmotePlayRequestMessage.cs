using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class EmotePlayRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 9094;
        public override ushort MessageId => Id;

        public byte emoteId;

        public EmotePlayRequestMessage()
        {
        }
        public EmotePlayRequestMessage(byte emoteId)
        {
            this.emoteId = emoteId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (emoteId < 0 || emoteId > 255)
            {
                throw new Exception("Forbidden value (" + emoteId + ") on element emoteId.");
            }

            writer.WriteByte((byte)emoteId);
        }
        public override void Deserialize(IDataReader reader)
        {
            emoteId = (byte)reader.ReadSByte();
            if (emoteId < 0 || emoteId > 255)
            {
                throw new Exception("Forbidden value (" + emoteId + ") on element of EmotePlayRequestMessage.emoteId.");
            }

        }


    }
}








