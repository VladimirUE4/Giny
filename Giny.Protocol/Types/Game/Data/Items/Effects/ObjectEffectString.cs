using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ObjectEffectString : ObjectEffect  
    { 
        public const ushort Id = 7628;
        public override ushort TypeId => Id;

        public string value;

        public ObjectEffectString()
        {
        }
        public ObjectEffectString(string value)
        {
            this.value = value;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)value);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = (string)reader.ReadUTF();
        }


    }
}








