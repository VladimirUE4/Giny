using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class HouseGuildedInformations : HouseInstanceInformations  
    { 
        public const ushort Id = 856;
        public override ushort TypeId => Id;

        public GuildInformations guildInfo;

        public HouseGuildedInformations()
        {
        }
        public HouseGuildedInformations(GuildInformations guildInfo)
        {
            this.guildInfo = guildInfo;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildInfo.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildInfo = new GuildInformations();
            guildInfo.Deserialize(reader);
        }


    }
}








