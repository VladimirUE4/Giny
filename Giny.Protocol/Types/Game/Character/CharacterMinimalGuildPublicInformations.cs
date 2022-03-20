using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterMinimalGuildPublicInformations : CharacterMinimalInformations  
    { 
        public const ushort Id = 393;
        public override ushort TypeId => Id;

        public int rank;

        public CharacterMinimalGuildPublicInformations()
        {
        }
        public CharacterMinimalGuildPublicInformations(int rank,long id,string name,short level)
        {
            this.rank = rank;
            this.id = id;
            this.name = name;
            this.level = level;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (rank < 0)
            {
                throw new System.Exception("Forbidden value (" + rank + ") on element rank.");
            }

            writer.WriteVarInt((int)rank);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            rank = (int)reader.ReadVarUhInt();
            if (rank < 0)
            {
                throw new System.Exception("Forbidden value (" + rank + ") on element of CharacterMinimalGuildPublicInformations.rank.");
            }

        }


    }
}








