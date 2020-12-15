using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MountEquipedErrorMessage : NetworkMessage  
    { 
        public new const ushort Id = 3527;
        public override ushort MessageId => Id;

        public byte errorType;

        public MountEquipedErrorMessage()
        {
        }
        public MountEquipedErrorMessage(byte errorType)
        {
            this.errorType = errorType;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte((byte)errorType);
        }
        public override void Deserialize(IDataReader reader)
        {
            errorType = (byte)reader.ReadByte();
            if (errorType < 0)
            {
                throw new Exception("Forbidden value (" + errorType + ") on element of MountEquipedExceptionMessage.errorType.");
            }

        }


    }
}








