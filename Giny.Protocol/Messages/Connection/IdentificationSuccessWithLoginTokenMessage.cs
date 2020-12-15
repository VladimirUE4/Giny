using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class IdentificationSuccessWithLoginTokenMessage : IdentificationSuccessMessage  
    { 
        public new const ushort Id = 5639;
        public override ushort MessageId => Id;

        public string loginToken;

        public IdentificationSuccessWithLoginTokenMessage()
        {
        }
        public IdentificationSuccessWithLoginTokenMessage(string loginToken)
        {
            this.loginToken = loginToken;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)loginToken);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            loginToken = (string)reader.ReadUTF();
        }


    }
}








