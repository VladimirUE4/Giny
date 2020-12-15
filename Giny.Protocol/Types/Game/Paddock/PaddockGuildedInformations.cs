using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class PaddockGuildedInformations : PaddockBuyableInformations  
    { 
        public const ushort Id = 355;
        public override ushort TypeId => Id;

        public bool deserted;
        public GuildInformations guildInfo;

        public PaddockGuildedInformations()
        {
        }
        public PaddockGuildedInformations(bool deserted,GuildInformations guildInfo)
        {
            this.deserted = deserted;
            this.guildInfo = guildInfo;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean((bool)deserted);
            guildInfo.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            deserted = (bool)reader.ReadBoolean();
            guildInfo = new GuildInformations();
            guildInfo.Deserialize(reader);
        }


    }
}








