using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class AlliancePrismInformation : PrismInformation  
    { 
        public const ushort Id = 1469;
        public override ushort TypeId => Id;

        public AllianceInformations alliance;

        public AlliancePrismInformation()
        {
        }
        public AlliancePrismInformation(AllianceInformations alliance)
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
            alliance = new AllianceInformations();
            alliance.Deserialize(reader);
        }


    }
}








