using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class LifePointsRegenEndMessage : UpdateLifePointsMessage  
    { 
        public new const ushort Id = 3437;
        public override ushort MessageId => Id;

        public int lifePointsGained;

        public LifePointsRegenEndMessage()
        {
        }
        public LifePointsRegenEndMessage(int lifePointsGained)
        {
            this.lifePointsGained = lifePointsGained;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (lifePointsGained < 0)
            {
                throw new Exception("Forbidden value (" + lifePointsGained + ") on element lifePointsGained.");
            }

            writer.WriteVarInt((int)lifePointsGained);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            lifePointsGained = (int)reader.ReadVarUhInt();
            if (lifePointsGained < 0)
            {
                throw new Exception("Forbidden value (" + lifePointsGained + ") on element of LifePointsRegenEndMessage.lifePointsGained.");
            }

        }


    }
}








