using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightAllianceTeamInformations : FightTeamInformations  
    { 
        public const ushort Id = 2398;
        public override ushort TypeId => Id;

        public byte relation;

        public FightAllianceTeamInformations()
        {
        }
        public FightAllianceTeamInformations(byte relation)
        {
            this.relation = relation;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte((byte)relation);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            relation = (byte)reader.ReadByte();
            if (relation < 0)
            {
                throw new Exception("Forbidden value (" + relation + ") on element of FightAllianceTeamInformations.relation.");
            }

        }


    }
}








