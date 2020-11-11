using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class SpawnScaledMonsterInformation : BaseSpawnMonsterInformation  
    { 
        public const ushort Id = 8592;
        public override ushort TypeId => Id;

        public short creatureLevel;

        public SpawnScaledMonsterInformation()
        {
        }
        public SpawnScaledMonsterInformation(short creatureLevel)
        {
            this.creatureLevel = creatureLevel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (creatureLevel < 0)
            {
                throw new Exception("Forbidden value (" + creatureLevel + ") on element creatureLevel.");
            }

            writer.WriteShort((short)creatureLevel);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            creatureLevel = (short)reader.ReadShort();
            if (creatureLevel < 0)
            {
                throw new Exception("Forbidden value (" + creatureLevel + ") on element of SpawnScaledMonsterInformation.creatureLevel.");
            }

        }


    }
}








