using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ForgettableSpellItem : SpellItem  
    { 
        public const ushort Id = 9108;
        public override ushort TypeId => Id;

        public bool available;

        public ForgettableSpellItem()
        {
        }
        public ForgettableSpellItem(bool available)
        {
            this.available = available;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean((bool)available);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            available = (bool)reader.ReadBoolean();
        }


    }
}








