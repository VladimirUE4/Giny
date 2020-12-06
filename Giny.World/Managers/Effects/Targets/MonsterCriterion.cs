using Giny.Core.DesignPattern;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Effects.Targets
{
    public class MonsterCriterion : TargetCriterion
    {
        public MonsterCriterion(int monsterId, bool required)
        {
            MonsterId = monsterId;
            Required = required;
        }

        public int MonsterId
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsDisjonction => Required;

        public override bool IsTargetValid(Fighter actor, SpellEffectHandler handler)
            => Required ? ((actor is IMonster) && (actor as IMonster).Record.Id == MonsterId) :
                (!(actor is IMonster) || (actor as IMonster).Record.Id != MonsterId);

        public override string ToString()
        {
            return "Monster (" + MonsterId + ")";
        }
    }
}
