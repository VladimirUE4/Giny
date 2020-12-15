using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightModifyEffectsDurationMessage : AbstractGameActionMessage  
    { 
        public new const ushort Id = 353;
        public override ushort MessageId => Id;

        public double targetId;
        public short delta;

        public GameActionFightModifyEffectsDurationMessage()
        {
        }
        public GameActionFightModifyEffectsDurationMessage(double targetId,short delta)
        {
            this.targetId = targetId;
            this.delta = delta;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (targetId < -9.00719925474099E+15 || targetId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + targetId + ") on element targetId.");
            }

            writer.WriteDouble((double)targetId);
            writer.WriteShort((short)delta);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = (double)reader.ReadDouble();
            if (targetId < -9.00719925474099E+15 || targetId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + targetId + ") on element of GameActionFightModifyEffectsDurationMessage.targetId.");
            }

            delta = (short)reader.ReadShort();
        }


    }
}








