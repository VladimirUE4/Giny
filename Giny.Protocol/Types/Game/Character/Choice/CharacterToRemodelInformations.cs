using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterToRemodelInformations : CharacterRemodelingInformation  
    { 
        public const ushort Id = 2646;
        public override ushort TypeId => Id;

        public byte possibleChangeMask;
        public byte mandatoryChangeMask;

        public CharacterToRemodelInformations()
        {
        }
        public CharacterToRemodelInformations(byte possibleChangeMask,byte mandatoryChangeMask)
        {
            this.possibleChangeMask = possibleChangeMask;
            this.mandatoryChangeMask = mandatoryChangeMask;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (possibleChangeMask < 0)
            {
                throw new Exception("Forbidden value (" + possibleChangeMask + ") on element possibleChangeMask.");
            }

            writer.WriteByte((byte)possibleChangeMask);
            if (mandatoryChangeMask < 0)
            {
                throw new Exception("Forbidden value (" + mandatoryChangeMask + ") on element mandatoryChangeMask.");
            }

            writer.WriteByte((byte)mandatoryChangeMask);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            possibleChangeMask = (byte)reader.ReadByte();
            if (possibleChangeMask < 0)
            {
                throw new Exception("Forbidden value (" + possibleChangeMask + ") on element of CharacterToRemodelInformations.possibleChangeMask.");
            }

            mandatoryChangeMask = (byte)reader.ReadByte();
            if (mandatoryChangeMask < 0)
            {
                throw new Exception("Forbidden value (" + mandatoryChangeMask + ") on element of CharacterToRemodelInformations.mandatoryChangeMask.");
            }

        }


    }
}








