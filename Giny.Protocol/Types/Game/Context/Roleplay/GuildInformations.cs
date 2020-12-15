using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GuildInformations : BasicGuildInformations  
    { 
        public const ushort Id = 5897;
        public override ushort TypeId => Id;

        public GuildEmblem guildEmblem;

        public GuildInformations()
        {
        }
        public GuildInformations(GuildEmblem guildEmblem)
        {
            this.guildEmblem = guildEmblem;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildEmblem.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildEmblem = new GuildEmblem();
            guildEmblem.Deserialize(reader);
        }


    }
}








