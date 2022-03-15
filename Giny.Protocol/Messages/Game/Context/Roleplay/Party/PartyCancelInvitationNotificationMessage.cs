using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyCancelInvitationNotificationMessage : AbstractPartyEventMessage  
    { 
        public  const ushort Id = 1489;
        public override ushort MessageId => Id;

        public long cancelerId;
        public long guestId;

        public PartyCancelInvitationNotificationMessage()
        {
        }
        public PartyCancelInvitationNotificationMessage(long cancelerId,long guestId)
        {
            this.cancelerId = cancelerId;
            this.guestId = guestId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (cancelerId < 0 || cancelerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + cancelerId + ") on element cancelerId.");
            }

            writer.WriteVarLong((long)cancelerId);
            if (guestId < 0 || guestId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + guestId + ") on element guestId.");
            }

            writer.WriteVarLong((long)guestId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cancelerId = (long)reader.ReadVarUhLong();
            if (cancelerId < 0 || cancelerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + cancelerId + ") on element of PartyCancelInvitationNotificationMessage.cancelerId.");
            }

            guestId = (long)reader.ReadVarUhLong();
            if (guestId < 0 || guestId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + guestId + ") on element of PartyCancelInvitationNotificationMessage.guestId.");
            }

        }


    }
}








