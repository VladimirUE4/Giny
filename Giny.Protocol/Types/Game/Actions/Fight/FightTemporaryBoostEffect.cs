using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTemporaryBoostEffect : AbstractFightDispellableEffect  
    { 
        public const ushort Id = 7366;
        public override ushort TypeId => Id;

        public int delta;

        public FightTemporaryBoostEffect()
        {
        }
        public FightTemporaryBoostEffect(int delta)
        {
            this.delta = delta;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt((int)delta);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            delta = (int)reader.ReadInt();
        }


    }
}








