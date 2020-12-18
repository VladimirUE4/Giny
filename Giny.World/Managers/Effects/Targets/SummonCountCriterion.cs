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
    /// Q
    /// </summary>
    public class SummonCountCriterion : TargetCriterion
    {
        public SummonCountCriterion(bool caster) : base(caster)
        {
        }

        public override bool IsTargetValid(Fighter actor, SpellEffectHandler handler)
        {
            return true;
        }
    }
}
