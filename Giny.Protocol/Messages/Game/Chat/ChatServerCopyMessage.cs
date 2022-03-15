using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ChatServerCopyMessage : ChatAbstractServerMessage  
    { 
        public  const ushort Id = 5344;
        public override ushort MessageId => Id;

        public long receiverId;
        public string receiverName;

        public ChatServerCopyMessage()
        {
        }
        public ChatServerCopyMessage(long receiverId,string receiverName)
        {
            this.receiverId = receiverId;
            this.receiverName = receiverName;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (receiverId < 0 || receiverId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + receiverId + ") on element receiverId.");
            }

            writer.WriteVarLong((long)receiverId);
            writer.WriteUTF((string)receiverName);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            receiverId = (long)reader.ReadVarUhLong();
            if (receiverId < 0 || receiverId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + receiverId + ") on element of ChatServerCopyMessage.receiverId.");
            }

            receiverName = (string)reader.ReadUTF();
        }


    }
}








