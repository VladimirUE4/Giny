using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightInvisibilityMessage : AbstractGameActionMessage  
    { 
        public new const ushort Id = 4961;
        public override ushort MessageId => Id;

        public double targetId;
        public byte state;

        public GameActionFightInvisibilityMessage()
        {
        }
        public GameActionFightInvisibilityMessage(double targetId,byte state)
        {
            this.targetId = targetId;
            this.state = state;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (targetId < -9.00719925474099E+15 || targetId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + targetId + ") on element targetId.");
            }

            writer.WriteDouble((double)targetId);
            writer.WriteByte((byte)state);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = (double)reader.ReadDouble();
            if (targetId < -9.00719925474099E+15 || targetId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + targetId + ") on element of GameActionFightInvisibilityMessage.targetId.");
            }

            state = (byte)reader.ReadByte();
            if (state < 0)
            {
                throw new Exception("Forbidden value (" + state + ") on element of GameActionFightInvisibilityMessage.state.");
            }

        }


    }
}








