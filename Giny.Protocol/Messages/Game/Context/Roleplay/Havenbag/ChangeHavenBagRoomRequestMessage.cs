using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ChangeHavenBagRoomRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 5103;
        public override ushort MessageId => Id;

        public byte roomId;

        public ChangeHavenBagRoomRequestMessage()
        {
        }
        public ChangeHavenBagRoomRequestMessage(byte roomId)
        {
            this.roomId = roomId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (roomId < 0)
            {
                throw new Exception("Forbidden value (" + roomId + ") on element roomId.");
            }

            writer.WriteByte((byte)roomId);
        }
        public override void Deserialize(IDataReader reader)
        {
            roomId = (byte)reader.ReadByte();
            if (roomId < 0)
            {
                throw new Exception("Forbidden value (" + roomId + ") on element of ChangeHavenBagRoomRequestMessage.roomId.");
            }

        }


    }
}








