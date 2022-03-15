using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightMutantInformations : GameFightFighterNamedInformations  
    { 
        public const ushort Id = 2861;
        public override ushort TypeId => Id;

        public byte powerLevel;

        public GameFightMutantInformations()
        {
        }
        public GameFightMutantInformations(byte powerLevel)
        {
            this.powerLevel = powerLevel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (powerLevel < 0)
            {
                throw new Exception("Forbidden value (" + powerLevel + ") on element powerLevel.");
            }

            writer.WriteByte((byte)powerLevel);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            powerLevel = (byte)reader.ReadByte();
            if (powerLevel < 0)
            {
                throw new Exception("Forbidden value (" + powerLevel + ") on element of GameFightMutantInformations.powerLevel.");
            }

        }


    }
}








