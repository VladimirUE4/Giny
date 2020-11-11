using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class URLOpenMessage : NetworkMessage  
    { 
        public new const ushort Id = 1508;
        public override ushort MessageId => Id;

        public byte urlId;

        public URLOpenMessage()
        {
        }
        public URLOpenMessage(byte urlId)
        {
            this.urlId = urlId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (urlId < 0)
            {
                throw new Exception("Forbidden value (" + urlId + ") on element urlId.");
            }

            writer.WriteByte((byte)urlId);
        }
        public override void Deserialize(IDataReader reader)
        {
            urlId = (byte)reader.ReadByte();
            if (urlId < 0)
            {
                throw new Exception("Forbidden value (" + urlId + ") on element of URLOpenMessage.urlId.");
            }

        }


    }
}








