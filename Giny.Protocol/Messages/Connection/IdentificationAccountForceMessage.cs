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
        public  const ushort Id = 2449;
        public override ushort MessageId => Id;

        public string forcedAccountLogin;

        public IdentificationAccountForceMessage()
        {
        }
        public IdentificationAccountForceMessage(string forcedAccountLogin,Version version,string lang,byte[] credentials,short serverId,bool autoconnect,bool useCertificate,bool useLoginToken,long sessionOptionalSalt,short[] failedAttempts)
        {
            this.forcedAccountLogin = forcedAccountLogin;
            this.version = version;
            this.lang = lang;
            this.credentials = credentials;
            this.serverId = serverId;
            this.autoconnect = autoconnect;
            this.useCertificate = useCertificate;
            this.useLoginToken = useLoginToken;
            this.sessionOptionalSalt = sessionOptionalSalt;
            this.failedAttempts = failedAttempts;
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








