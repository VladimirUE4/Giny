using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class LocalizedChatSmileyMessage : ChatSmileyMessage  
    { 
        public  const ushort Id = 5134;
        public override ushort MessageId => Id;

        public short cellId;

        public LocalizedChatSmileyMessage()
        {
        }
        public LocalizedChatSmileyMessage(short cellId)
        {
            this.cellId = cellId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (cellId < 0 || cellId > 559)
            {
                throw new Exception("Forbidden value (" + cellId + ") on element cellId.");
            }

            writer.WriteVarShort((short)cellId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cellId = (short)reader.ReadVarUhShort();
            if (cellId < 0 || cellId > 559)
            {
                throw new Exception("Forbidden value (" + cellId + ") on element of LocalizedChatSmileyMessage.cellId.");
            }

        }


    }
}








