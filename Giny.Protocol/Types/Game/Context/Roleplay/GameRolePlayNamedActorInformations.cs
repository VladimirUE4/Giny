using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayNamedActorInformations : GameRolePlayActorInformations  
    { 
        public const ushort Id = 6486;
        public override ushort TypeId => Id;

        public string name;

        public GameRolePlayNamedActorInformations()
        {
        }
        public GameRolePlayNamedActorInformations(string name)
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








