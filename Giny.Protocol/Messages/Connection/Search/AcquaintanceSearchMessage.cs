using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class AcquaintanceSearchMessage : NetworkMessage  
    { 
        public new const ushort Id = 8113;
        public override ushort MessageId => Id;

        public string nickname;

        public AcquaintanceSearchMessage()
        {
        }
        public AcquaintanceSearchMessage(string nickname)
        {
            this.nickname = nickname;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF((string)nickname);
        }
        public override void Deserialize(IDataReader reader)
        {
            nickname = (string)reader.ReadUTF();
        }


    }
}








