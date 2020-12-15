using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class IdentificationAccountForceMessage : IdentificationMessage  
    { 
        public new const ushort Id = 6812;
        public override ushort MessageId => Id;

        public string forcedAccountLogin;

        public IdentificationAccountForceMessage()
        {
        }
        public IdentificationAccountForceMessage(string forcedAccountLogin)
        {
            this.forcedAccountLogin = forcedAccountLogin;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)forcedAccountLogin);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            forcedAccountLogin = (string)reader.ReadUTF();
        }


    }
}








