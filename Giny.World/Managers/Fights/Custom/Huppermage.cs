using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.World.Managers.Fights.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Custom
{
    public class Huppermage
    {
        [SpellCastHandler(SpellEnum.ElementalGuardian)]
        public class Conquest : DefaultSpellCastHandler
        {
            public Conquest(SpellCast cast) : base(cast)
            {

            }
            protected override IEnumerable<SpellEffectHandler> OrderHandlers()
            {
                return OrderByEffects(EffectsEnum.Effect_Kill, EffectsEnum.Effect_SummonSlave);
            }
        }
    }
}
