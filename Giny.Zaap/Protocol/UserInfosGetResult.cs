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
    public class UserInfosGetResult : ZaapMessage
    {
        public UserInfosGetResult(TMessage tMessage) : base(tMessage)
        {
        }

        public override void Deserialize(TProtocol protocol, BigEndianReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(TProtocol protocol, BigEndianWriter writer)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();

            values.Add("login", MainWindow.Username);

            string toSend = JsonConvert.SerializeObject(values);

            protocol.WriteFieldBegin(new TField(toSend, TType.STRING, 0), writer);

            writer.WriteInt(toSend.Length);
            writer.WriteUTFBytes(toSend);

            protocol.WriteFieldStop(writer);
        }
    }
}
