using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightActivateGlyphTrapMessage : AbstractGameActionMessage  
    { 
        public  const ushort Id = 9234;
        public override ushort MessageId => Id;

        public short markId;
        public bool active;

        public GameActionFightActivateGlyphTrapMessage()
        {
        }
        public GameActionFightActivateGlyphTrapMessage(short markId,bool active)
        {
            this.markId = markId;
            this.active = active;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)markId);
            writer.WriteBoolean((bool)active);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            markId = (short)reader.ReadShort();
            active = (bool)reader.ReadBoolean();
        }


    }
}








