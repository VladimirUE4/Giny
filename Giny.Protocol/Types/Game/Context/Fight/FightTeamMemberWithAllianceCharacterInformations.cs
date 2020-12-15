using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTeamMemberWithAllianceCharacterInformations : FightTeamMemberCharacterInformations  
    { 
        public const ushort Id = 9492;
        public override ushort TypeId => Id;

        public BasicAllianceInformations allianceInfos;

        public FightTeamMemberWithAllianceCharacterInformations()
        {
        }
        public FightTeamMemberWithAllianceCharacterInformations(BasicAllianceInformations allianceInfos)
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
            allianceInfos = new BasicAllianceInformations();
            allianceInfos.Deserialize(reader);
        }


    }
}








