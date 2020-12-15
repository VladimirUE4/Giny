using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyMemberEjectedMessage : PartyMemberRemoveMessage  
    { 
        public new const ushort Id = 1694;
        public override ushort MessageId => Id;

        public long kickerId;

        public PartyMemberEjectedMessage()
        {
        }
        public PartyMemberEjectedMessage(long kickerId)
        {
            this.kickerId = kickerId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (kickerId < 0 || kickerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + kickerId + ") on element kickerId.");
            }

            writer.WriteVarLong((long)kickerId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            kickerId = (long)reader.ReadVarUhLong();
            if (kickerId < 0 || kickerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + kickerId + ") on element of PartyMemberEjectedMessage.kickerId.");
            }

        }


    }
}








