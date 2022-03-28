using Giny.Core.IO;
using Giny.Zaap.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Zaap.Protocol
{

    public class AuthGetGameTokenResult : ZaapMessage
    {
        public AuthGetGameTokenResult(TMessage tMessage) : base(tMessage)
        {
        }

        public override void Deserialize(TProtocol protocol, BigEndianReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(TProtocol protocol, BigEndianWriter writer)
        {
            string toSend = MainWindow.Password;

            protocol.WriteFieldBegin(new TField(toSend, TType.STRING, 0), writer);

            writer.WriteInt(toSend.Length);
            writer.WriteUTFBytes(toSend);

            protocol.WriteFieldStop(writer);
        }
    }
}
