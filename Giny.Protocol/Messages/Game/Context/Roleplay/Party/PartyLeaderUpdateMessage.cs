using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyLeaderUpdateMessage : AbstractPartyEventMessage  
    { 
        public new const ushort Id = 2432;
        public override ushort MessageId => Id;

        public long partyLeaderId;

        public PartyLeaderUpdateMessage()
        {
        }
        public PartyLeaderUpdateMessage(long partyLeaderId)
        {
            this.partyLeaderId = partyLeaderId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (partyLeaderId < 0 || partyLeaderId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + partyLeaderId + ") on element partyLeaderId.");
            }

            writer.WriteVarLong((long)partyLeaderId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyLeaderId = (long)reader.ReadVarUhLong();
            if (partyLeaderId < 0 || partyLeaderId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + partyLeaderId + ") on element of PartyLeaderUpdateMessage.partyLeaderId.");
            }

        }


    }
}








