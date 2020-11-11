using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightLifeAndShieldPointsLostMessage : GameActionFightLifePointsLostMessage  
    { 
        public new const ushort Id = 5065;
        public override ushort MessageId => Id;

        public short shieldLoss;

        public GameActionFightLifeAndShieldPointsLostMessage()
        {
        }
        public GameActionFightLifeAndShieldPointsLostMessage(short shieldLoss)
        {
            this.shieldLoss = shieldLoss;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (shieldLoss < 0)
            {
                throw new Exception("Forbidden value (" + shieldLoss + ") on element shieldLoss.");
            }

            writer.WriteVarShort((short)shieldLoss);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            shieldLoss = (short)reader.ReadVarUhShort();
            if (shieldLoss < 0)
            {
                throw new Exception("Forbidden value (" + shieldLoss + ") on element of GameActionFightLifeAndShieldPointsLostMessage.shieldLoss.");
            }

        }


    }
}








