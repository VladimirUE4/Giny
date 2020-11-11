using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class NewMailMessage : MailStatusMessage  
    { 
        public new const ushort Id = 7617;
        public override ushort MessageId => Id;

        public int[] sendersAccountId;

        public NewMailMessage()
        {
        }
        public NewMailMessage(int[] sendersAccountId)
        {
            this.sendersAccountId = sendersAccountId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)sendersAccountId.Length);
            for (uint _i1 = 0;_i1 < sendersAccountId.Length;_i1++)
            {
                if (sendersAccountId[_i1] < 0)
                {
                    throw new Exception("Forbidden value (" + sendersAccountId[_i1] + ") on element 1 (starting at 1) of sendersAccountId.");
                }

                writer.WriteInt((int)sendersAccountId[_i1]);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            uint _val1 = 0;
            base.Deserialize(reader);
            uint _sendersAccountIdLen = (uint)reader.ReadUShort();
            sendersAccountId = new int[_sendersAccountIdLen];
            for (uint _i1 = 0;_i1 < _sendersAccountIdLen;_i1++)
            {
                _val1 = (uint)reader.ReadInt();
                if (_val1 < 0)
                {
                    throw new Exception("Forbidden value (" + _val1 + ") on elements of sendersAccountId.");
                }

                sendersAccountId[_i1] = (int)_val1;
            }

        }


    }
}








