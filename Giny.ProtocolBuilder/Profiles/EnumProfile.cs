using Giny.ProtocolBuilder.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.ProtocolBuilder.Profiles
{
    public class EnumProfile : Profile
    {
        public EnumProfile(string as3FilePath) : base(as3FilePath)
        {

        }

        public override string TemplateFileName => "EnumTemplate.tt";

        public override string RelativeOutputPath => String.Empty;

        public override string OutputDirectory => Path.Combine(Environment.CurrentDirectory, Constants.ENUMS_OUTPUT_PATH);

        public override DofusConverter CreateDofusConverter()
        {
            return new EnumConverter(AS3File);
        }

        public override bool Skip()
        {
            return false;
        }
    }
}
