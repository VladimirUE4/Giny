using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class PlayerStatusExtended : PlayerStatus  
    { 
        public const ushort Id = 1176;
        public override ushort TypeId => Id;

        public string message;

        public PlayerStatusExtended()
        {
        }
        public PlayerStatusExtended(string message)
        {
            this.message = message;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)message);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            message = (string)reader.ReadUTF();
        }


    }
}








