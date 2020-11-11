using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightFighterMonsterLightInformations : GameFightFighterLightInformations  
    { 
        public const ushort Id = 1104;
        public override ushort TypeId => Id;

        public short creatureGenericId;

        public GameFightFighterMonsterLightInformations()
        {
        }
        public GameFightFighterMonsterLightInformations(short creatureGenericId)
        {
            this.creatureGenericId = creatureGenericId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (creatureGenericId < 0)
            {
                throw new Exception("Forbidden value (" + creatureGenericId + ") on element creatureGenericId.");
            }

            writer.WriteVarShort((short)creatureGenericId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            creatureGenericId = (short)reader.ReadVarUhShort();
            if (creatureGenericId < 0)
            {
                throw new Exception("Forbidden value (" + creatureGenericId + ") on element of GameFightFighterMonsterLightInformations.creatureGenericId.");
            }

        }


    }
}








