using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyNameUpdateMessage : AbstractPartyMessage  
    { 
        public new const ushort Id = 1249;
        public override ushort MessageId => Id;

        public string partyName;

        public PartyNameUpdateMessage()
        {
        }
        public PartyNameUpdateMessage(string partyName)
        {
            this.partyName = partyName;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)partyName);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyName = (string)reader.ReadUTF();
        }


    }
}








