using Giny.IO.D2I;
using Giny.IO.D2O;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.EnumsBuilder.Generation
{
    public class NpcActions : CustomEnum
    {
        public override string ClassName => "NpcActionsEnum";

        protected override string GenerateEnumContent(List<D2OReader> readers, D2IFile d2i)
        {
            var actions = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "NpcAction")).EnumerateObjects().Cast<Giny.IO.D2OClasses.NpcAction>();

            StringBuilder sb = new StringBuilder();

            foreach (var action in actions)
            {
                var text = d2i.GetText((int)action.nameId);

                sb.AppendLine(ApplyRules(text) + "=" + action.realId + ",");
            }

            return sb.ToString();
        }
    }
}
