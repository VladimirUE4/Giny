using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class BreachRewardBoughtMessage : NetworkMessage  
    { 
        public new const ushort Id = 3338;
        public override ushort MessageId => Id;

        public int id;
        public bool bought;

        public BreachRewardBoughtMessage()
        {
        }
        public BreachRewardBoughtMessage(int id,bool bought)
        {
            this.id = id;
            this.bought = bought;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (id < 0)
            {
                throw new Exception("Forbidden value (" + id + ") on element id.");
            }

            writer.WriteVarInt((int)id);
            writer.WriteBoolean((bool)bought);
        }
        public override void Deserialize(IDataReader reader)
        {
            id = (int)reader.ReadVarUhInt();
            if (id < 0)
            {
                throw new Exception("Forbidden value (" + id + ") on element of BreachRewardBoughtMessage.id.");
            }

            bought = (bool)reader.ReadBoolean();
        }


    }
}








