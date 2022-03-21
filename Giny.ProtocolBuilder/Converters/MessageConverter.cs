using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.AS3;
using Giny.AS3.Enums;
using Giny.AS3.Expressions;

namespace Giny.ProtocolBuilder.Converters
{
    public class MessageConverter : SerializableConverter
    {
        public override bool WriteDefaultFieldValues => false;

        public override string[] Imports => new string[]
        {
            "System.Collections.Generic",
            "Giny.Core.Network.Messages",
            "Giny.Protocol.Types",
            "Giny.Core.IO.Interfaces",
            "Giny.Protocol",
            "Giny.Protocol.Enums",
        };

        public override string BaseClassName => "NetworkMessage";

        public override string GetNamespace()
        {
            return "Giny.Protocol.Messages";
        }
        public MessageConverter(AS3File file) : base(file)
        {

        }

        public override void PostPrepare()
        {
            base.PostPrepare();

            if (GetClassName() == "RawDataMessage")
            {
                var method = GetMethodToWrite("Deserialize");

                method.Expressions.Clear();

                AS3Variable v1 = new AS3Variable("_contentLen", "int");
                MethodCallExpression e1 = new MethodCallExpression(File, "reader.ReadVarInt()", 0);
                VariableDeclarationExpression line1 = new VariableDeclarationExpression(v1, e1);


                MethodCallExpression e2 = new MethodCallExpression(File, "reader.ReadBytes(_contentLen)", 0);
                AssignationExpression line2 = new AssignationExpression("content", e2);

                method.Expressions.Add(line1);
                method.Expressions.Add(line2);
            }
        }

        public string GetMessageProtocolId()
        {
            int protocolId = (int)File.GetField("protocolId").GetValue<ConstantExpression>().Value;
            StringBuilder sb = new StringBuilder();
            string modifier = File.Extends != BaseClassName ? string.Empty : "new";
            Append(string.Format("public {0} const ushort Id = {1};", modifier, protocolId), sb);
            Append("public override ushort MessageId => Id;", sb);
            return sb.ToString();
        }


    }
}
