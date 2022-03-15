using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class BreachGameFightEndMessage : GameFightEndMessage  
    { 
        public  const ushort Id = 7323;
        public override ushort MessageId => Id;

        public int budget;

        public BreachGameFightEndMessage()
        {
        }
        public BreachGameFightEndMessage(int budget)
        {
            this.budget = budget;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt((int)budget);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            budget = (int)reader.ReadInt();
        }


    }
}








