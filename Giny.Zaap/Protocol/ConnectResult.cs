using Giny.Core.IO;
using Giny.Zaap.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Zaap.Protocol
{
    public class ConnectResult
    {
        public ConnectResult()
        {

        }

        public void Write(TProtocol protocol, BigEndianWriter writer)
        {
            protocol.WriteFieldBegin(new TField("success", TType.STRING, 0), writer);

            string toSend = "success";

            writer.WriteInt(toSend.Length);
            writer.WriteUTFBytes(toSend);

            protocol.WriteFieldStop(writer);
        }
    }
}
