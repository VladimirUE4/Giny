using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PaddockBuyRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 5153;
        public override ushort MessageId => Id;

        public long proposedPrice;

        public PaddockBuyRequestMessage()
        {
        }
        public PaddockBuyRequestMessage(long proposedPrice)
        {
            this.proposedPrice = proposedPrice;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (proposedPrice < 0 || proposedPrice > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + proposedPrice + ") on element proposedPrice.");
            }

            writer.WriteVarLong((long)proposedPrice);
        }
        public override void Deserialize(IDataReader reader)
        {
            proposedPrice = (long)reader.ReadVarUhLong();
            if (proposedPrice < 0 || proposedPrice > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + proposedPrice + ") on element of PaddockBuyRequestMessage.proposedPrice.");
            }

        }


    }
}








