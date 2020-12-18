using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Effects.Targets
{
    public class StateCriterion : TargetCriterion
    {
        public StateCriterion(bool caster, int state, bool required) : base(caster)
        {
            State = state;
            Required = required;
        }

        public int State
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

        public override string ToString()
        {
            SpellStateRecord record = SpellStateRecord.GetSpellStateRecord(State);

            if (Required)
            {
                return "Has State (" + record.Name + ")";
            }
            else
            {
                return "Has not State (" + record.Name + ")";
            }

        }
    }
}
