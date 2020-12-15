using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterMinimalPlusLookInformations : CharacterMinimalInformations  
    { 
        public const ushort Id = 9017;
        public override ushort TypeId => Id;

        public EntityLook entityLook;
        public byte breed;

        public CharacterMinimalPlusLookInformations()
        {
        }
        public CharacterMinimalPlusLookInformations(EntityLook entityLook,byte breed)
        {
            this.entityLook = entityLook;
            this.breed = breed;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            entityLook.Serialize(writer);
            writer.WriteByte((byte)breed);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            entityLook = new EntityLook();
            entityLook.Deserialize(reader);
            breed = (byte)reader.ReadByte();
        }


    }
}








