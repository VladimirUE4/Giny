using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class SpawnMonsterInformation : BaseSpawnMonsterInformation  
    { 
        public const ushort Id = 3120;
        public override ushort TypeId => Id;

        public byte creatureGrade;

        public SpawnMonsterInformation()
        {
        }
        public SpawnMonsterInformation(byte creatureGrade)
        {
            this.creatureGrade = creatureGrade;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (creatureGrade < 0)
            {
                throw new Exception("Forbidden value (" + creatureGrade + ") on element creatureGrade.");
            }

            writer.WriteByte((byte)creatureGrade);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            creatureGrade = (byte)reader.ReadByte();
            if (creatureGrade < 0)
            {
                throw new Exception("Forbidden value (" + creatureGrade + ") on element of SpawnMonsterInformation.creatureGrade.");
            }

        }


    }
}








