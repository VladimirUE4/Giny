using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class BreachBudgetMessage : NetworkMessage  
    { 
        public  const ushort Id = 1903;
        public override ushort MessageId => Id;

        public int bugdet;

        public BreachBudgetMessage()
        {
        }
        public BreachBudgetMessage(int bugdet)
        {
            this.bugdet = bugdet;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (bugdet < 0)
            {
                throw new Exception("Forbidden value (" + bugdet + ") on element bugdet.");
            }

            writer.WriteVarInt((int)bugdet);
        }
        public override void Deserialize(IDataReader reader)
        {
            bugdet = (int)reader.ReadVarUhInt();
            if (bugdet < 0)
            {
                throw new Exception("Forbidden value (" + bugdet + ") on element of BreachBudgetMessage.bugdet.");
            }

        }


    }
}








