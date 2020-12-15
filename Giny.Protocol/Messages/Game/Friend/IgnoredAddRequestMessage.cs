using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class IgnoredAddRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 5339;
        public override ushort MessageId => Id;

        public string name;
        public bool session;

        public IgnoredAddRequestMessage()
        {
        }
        public IgnoredAddRequestMessage(string name,bool session)
        {
            this.name = name;
            this.session = session;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF((string)name);
            writer.WriteBoolean((bool)session);
        }
        public override void Deserialize(IDataReader reader)
        {
            name = (string)reader.ReadUTF();
            session = (bool)reader.ReadBoolean();
        }


    }
}








