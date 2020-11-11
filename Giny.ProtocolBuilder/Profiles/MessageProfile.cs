using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.AS3.Expressions;
using Giny.ProtocolBuilder.Converters;

namespace Giny.ProtocolBuilder.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile(string as3FilePath) : base(as3FilePath)
        {

        }

        public override string TemplateFileName => "MessageTemplate.tt";

        public override string RelativeOutputPath => @"Sources\com\ankamagames\dofus\network\messages\";

        public override string OutputDirectory => Path.Combine(Environment.CurrentDirectory, Constants.MESSAGES_OUTPUT_PATH);

        public override DofusConverter CreateDofusConverter()
        {
            return new MessageConverter(AS3File);
        }

        public override bool Skip()
        {
            int messageId = (int)AS3File.GetField("protocolId").GetValue<ConstantIntExpression>().Value;

            switch (messageId)
            {
                case 2:
                    return true;
            }

            return false;
        }
    }
}
