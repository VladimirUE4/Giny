using Giny.Protocol.Custom.Enums;
using Giny.World.Managers.Fights.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Custom.Iop
{
    [SpellCastHandler(SpellEnum.Strengthstorm13121)]
    public class StrengthStorm : DefaultSpellCastHandler
    {
        public StrengthStorm(SpellCast cast) : base(cast)
        {

        }

        public override void BeforeExecute()
        {
            

        }
    }
}
