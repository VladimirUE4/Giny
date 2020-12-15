using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyInvitationDungeonRequestMessage : PartyInvitationRequestMessage  
    { 
        public new const ushort Id = 6000;
        public override ushort MessageId => Id;

        public short dungeonId;

        public PartyInvitationDungeonRequestMessage()
        {
        }
        public PartyInvitationDungeonRequestMessage(short dungeonId)
        {
            this.dungeonId = dungeonId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (dungeonId < 0)
            {
                throw new Exception("Forbidden value (" + dungeonId + ") on element dungeonId.");
            }

            writer.WriteVarShort((short)dungeonId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            dungeonId = (short)reader.ReadVarUhShort();
            if (dungeonId < 0)
            {
                throw new Exception("Forbidden value (" + dungeonId + ") on element of PartyInvitationDungeonRequestMessage.dungeonId.");
            }

        }


    }
}








