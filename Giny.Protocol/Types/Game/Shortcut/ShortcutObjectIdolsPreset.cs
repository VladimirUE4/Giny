using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ShortcutObjectIdolsPreset : ShortcutObject  
    { 
        public const ushort Id = 6872;
        public override ushort TypeId => Id;

        public short presetId;

        public ShortcutObjectIdolsPreset()
        {
        }
        public ShortcutObjectIdolsPreset(short presetId)
        {
            this.presetId = presetId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)presetId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            presetId = (short)reader.ReadShort();
        }


    }
}








