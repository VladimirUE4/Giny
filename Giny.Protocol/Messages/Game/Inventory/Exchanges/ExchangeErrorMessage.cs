using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeErrorMessage : NetworkMessage  
    { 
        public new const ushort Id = 6446;
        public override ushort MessageId => Id;

        public byte errorType;

        public ExchangeErrorMessage()
        {
        }
        public ExchangeErrorMessage(byte errorType)
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
        }


    }
}








