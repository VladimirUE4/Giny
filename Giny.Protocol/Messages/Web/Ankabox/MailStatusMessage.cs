using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MailStatusMessage : NetworkMessage  
    { 
        public new const ushort Id = 3369;
        public override ushort MessageId => Id;

        public short unread;
        public short total;

        public MailStatusMessage()
        {
        }
        public MailStatusMessage(short unread,short total)
        {
            this.unread = unread;
            this.total = total;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (unread < 0)
            {
                throw new Exception("Forbidden value (" + unread + ") on element unread.");
            }

            writer.WriteVarShort((short)unread);
            if (total < 0)
            {
                throw new Exception("Forbidden value (" + total + ") on element total.");
            }

            writer.WriteVarShort((short)total);
        }
        public override void Deserialize(IDataReader reader)
        {
            unread = (short)reader.ReadVarUhShort();
            if (unread < 0)
            {
                throw new Exception("Forbidden value (" + unread + ") on element of MailStatusMessage.unread.");
            }

            total = (short)reader.ReadVarUhShort();
            if (total < 0)
            {
                throw new Exception("Forbidden value (" + total + ") on element of MailStatusMessage.total.");
            }

        }


    }
}








