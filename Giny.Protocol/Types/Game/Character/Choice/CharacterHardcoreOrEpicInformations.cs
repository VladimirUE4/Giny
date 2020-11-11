using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterHardcoreOrEpicInformations : CharacterBaseInformations  
    { 
        public const ushort Id = 2827;
        public override ushort TypeId => Id;

        public byte deathState;
        public short deathCount;
        public short deathMaxLevel;

        public CharacterHardcoreOrEpicInformations()
        {
        }
        public CharacterHardcoreOrEpicInformations(byte deathState,short deathCount,short deathMaxLevel)
        {
            this.deathState = deathState;
            this.deathCount = deathCount;
            this.deathMaxLevel = deathMaxLevel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte((byte)deathState);
            if (deathCount < 0)
            {
                throw new Exception("Forbidden value (" + deathCount + ") on element deathCount.");
            }

            writer.WriteVarShort((short)deathCount);
            if (deathMaxLevel < 0)
            {
                throw new Exception("Forbidden value (" + deathMaxLevel + ") on element deathMaxLevel.");
            }

            writer.WriteVarShort((short)deathMaxLevel);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            deathState = (byte)reader.ReadByte();
            if (deathState < 0)
            {
                throw new Exception("Forbidden value (" + deathState + ") on element of CharacterHardcoreOrEpicInformations.deathState.");
            }

            deathCount = (short)reader.ReadVarUhShort();
            if (deathCount < 0)
            {
                throw new Exception("Forbidden value (" + deathCount + ") on element of CharacterHardcoreOrEpicInformations.deathCount.");
            }

            deathMaxLevel = (short)reader.ReadVarUhShort();
            if (deathMaxLevel < 0)
            {
                throw new Exception("Forbidden value (" + deathMaxLevel + ") on element of CharacterHardcoreOrEpicInformations.deathMaxLevel.");
            }

        }


    }
}








