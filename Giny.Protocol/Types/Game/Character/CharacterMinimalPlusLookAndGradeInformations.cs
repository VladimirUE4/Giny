using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterMinimalPlusLookAndGradeInformations : CharacterMinimalPlusLookInformations  
    { 
        public const ushort Id = 4536;
        public override ushort TypeId => Id;

        public int grade;

        public CharacterMinimalPlusLookAndGradeInformations()
        {
        }
        public CharacterMinimalPlusLookAndGradeInformations(int grade)
        {
            this.grade = grade;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (grade < 0)
            {
                throw new Exception("Forbidden value (" + grade + ") on element grade.");
            }

            writer.WriteVarInt((int)grade);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            grade = (int)reader.ReadVarUhInt();
            if (grade < 0)
            {
                throw new Exception("Forbidden value (" + grade + ") on element of CharacterMinimalPlusLookAndGradeInformations.grade.");
            }

        }


    }
}








