using Giny.AS3;
using Giny.AS3.Enums;
using Giny.AS3.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.ProtocolBuilder.Converters
{
    public class TypeConverter : SerializableConverter
    {
        public override bool WriteDefaultFieldValues => false;

        public override string[] Imports => new string[]
        {
            "System",
            "System.Collections.Generic",
            "Giny.Core.IO.Interfaces",
            "Giny.Protocol",
            "Giny.Protocol.Enums",
        };
        public override string GetNamespace()
        {
            return "Giny.Protocol.Types";
        }
        public TypeConverter(AS3File file) : base(file)
        {

        }
        public override string GetExtends()
        {
            return base.GetExtends();
        }
        public override void Initialize()
        {
            base.Initialize();

            if (GetExtends() == string.Empty)
            {
                SerializeMethod.SetModifiers(AS3ModifiersEnum.@virtual);
                DeserializeMethod.SetModifiers(AS3ModifiersEnum.@virtual);
            }
        }
        public string GetTypeProtocolId()
        {
            int protocolId = (int)File.GetField("protocolId").GetValue<ConstantExpression>().Value;

            StringBuilder sb = new StringBuilder();


            string modifier = this.GetExtends() == string.Empty ? "virtual" : "override";

            Append(string.Format("public const ushort Id = {0};", protocolId), sb);
            Append("public " + modifier + " ushort TypeId => Id;", sb);
            return sb.ToString();
        }
    }
}
