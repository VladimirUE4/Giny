using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class IconNamedPreset : PresetsContainerPreset  
    { 
        public const ushort Id = 7810;
        public override ushort TypeId => Id;

        public short iconId;
        public string name;

        public IconNamedPreset()
        {
        }
        public IconNamedPreset(short iconId,string name)
        {
            this.iconId = iconId;
            this.name = name;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (iconId < 0)
            {
                throw new Exception("Forbidden value (" + iconId + ") on element iconId.");
            }

            writer.WriteShort((short)iconId);
            writer.WriteUTF((string)name);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            iconId = (short)reader.ReadShort();
            if (iconId < 0)
            {
                throw new Exception("Forbidden value (" + iconId + ") on element of IconNamedPreset.iconId.");
            }

            name = (string)reader.ReadUTF();
        }


    }
}








