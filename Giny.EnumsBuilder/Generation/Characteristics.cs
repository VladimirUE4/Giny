using Giny.IO.D2I;
using Giny.IO.D2O;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.EnumsBuilder.Generation
{
    public class Characteristics : CustomEnum
    {
        public override string ClassName => "CharacteristicEnum";

        protected override string GenerateEnumContent(List<D2OReader> readers, D2IFile d2i)
        {
            var characteristics = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "Characteristic")).EnumerateObjects().Cast<Giny.IO.D2OClasses.Characteristic>();

            StringBuilder sb = new StringBuilder();

            foreach (var characteristic in characteristics)
            {
                sb.AppendLine(ApplyRules(characteristic.keyword) + "=" + characteristic.id + ",");
            }

            return sb.ToString();
        }
    }
}
