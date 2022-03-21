using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class CharacterCreationResultMessage : NetworkMessage  
    { 
        public new const ushort Id = 110;
        public override ushort MessageId => Id;

        public byte result;

        public CharacterCreationResultMessage()
        {
        }
        public CharacterCreationResultMessage(byte result)
        {
            this.result = result;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte((byte)result);
        }
        public override void Deserialize(IDataReader reader)
        {
            result = (byte)reader.ReadByte();
            if (result < 0)
            {
                throw new System.Exception("Forbidden value (" + result + ") on element of CharacterCreationResultMessage.result.");
            }

        }


    }
}








