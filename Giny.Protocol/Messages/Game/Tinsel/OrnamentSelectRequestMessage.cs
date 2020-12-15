using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class OrnamentSelectRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 2389;
        public override ushort MessageId => Id;

        public short ornamentId;

        public OrnamentSelectRequestMessage()
        {
        }
        public OrnamentSelectRequestMessage(short ornamentId)
        {
            this.ornamentId = ornamentId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (ornamentId < 0)
            {
                throw new Exception("Forbidden value (" + ornamentId + ") on element ornamentId.");
            }

            writer.WriteVarShort((short)ornamentId);
        }
        public override void Deserialize(IDataReader reader)
        {
            ornamentId = (short)reader.ReadVarUhShort();
            if (ornamentId < 0)
            {
                throw new Exception("Forbidden value (" + ornamentId + ") on element of OrnamentSelectRequestMessage.ornamentId.");
            }

        }


    }
}








