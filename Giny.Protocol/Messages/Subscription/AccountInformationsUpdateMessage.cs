using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class AccountInformationsUpdateMessage : NetworkMessage  
    { 
        public new const ushort Id = 4873;
        public override ushort MessageId => Id;

        public double subscriptionEndDate;

        public AccountInformationsUpdateMessage()
        {
        }
        public AccountInformationsUpdateMessage(double subscriptionEndDate)
        {
            this.subscriptionEndDate = subscriptionEndDate;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (subscriptionEndDate < 0 || subscriptionEndDate > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + subscriptionEndDate + ") on element subscriptionEndDate.");
            }

            writer.WriteDouble((double)subscriptionEndDate);
        }
        public override void Deserialize(IDataReader reader)
        {
            subscriptionEndDate = (double)reader.ReadDouble();
            if (subscriptionEndDate < 0 || subscriptionEndDate > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + subscriptionEndDate + ") on element of AccountInformationsUpdateMessage.subscriptionEndDate.");
            }

        }


    }
}








