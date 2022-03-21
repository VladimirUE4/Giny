using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PresetSaveErrorMessage : NetworkMessage  
    { 
        public new const ushort Id = 2325;
        public override ushort MessageId => Id;

        public short presetId;
        public byte code;

        public PresetSaveErrorMessage()
        {
        }
        public PresetSaveErrorMessage(short presetId,byte code)
        {
            this.presetId = presetId;
            this.code = code;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)presetId);
            writer.WriteByte((byte)code);
        }
        public override void Deserialize(IDataReader reader)
        {
            presetId = (short)reader.ReadShort();
            code = (byte)reader.ReadByte();
            if (code < 0)
            {
                throw new System.Exception("Forbidden value (" + code + ") on element of PresetSaveSystem.ExceptionMessage.code.");
            }

        }


    }
}








