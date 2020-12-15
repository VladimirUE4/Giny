using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class TitleGainedMessage : NetworkMessage  
    { 
        public new const ushort Id = 9232;
        public override ushort MessageId => Id;

        public short titleId;

        public TitleGainedMessage()
        {
        }
        public TitleGainedMessage(short titleId)
        {
            this.titleId = titleId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (titleId < 0)
            {
                throw new Exception("Forbidden value (" + titleId + ") on element titleId.");
            }

            writer.WriteVarShort((short)titleId);
        }
        public override void Deserialize(IDataReader reader)
        {
            titleId = (short)reader.ReadVarUhShort();
            if (titleId < 0)
            {
                throw new Exception("Forbidden value (" + titleId + ") on element of TitleGainedMessage.titleId.");
            }

        }


    }
}








