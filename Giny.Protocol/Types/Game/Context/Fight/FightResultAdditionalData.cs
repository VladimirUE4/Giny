using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightResultAdditionalData  
    { 
        public const ushort Id = 6509;
        public virtual ushort TypeId => Id;


        public FightResultAdditionalData()
        {
        }
        public virtual void Serialize(IDataWriter writer)
        {
        }
        public virtual void Deserialize(IDataReader reader)
        {
        }


    }
}








