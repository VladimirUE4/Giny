using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTemporarySpellBoostEffect : FightTemporaryBoostEffect  
    { 
        public const ushort Id = 846;
        public override ushort TypeId => Id;

        public short boostedSpellId;

        public FightTemporarySpellBoostEffect()
        {
        }
        public FightTemporarySpellBoostEffect(short boostedSpellId)
        {
            this.boostedSpellId = boostedSpellId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (boostedSpellId < 0)
            {
                throw new Exception("Forbidden value (" + boostedSpellId + ") on element boostedSpellId.");
            }

            writer.WriteVarShort((short)boostedSpellId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            boostedSpellId = (short)reader.ReadVarUhShort();
            if (boostedSpellId < 0)
            {
                throw new Exception("Forbidden value (" + boostedSpellId + ") on element of FightTemporarySpellBoostEffect.boostedSpellId.");
            }

        }


    }
}








