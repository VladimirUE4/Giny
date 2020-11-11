using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class KamasUpdateMessage : NetworkMessage  
    { 
        public new const ushort Id = 2022;
        public override ushort MessageId => Id;

        public long kamasTotal;

        public KamasUpdateMessage()
        {
        }
        public KamasUpdateMessage(long kamasTotal)
        {
            this.kamasTotal = kamasTotal;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (kamasTotal < 0 || kamasTotal > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + kamasTotal + ") on element kamasTotal.");
            }

            writer.WriteVarLong((long)kamasTotal);
        }
        public override void Deserialize(IDataReader reader)
        {
            kamasTotal = (long)reader.ReadVarUhLong();
            if (kamasTotal < 0 || kamasTotal > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + kamasTotal + ") on element of KamasUpdateMessage.kamasTotal.");
            }

        }


    }
}








