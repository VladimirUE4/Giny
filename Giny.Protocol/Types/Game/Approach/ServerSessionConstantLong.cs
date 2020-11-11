using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ServerSessionConstantLong : ServerSessionConstant  
    { 
        public const ushort Id = 2359;
        public override ushort TypeId => Id;

        public double value;

        public ServerSessionConstantLong()
        {
        }
        public ServerSessionConstantLong(double value)
        {
            this.value = value;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (value < -9.00719925474099E+15 || value > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + value + ") on element value.");
            }

            writer.WriteDouble((double)value);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = (double)reader.ReadDouble();
            if (value < -9.00719925474099E+15 || value > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + value + ") on element of ServerSessionConstantLong.value.");
            }

        }


    }
}








