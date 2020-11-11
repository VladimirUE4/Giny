using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class LifePointsRegenBeginMessage : NetworkMessage  
    { 
        public new const ushort Id = 1894;
        public override ushort MessageId => Id;

        public byte regenRate;

        public LifePointsRegenBeginMessage()
        {
        }
        public LifePointsRegenBeginMessage(byte regenRate)
        {
            this.regenRate = regenRate;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (regenRate < 0 || regenRate > 255)
            {
                throw new Exception("Forbidden value (" + regenRate + ") on element regenRate.");
            }

            writer.WriteByte((byte)regenRate);
        }
        public override void Deserialize(IDataReader reader)
        {
            regenRate = (byte)reader.ReadSByte();
            if (regenRate < 0 || regenRate > 255)
            {
                throw new Exception("Forbidden value (" + regenRate + ") on element of LifePointsRegenBeginMessage.regenRate.");
            }

        }


    }
}








