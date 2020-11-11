using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class UpdateMountBooleanCharacteristic : UpdateMountCharacteristic  
    { 
        public const ushort Id = 9905;
        public override ushort TypeId => Id;

        public bool value;

        public UpdateMountBooleanCharacteristic()
        {
        }
        public UpdateMountBooleanCharacteristic(bool value)
        {
            this.value = value;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean((bool)value);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = (bool)reader.ReadBoolean();
        }


    }
}








