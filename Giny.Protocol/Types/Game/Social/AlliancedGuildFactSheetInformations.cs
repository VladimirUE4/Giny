using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class AlliancedGuildFactSheetInformations : GuildInformations  
    { 
        public const ushort Id = 8863;
        public override ushort TypeId => Id;

        public BasicNamedAllianceInformations allianceInfos;

        public AlliancedGuildFactSheetInformations()
        {
        }
        public AlliancedGuildFactSheetInformations(BasicNamedAllianceInformations allianceInfos)
        {
            this.allianceInfos = allianceInfos;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            allianceInfos.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceInfos = new BasicNamedAllianceInformations();
            allianceInfos.Deserialize(reader);
        }


    }
}








