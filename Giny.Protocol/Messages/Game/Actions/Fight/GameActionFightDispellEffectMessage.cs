using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightDispellEffectMessage : GameActionFightDispellMessage  
    { 
        public new const ushort Id = 827;
        public override ushort MessageId => Id;

        public int boostUID;

        public GameActionFightDispellEffectMessage()
        {
        }
        public GameActionFightDispellEffectMessage(int boostUID)
        {
            this.boostUID = boostUID;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (boostUID < 0)
            {
                throw new Exception("Forbidden value (" + boostUID + ") on element boostUID.");
            }

            writer.WriteInt((int)boostUID);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            boostUID = (int)reader.ReadInt();
            if (boostUID < 0)
            {
                throw new Exception("Forbidden value (" + boostUID + ") on element of GameActionFightDispellEffectMessage.boostUID.");
            }

        }


    }
}








