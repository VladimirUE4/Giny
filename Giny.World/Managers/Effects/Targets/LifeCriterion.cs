using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Effects.Targets
{
    public class LifeCriterion : TargetCriterion
    {
        public LifeCriterion(int lifePercent, bool mustBeGreater)
        {
            LifePercent = lifePercent;
            MustBeGreater = mustBeGreater;
        }

        public int LifePercent
        {
            get;
            set;
        }

        public bool MustBeGreater
        {
            get;
            set;
        }

        public override bool IsTargetValid(Fighter actor, SpellEffectHandler handler)
        {
            return MustBeGreater ? actor.Stats.LifePoints / (double)actor.Stats.MaxLifePoints > LifePercent :
                actor.Stats.LifePoints / (double)actor.Stats.MaxLifePoints <= LifePercent;
        }
    }
}
