using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class TaxCollectorStaticExtendedInformations : TaxCollectorStaticInformations  
    { 
        public const ushort Id = 6505;
        public override ushort TypeId => Id;

        public AllianceInformations allianceIdentity;

        public TaxCollectorStaticExtendedInformations()
        {
        }
        public TaxCollectorStaticExtendedInformations(AllianceInformations allianceIdentity)
        {
            this.allianceIdentity = allianceIdentity;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            allianceIdentity.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceIdentity = new AllianceInformations();
            allianceIdentity.Deserialize(reader);
        }


    }
}








