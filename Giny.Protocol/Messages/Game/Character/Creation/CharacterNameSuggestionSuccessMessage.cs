using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class CharacterNameSuggestionSuccessMessage : NetworkMessage  
    { 
        public new const ushort Id = 421;
        public override ushort MessageId => Id;

        public string suggestion;

        public CharacterNameSuggestionSuccessMessage()
        {
        }
        public CharacterNameSuggestionSuccessMessage(string suggestion)
        {
            this.suggestion = suggestion;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF((string)suggestion);
        }
        public override void Deserialize(IDataReader reader)
        {
            suggestion = (string)reader.ReadUTF();
        }


    }
}








