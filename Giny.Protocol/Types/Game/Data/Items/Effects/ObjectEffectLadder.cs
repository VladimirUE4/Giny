using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ObjectEffectLadder : ObjectEffectCreature  
    { 
        public const ushort Id = 7384;
        public override ushort TypeId => Id;

        public int monsterCount;

        public ObjectEffectLadder()
        {
        }
        public ObjectEffectLadder(int monsterCount)
        {
            this.monsterCount = monsterCount;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (monsterCount < 0)
            {
                throw new Exception("Forbidden value (" + monsterCount + ") on element monsterCount.");
            }

            writer.WriteVarInt((int)monsterCount);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            monsterCount = (int)reader.ReadVarUhInt();
            if (monsterCount < 0)
            {
                throw new Exception("Forbidden value (" + monsterCount + ") on element of ObjectEffectLadder.monsterCount.");
            }

        }


    }
}








