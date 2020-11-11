using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ObjectEffectCreature : ObjectEffect  
    { 
        public const ushort Id = 6596;
        public override ushort TypeId => Id;

        public short monsterFamilyId;

        public ObjectEffectCreature()
        {
        }
        public ObjectEffectCreature(short monsterFamilyId)
        {
            this.monsterFamilyId = monsterFamilyId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (monsterFamilyId < 0)
            {
                throw new Exception("Forbidden value (" + monsterFamilyId + ") on element monsterFamilyId.");
            }

            writer.WriteVarShort((short)monsterFamilyId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            monsterFamilyId = (short)reader.ReadVarUhShort();
            if (monsterFamilyId < 0)
            {
                throw new Exception("Forbidden value (" + monsterFamilyId + ") on element of ObjectEffectCreature.monsterFamilyId.");
            }

        }


    }
}








