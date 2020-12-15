using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterBaseInformations : CharacterMinimalPlusLookInformations  
    { 
        public const ushort Id = 9272;
        public override ushort TypeId => Id;

        public bool sex;

        public CharacterBaseInformations()
        {
        }
        public CharacterBaseInformations(bool sex)
        {
            this.sex = sex;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean((bool)sex);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            sex = (bool)reader.ReadBoolean();
        }


    }
}








