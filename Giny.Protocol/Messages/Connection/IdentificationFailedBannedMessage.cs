using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class IdentificationFailedBannedMessage : IdentificationFailedMessage  
    { 
        public new const ushort Id = 5973;
        public override ushort MessageId => Id;

        public double banEndDate;

        public IdentificationFailedBannedMessage()
        {
        }
        public IdentificationFailedBannedMessage(double banEndDate)
        {
            this.banEndDate = banEndDate;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (banEndDate < 0 || banEndDate > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + banEndDate + ") on element banEndDate.");
            }

            writer.WriteDouble((double)banEndDate);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            banEndDate = (double)reader.ReadDouble();
            if (banEndDate < 0 || banEndDate > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + banEndDate + ") on element of IdentificationFailedBannedMessage.banEndDate.");
            }

        }


    }
}








