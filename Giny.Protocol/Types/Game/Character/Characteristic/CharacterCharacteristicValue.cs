using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterCharacteristicValue : CharacterCharacteristic  
    { 
        public const ushort Id = 1995;
        public override ushort TypeId => Id;

        public int total;

        public CharacterCharacteristicValue()
        {
        }
        public CharacterCharacteristicValue(int total)
        {
            this.total = total;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt((int)total);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            total = (int)reader.ReadInt();
        }


    }
}








