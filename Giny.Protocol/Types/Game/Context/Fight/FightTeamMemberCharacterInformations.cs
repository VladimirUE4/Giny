using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTeamMemberCharacterInformations : FightTeamMemberInformations  
    { 
        public const ushort Id = 6593;
        public override ushort TypeId => Id;

        public string name;
        public short level;

        public FightTeamMemberCharacterInformations()
        {
        }
        public FightTeamMemberCharacterInformations(string name,short level)
        {
            this.name = name;
            this.level = level;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)name);
            if (level < 0)
            {
                throw new Exception("Forbidden value (" + level + ") on element level.");
            }

            writer.WriteVarShort((short)level);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = (string)reader.ReadUTF();
            level = (short)reader.ReadVarUhShort();
            if (level < 0)
            {
                throw new Exception("Forbidden value (" + level + ") on element of FightTeamMemberCharacterInformations.level.");
            }

        }


    }
}








