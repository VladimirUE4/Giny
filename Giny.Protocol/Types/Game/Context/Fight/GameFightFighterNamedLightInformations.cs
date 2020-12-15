using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightFighterNamedLightInformations : GameFightFighterLightInformations  
    { 
        public const ushort Id = 8213;
        public override ushort TypeId => Id;

        public string name;

        public GameFightFighterNamedLightInformations()
        {
        }
        public GameFightFighterNamedLightInformations(string name)
        {
            this.name = name;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)name);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = (string)reader.ReadUTF();
        }


    }
}








