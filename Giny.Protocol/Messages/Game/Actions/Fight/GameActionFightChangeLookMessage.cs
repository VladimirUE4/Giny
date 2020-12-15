using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightChangeLookMessage : AbstractGameActionMessage  
    { 
        public new const ushort Id = 562;
        public override ushort MessageId => Id;

        public double targetId;
        public EntityLook entityLook;

        public GameActionFightChangeLookMessage()
        {
        }
        public GameActionFightChangeLookMessage(double targetId,EntityLook entityLook)
        {
            this.targetId = targetId;
            this.entityLook = entityLook;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (targetId < -9.00719925474099E+15 || targetId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + targetId + ") on element targetId.");
            }

            writer.WriteDouble((double)targetId);
            entityLook.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = (double)reader.ReadDouble();
            if (targetId < -9.00719925474099E+15 || targetId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + targetId + ") on element of GameActionFightChangeLookMessage.targetId.");
            }

            entityLook = new EntityLook();
            entityLook.Deserialize(reader);
        }


    }
}








