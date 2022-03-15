using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildFightJoinRequestMessage : NetworkMessage  
    { 
        public  const ushort Id = 9050;
        public override ushort MessageId => Id;

        public double taxCollectorId;

        public GuildFightJoinRequestMessage()
        {
        }
        public GuildFightJoinRequestMessage(double taxCollectorId)
        {
            this.taxCollectorId = taxCollectorId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (taxCollectorId < 0 || taxCollectorId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + taxCollectorId + ") on element taxCollectorId.");
            }

            writer.WriteDouble((double)taxCollectorId);
        }
        public override void Deserialize(IDataReader reader)
        {
            taxCollectorId = (double)reader.ReadDouble();
            if (taxCollectorId < 0 || taxCollectorId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + taxCollectorId + ") on element of GuildFightJoinRequestMessage.taxCollectorId.");
            }

        }


    }
}








