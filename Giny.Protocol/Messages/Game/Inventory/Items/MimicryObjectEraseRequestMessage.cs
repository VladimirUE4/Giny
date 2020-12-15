using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MimicryObjectEraseRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 7469;
        public override ushort MessageId => Id;

        public int hostUID;
        public byte hostPos;

        public MimicryObjectEraseRequestMessage()
        {
        }
        public MimicryObjectEraseRequestMessage(int hostUID,byte hostPos)
        {
            this.hostUID = hostUID;
            this.hostPos = hostPos;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (hostUID < 0)
            {
                throw new Exception("Forbidden value (" + hostUID + ") on element hostUID.");
            }

            writer.WriteVarInt((int)hostUID);
            if (hostPos < 0 || hostPos > 255)
            {
                throw new Exception("Forbidden value (" + hostPos + ") on element hostPos.");
            }

            writer.WriteByte((byte)hostPos);
        }
        public override void Deserialize(IDataReader reader)
        {
            hostUID = (int)reader.ReadVarUhInt();
            if (hostUID < 0)
            {
                throw new Exception("Forbidden value (" + hostUID + ") on element of MimicryObjectEraseRequestMessage.hostUID.");
            }

            hostPos = (byte)reader.ReadSByte();
            if (hostPos < 0 || hostPos > 255)
            {
                throw new Exception("Forbidden value (" + hostPos + ") on element of MimicryObjectEraseRequestMessage.hostPos.");
            }

        }


    }
}








