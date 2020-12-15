using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class PlayerStatus  
    { 
        public const ushort Id = 6393;
        public virtual ushort TypeId => Id;

        public byte statusId;

        public PlayerStatus()
        {
        }
        public PlayerStatus(byte statusId)
        {
            this.statusId = statusId;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteByte((byte)statusId);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            statusId = (byte)reader.ReadByte();
            if (statusId < 0)
            {
                throw new Exception("Forbidden value (" + statusId + ") on element of PlayerStatus.statusId.");
            }

        }


    }
}








