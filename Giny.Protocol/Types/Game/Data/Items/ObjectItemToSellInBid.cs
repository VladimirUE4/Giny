using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ObjectItemToSellInBid : ObjectItemToSell  
    { 
        public const ushort Id = 8467;
        public override ushort TypeId => Id;

        public int unsoldDelay;

        public ObjectItemToSellInBid()
        {
        }
        public ObjectItemToSellInBid(int unsoldDelay)
        {
            this.unsoldDelay = unsoldDelay;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (unsoldDelay < 0)
            {
                throw new Exception("Forbidden value (" + unsoldDelay + ") on element unsoldDelay.");
            }

            writer.WriteInt((int)unsoldDelay);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            unsoldDelay = (int)reader.ReadInt();
            if (unsoldDelay < 0)
            {
                throw new Exception("Forbidden value (" + unsoldDelay + ") on element of ObjectItemToSellInBid.unsoldDelay.");
            }

        }


    }
}








