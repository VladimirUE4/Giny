using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class TreasureHuntStepFight : TreasureHuntStep  
    { 
        public const ushort Id = 7770;
        public override ushort TypeId => Id;


        public TreasureHuntStepFight()
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








