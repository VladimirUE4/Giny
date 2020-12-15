using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightDispellSpellMessage : GameActionFightDispellMessage  
    { 
        public new const ushort Id = 6668;
        public override ushort MessageId => Id;

        public short spellId;

        public GameActionFightDispellSpellMessage()
        {
        }
        public GameActionFightDispellSpellMessage(short spellId)
        {
            this.spellId = spellId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (spellId < 0)
            {
                throw new Exception("Forbidden value (" + spellId + ") on element spellId.");
            }

            writer.WriteVarShort((short)spellId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spellId = (short)reader.ReadVarUhShort();
            if (spellId < 0)
            {
                throw new Exception("Forbidden value (" + spellId + ") on element of GameActionFightDispellSpellMessage.spellId.");
            }

        }


    }
}








