using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterMinimalGuildPublicInformations : CharacterMinimalInformations  
    { 
        public const ushort Id = 6767;
        public override ushort TypeId => Id;

        public int rank;

        public CharacterMinimalGuildPublicInformations()
        {
        }
        public CharacterMinimalGuildPublicInformations(int rank)
        {
            this.rank = rank;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (rank < 0)
            {
                throw new Exception("Forbidden value (" + rank + ") on element rank.");
            }

            writer.WriteVarInt((int)rank);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            rank = (int)reader.ReadVarUhInt();
            if (rank < 0)
            {
                throw new Exception("Forbidden value (" + rank + ") on element of CharacterMinimalGuildPublicInformations.rank.");
            }

        }


    }
}








