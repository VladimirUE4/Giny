using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class AllianceBulletinMessage : BulletinMessage  
    { 
        public  const ushort Id = 661;
        public override ushort MessageId => Id;


        public AllianceBulletinMessage()
        {
        }
        public AllianceBulletinMessage(string content,int timestamp,long memberId,string memberName,int lastNotifiedTimestamp)
        {
            this.content = content;
            this.timestamp = timestamp;
            this.memberId = memberId;
            this.memberName = memberName;
            this.lastNotifiedTimestamp = lastNotifiedTimestamp;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }


    }
}








