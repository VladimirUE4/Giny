using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ExtendedLockedBreachBranch : ExtendedBreachBranch  
    { 
        public const ushort Id = 1746;
        public override ushort TypeId => Id;

        public int unlockPrice;

        public ExtendedLockedBreachBranch()
        {
        }
        public ExtendedLockedBreachBranch(int unlockPrice)
        {
            this.unlockPrice = unlockPrice;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (unlockPrice < 0)
            {
                throw new Exception("Forbidden value (" + unlockPrice + ") on element unlockPrice.");
            }

            writer.WriteVarInt((int)unlockPrice);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            unlockPrice = (int)reader.ReadVarUhInt();
            if (unlockPrice < 0)
            {
                throw new Exception("Forbidden value (" + unlockPrice + ") on element of ExtendedLockedBreachBranch.unlockPrice.");
            }

        }


    }
}








