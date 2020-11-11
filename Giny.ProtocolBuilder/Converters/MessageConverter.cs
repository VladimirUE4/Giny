using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.AS3;
using Giny.AS3.Expressions;

namespace Giny.ProtocolBuilder.Converters
{
    public class MessageConverter : SerializableConverter
    {
        public override bool WriteDefaultFieldValues => false;

        public override string[] Imports => new string[]
        {
            "System",
            "System.Collections.Generic",
            "Giny.Core.Network.Messages",
            "Giny.Protocol.Types",
            "Giny.Core.IO.Interfaces",
            "Giny.Protocol",
            "Giny.Protocol.Enums",
        };
        public override string GetNamespace()
        {
            return "Giny.Protocol.Messages";
        }
        public MessageConverter(AS3File file) : base(file)
        {

        }

        public string GetMessageProtocolId()
        {
            int protocolId = (int)File.GetField("protocolId").GetValue<ConstantExpression>().Value;
            StringBuilder sb = new StringBuilder();
            string modifier = File.Extends == string.Empty ? string.Empty : "new";
            Append(string.Format("public {0} const ushort Id = {1};", modifier, protocolId), sb);
            Append("public override ushort MessageId => Id;", sb);
            return sb.ToString();
        }


    }
}
