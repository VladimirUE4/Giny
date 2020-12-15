using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ServerSessionConstantInteger : ServerSessionConstant  
    { 
        public const ushort Id = 8729;
        public override ushort TypeId => Id;

        public int value;

        public ServerSessionConstantInteger()
        {
        }
        public ServerSessionConstantInteger(int value)
        {
            this.value = value;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt((int)value);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = (int)reader.ReadInt();
        }


    }
}








