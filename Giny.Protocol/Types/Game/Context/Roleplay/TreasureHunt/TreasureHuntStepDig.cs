using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class TreasureHuntStepDig : TreasureHuntStep  
    { 
        public const ushort Id = 199;
        public override ushort TypeId => Id;


        public TreasureHuntStepDig()
        {
        }
        public override void Serialize(IDataWriter writer)
        {
        }
        public override void Deserialize(IDataReader reader)
        {
        }


    }
}








