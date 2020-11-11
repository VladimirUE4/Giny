using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public class HealAction : AIAction
    {
        public HealAction(AIFighter fighter) : base(fighter)
        {
        }


        public override double ComputePriority()
        {
            return 0.2d;
        }

        public override void Execute()
        {
            foreach (var spellRecord in Fighter.Record.SpellRecords.Where(x => x.Value.Category.HasFlag(SpellCategoryEnum.Healing)))
            {
                foreach (var ally in Fighter.Team.GetFighters<Fighter>().OrderBy(x => x.Stats.LifePercentage))
                {
                    Fighter.CastSpell(spellRecord.Key, ally.Cell.Id);
                }
            }
        }
    }
}
