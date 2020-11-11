using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class BasicWhoIsNoMatchMessage : NetworkMessage  
    { 
        public new const ushort Id = 5493;
        public override ushort MessageId => Id;

        public string search;

        public BasicWhoIsNoMatchMessage()
        {
        }
        public BasicWhoIsNoMatchMessage(string search)
        {
            this.search = search;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF((string)search);
        }
        public override void Deserialize(IDataReader reader)
        {
            search = (string)reader.ReadUTF();
        }


    }
}








