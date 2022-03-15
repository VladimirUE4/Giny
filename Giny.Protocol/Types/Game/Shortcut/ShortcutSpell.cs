using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ShortcutSpell : Shortcut  
    { 
        public const ushort Id = 445;
        public override ushort TypeId => Id;

        public short spellId;

        public ShortcutSpell()
        {
        }
        public ShortcutSpell(short spellId)
        {
            this.spellId = spellId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (spellId < 0)
            {
                throw new Exception("Forbidden value (" + spellId + ") on element spellId.");
            }

            writer.WriteVarShort((short)spellId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spellId = (short)reader.ReadVarUhShort();
            if (spellId < 0)
            {
                throw new Exception("Forbidden value (" + spellId + ") on element of ShortcutSpell.spellId.");
            }

        }


    }
}








