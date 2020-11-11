using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ContactLookRequestByNameMessage : ContactLookRequestMessage  
    { 
        public new const ushort Id = 2201;
        public override ushort MessageId => Id;

        public string playerName;

        public ContactLookRequestByNameMessage()
        {
        }
        public ContactLookRequestByNameMessage(string playerName)
        {
            this.playerName = playerName;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)playerName);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerName = (string)reader.ReadUTF();
        }


    }
}








