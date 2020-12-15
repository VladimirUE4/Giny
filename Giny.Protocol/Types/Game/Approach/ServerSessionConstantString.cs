using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ServerSessionConstantString : ServerSessionConstant  
    { 
        public const ushort Id = 6149;
        public override ushort TypeId => Id;

        public string value;

        public ServerSessionConstantString()
        {
        }
        public ServerSessionConstantString(string value)
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








