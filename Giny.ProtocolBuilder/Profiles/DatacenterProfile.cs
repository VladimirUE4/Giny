using Giny.ProtocolBuilder.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.ProtocolBuilder.Profiles
{
    public class DatacenterProfile : Profile
    {
        public override bool ParseMethods => false;

        public DatacenterProfile(string as3FilePath) : base(as3FilePath)
        {

        }
        public override string TemplateFileName => "DatacenterTemplate.tt";

        public override string RelativeOutputPath => @"Sources\com\ankamagames\dofus\datacenter\";

        public override string OutputDirectory => Path.Combine(Environment.CurrentDirectory, Constants.DATACENTER_OUTPUT_PATH);

        public override DofusConverter CreateDofusConverter()
        {
            return new DatacenterConverter(AS3File);
        }

        public override bool Skip()
        {
            return AS3File.ClassName == string.Empty || AS3File.ClassName == "AnimFunMonsterData" || AS3File.ClassName == "AnimFunNpcData"; // interfaces or manually generated
        }
    }
}
