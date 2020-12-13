using Giny.Protocol.Custom.Enums;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Custom
{
    [SpellCastHandler(SpellEnum.Boozer12780)]
    public class Boozer : DefaultSpellCastHandler
    {
        public Boozer(SpellCast cast) : base(cast)
        {

        }
        protected override IEnumerable<SpellEffectHandler> OrderHandlers()
        {
            return Handlers;
        }


    }
}
