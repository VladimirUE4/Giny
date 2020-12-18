using Giny.Core.DesignPattern;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Effects.Targets
{
    public class MonsterCriterion : TargetCriterion
    {
        public MonsterCriterion(bool caster, int monsterId, bool required):base(caster)
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
        {
            return Required ? ((actor is IMonster) && (actor as IMonster).Record.Id == MonsterId) :
                (!(actor is IMonster) || (actor as IMonster).Record.Id != MonsterId);
        }

        public override string ToString()
        {
            MonsterRecord record = MonsterRecord.GetMonsterRecord((short)MonsterId);

            if (Required)
            {
                return "IsMonster (" + record.Name + ")";
            }
            else
            {
                return "IsNotMonster (" + record.Name + ")";
            }
        }
    }
}
