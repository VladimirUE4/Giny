using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterMinimalGuildInformations : CharacterMinimalPlusLookInformations  
    { 
        public const ushort Id = 5177;
        public override ushort TypeId => Id;

        public BasicGuildInformations guild;

        public CharacterMinimalGuildInformations()
        {
        }
        public CharacterMinimalGuildInformations(BasicGuildInformations guild)
        {
            this.guild = guild;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guild.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guild = new BasicGuildInformations();
            guild.Deserialize(reader);
        }


    }
}








