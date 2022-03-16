using Giny.IO.D2I;
using Giny.IO.D2O;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.EnumsBuilder.Generation
{
    public abstract class AbstractEnum
    {
        public abstract string ClassName { get; }

        protected abstract string GenerateEnumContent(List<D2OReader> readers, D2IFile d2i);

        public string Generate(List<D2OReader> readers, D2IFile d2i)
        {
            string content = ApplyRules(GenerateEnumContent(readers, d2i));

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("namespace Giny.Protocol.Custom.Enums");
            sb.AppendLine("{");
            sb.AppendLine("    public enum " + ClassName);
            sb.AppendLine("    {");
            sb.AppendLine(content);
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string ApplyRules(string generated)
        {
            return generated;
        }
    }
}


