using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyMemberRemoveMessage : AbstractPartyEventMessage  
    { 
        public new const ushort Id = 2903;
        public override ushort MessageId => Id;

        public long leavingPlayerId;

        public PartyMemberRemoveMessage()
        {
        }
        public PartyMemberRemoveMessage(long leavingPlayerId)
        {
            this.leavingPlayerId = leavingPlayerId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (leavingPlayerId < 0 || leavingPlayerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + leavingPlayerId + ") on element leavingPlayerId.");
            }

            writer.WriteVarLong((long)leavingPlayerId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            leavingPlayerId = (long)reader.ReadVarUhLong();
            if (leavingPlayerId < 0 || leavingPlayerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + leavingPlayerId + ") on element of PartyMemberRemoveMessage.leavingPlayerId.");
            }

        }


    }
}








