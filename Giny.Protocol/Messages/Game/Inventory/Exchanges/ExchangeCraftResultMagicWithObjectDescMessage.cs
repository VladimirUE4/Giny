using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeCraftResultMagicWithObjectDescMessage : ExchangeCraftResultWithObjectDescMessage  
    { 
        public  const ushort Id = 6242;
        public override ushort MessageId => Id;

        public byte magicPoolStatus;

        public ExchangeCraftResultMagicWithObjectDescMessage()
        {
        }
        public ExchangeCraftResultMagicWithObjectDescMessage(byte magicPoolStatus)
        {
            this.magicPoolStatus = magicPoolStatus;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte((byte)magicPoolStatus);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            magicPoolStatus = (byte)reader.ReadByte();
        }


    }
}








