using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class OrnamentGainedMessage : NetworkMessage  
    { 
        public  const ushort Id = 3920;
        public override ushort MessageId => Id;

        public short ornamentId;

        public OrnamentGainedMessage()
        {
        }
        public OrnamentGainedMessage(short ornamentId)
        {
            this.ornamentId = ornamentId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (ornamentId < 0)
            {
                throw new System.Exception("Forbidden value (" + ornamentId + ") on element ornamentId.");
            }

            writer.WriteShort((short)ornamentId);
        }
        public override void Deserialize(IDataReader reader)
        {
            ornamentId = (short)reader.ReadShort();
            if (ornamentId < 0)
            {
                throw new System.Exception("Forbidden value (" + ornamentId + ") on element of OrnamentGainedMessage.ornamentId.");
            }

        }


    }
}








