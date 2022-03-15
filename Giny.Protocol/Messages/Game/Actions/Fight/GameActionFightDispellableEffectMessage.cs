using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightDispellableEffectMessage : AbstractGameActionMessage  
    { 
        public  const ushort Id = 9416;
        public override ushort MessageId => Id;

        public AbstractFightDispellableEffect effect;

        public GameActionFightDispellableEffectMessage()
        {
        }
        public GameActionFightDispellableEffectMessage(AbstractFightDispellableEffect effect)
        {
            this.effect = effect;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)effect.TypeId);
            effect.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            uint _id1 = (uint)reader.ReadUShort();
            effect = ProtocolTypeManager.GetInstance<AbstractFightDispellableEffect>((short)_id1);
            effect.Deserialize(reader);
        }


    }
}








