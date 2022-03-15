using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterMinimalAllianceInformations : CharacterMinimalGuildInformations  
    { 
        public const ushort Id = 4354;
        public override ushort TypeId => Id;

        public BasicAllianceInformations alliance;

        public CharacterMinimalAllianceInformations()
        {
        }
        public CharacterMinimalAllianceInformations(BasicAllianceInformations alliance)
        {
            this.alliance = alliance;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            alliance.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            alliance = new BasicAllianceInformations();
            alliance.Deserialize(reader);
        }


    }
}








