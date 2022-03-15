using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTemporaryBoostStateEffect : FightTemporaryBoostEffect  
    { 
        public const ushort Id = 8281;
        public override ushort TypeId => Id;

        public short stateId;

        public FightTemporaryBoostStateEffect()
        {
        }
        public FightTemporaryBoostStateEffect(short stateId)
        {
            this.stateId = stateId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)stateId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            stateId = (short)reader.ReadShort();
        }


    }
}








