using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTemporarySpellImmunityEffect : AbstractFightDispellableEffect  
    { 
        public const ushort Id = 4141;
        public override ushort TypeId => Id;

        public int immuneSpellId;

        public FightTemporarySpellImmunityEffect()
        {
        }
        public FightTemporarySpellImmunityEffect(int immuneSpellId)
        {
            this.immuneSpellId = immuneSpellId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt((int)immuneSpellId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            immuneSpellId = (int)reader.ReadInt();
        }


    }
}








