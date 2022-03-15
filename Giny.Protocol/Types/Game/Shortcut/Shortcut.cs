using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class Shortcut  
    { 
        public const ushort Id = 5511;
        public virtual ushort TypeId => Id;

        public byte slot;

        public Shortcut()
        {
        }
        public Shortcut(byte slot)
        {
            this.slot = slot;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            if (slot < 0 || slot > 99)
            {
                throw new Exception("Forbidden value (" + slot + ") on element slot.");
            }

            writer.WriteByte((byte)slot);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            slot = (byte)reader.ReadByte();
            if (slot < 0 || slot > 99)
            {
                throw new Exception("Forbidden value (" + slot + ") on element of Shortcut.slot.");
            }

        }


    }
}








