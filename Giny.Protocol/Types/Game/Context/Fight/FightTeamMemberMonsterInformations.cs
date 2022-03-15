using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTeamMemberMonsterInformations : FightTeamMemberInformations  
    { 
        public const ushort Id = 6386;
        public override ushort TypeId => Id;

        public int monsterId;
        public byte grade;

        public FightTeamMemberMonsterInformations()
        {
        }
        public FightTeamMemberMonsterInformations(int monsterId,byte grade)
        {
            this.monsterId = monsterId;
            this.grade = grade;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt((int)monsterId);
            if (grade < 0)
            {
                throw new Exception("Forbidden value (" + grade + ") on element grade.");
            }

            writer.WriteByte((byte)grade);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            monsterId = (int)reader.ReadInt();
            grade = (byte)reader.ReadByte();
            if (grade < 0)
            {
                throw new Exception("Forbidden value (" + grade + ") on element of FightTeamMemberMonsterInformations.grade.");
            }

        }


    }
}








