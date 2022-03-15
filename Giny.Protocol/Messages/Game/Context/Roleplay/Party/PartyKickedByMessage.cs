using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyKickedByMessage : AbstractPartyMessage  
    { 
        public  const ushort Id = 8439;
        public override ushort MessageId => Id;

        public long kickerId;

        public PartyKickedByMessage()
        {
        }
        public PartyKickedByMessage(long kickerId)
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
                throw new Exception("Forbidden value (" + kickerId + ") on element of PartyKickedByMessage.kickerId.");
            }

        }


    }
}








