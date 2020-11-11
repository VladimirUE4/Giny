using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildSpellUpgradeRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 8084;
        public override ushort MessageId => Id;

        public int spellId;

        public GuildSpellUpgradeRequestMessage()
        {
        }
        public GuildSpellUpgradeRequestMessage(int spellId)
        {
            this.spellId = spellId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (spellId < 0)
            {
                throw new Exception("Forbidden value (" + spellId + ") on element spellId.");
            }

            writer.WriteInt((int)spellId);
        }
        public override void Deserialize(IDataReader reader)
        {
            spellId = (int)reader.ReadInt();
            if (spellId < 0)
            {
                throw new Exception("Forbidden value (" + spellId + ") on element of GuildSpellUpgradeRequestMessage.spellId.");
            }

        }


    }
}








