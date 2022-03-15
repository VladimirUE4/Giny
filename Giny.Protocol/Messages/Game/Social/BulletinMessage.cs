using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class BulletinMessage : SocialNoticeMessage  
    { 
        public  const ushort Id = 9541;
        public override ushort MessageId => Id;

        public int lastNotifiedTimestamp;

        public BulletinMessage()
        {
        }
        public BulletinMessage(int lastNotifiedTimestamp)
        {
            this.lastNotifiedTimestamp = lastNotifiedTimestamp;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (lastNotifiedTimestamp < 0)
            {
                throw new Exception("Forbidden value (" + lastNotifiedTimestamp + ") on element lastNotifiedTimestamp.");
            }

            writer.WriteInt((int)lastNotifiedTimestamp);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            lastNotifiedTimestamp = (int)reader.ReadInt();
            if (lastNotifiedTimestamp < 0)
            {
                throw new Exception("Forbidden value (" + lastNotifiedTimestamp + ") on element of BulletinMessage.lastNotifiedTimestamp.");
            }

        }


    }
}








