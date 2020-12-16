using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Effects.Targets
{
    /// <summary>
    /// wasTeleportedInInvalidCellThisTurn
    /// </summary>
    public class UnknownCriterionW : TargetCriterion
    {
        public override bool IsTargetValid(Fighter actor, SpellEffectHandler handler)
        {
            return false;
        }
    }
}
