using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Effects.Targets
{
    public class StateCriterion : TargetCriterion
    {
        public StateCriterion(int state, bool caster, bool required)
        {
            State = state;
            Caster = caster;
            Required = required;
        }

        public int State
        {
            get;
            set;
        }

        public bool Caster
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsDisjonction => false;

        public override bool IsTargetValid(Fighter actor, SpellEffectHandler handler)
        {
            if (Caster)
                return Required ? handler.Source.HasState(State) : !handler.Source.HasState(State);

            return Required ? actor.HasState(State) : !actor.HasState(State);
        }
    }
}
