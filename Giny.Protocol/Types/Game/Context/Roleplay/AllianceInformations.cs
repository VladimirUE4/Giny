using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class AllianceInformations : BasicNamedAllianceInformations  
    { 
        public const ushort Id = 9961;
        public override ushort TypeId => Id;

        public GuildEmblem allianceEmblem;

        public AllianceInformations()
        {
        }
        public AllianceInformations(GuildEmblem allianceEmblem)
        {
            this.allianceEmblem = allianceEmblem;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            allianceEmblem.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceEmblem = new GuildEmblem();
            allianceEmblem.Deserialize(reader);
        }


    }
}








