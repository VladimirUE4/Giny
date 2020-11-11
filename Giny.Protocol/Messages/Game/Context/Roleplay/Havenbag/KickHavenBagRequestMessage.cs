using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class KickHavenBagRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 711;
        public override ushort MessageId => Id;

        public long guestId;

        public KickHavenBagRequestMessage()
        {
        }
        public KickHavenBagRequestMessage(long guestId)
        {
            this.guestId = guestId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (guestId < 0 || guestId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + guestId + ") on element guestId.");
            }

            writer.WriteVarLong((long)guestId);
        }
        public override void Deserialize(IDataReader reader)
        {
            guestId = (long)reader.ReadVarUhLong();
            if (guestId < 0 || guestId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + guestId + ") on element of KickHavenBagRequestMessage.guestId.");
            }

        }


    }
}








