using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class PartyMemberArenaInformations : PartyMemberInformations  
    { 
        public const ushort Id = 2694;
        public override ushort TypeId => Id;

        public short rank;

        public PartyMemberArenaInformations()
        {
        }
        public PartyMemberArenaInformations(short rank)
        {
            this.rank = rank;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (rank < 0 || rank > 20000)
            {
                throw new Exception("Forbidden value (" + rank + ") on element rank.");
            }

            writer.WriteVarShort((short)rank);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            rank = (short)reader.ReadVarUhShort();
            if (rank < 0 || rank > 20000)
            {
                throw new Exception("Forbidden value (" + rank + ") on element of PartyMemberArenaInformations.rank.");
            }

        }


    }
}








