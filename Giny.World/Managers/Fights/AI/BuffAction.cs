using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public class BuffAction : AIAction
    {
        public BuffAction(AIFighter fighter) : base(fighter)
        {

        }

        public override double ComputePriority()
        {
            return 0d;
        }

        public override void Execute()
        {
            foreach (var spellRecord in Fighter.GetSpells().Where(x => x.Category.HasFlag(SpellCategoryEnum.Buff) && !x.Category.HasFlag(SpellCategoryEnum.Debuff) && !x.Category.HasFlag(SpellCategoryEnum.Damages)))
            {
                foreach (var ally in Fighter.Team.GetFighters<Fighter>().ToArray())
                {
                    Fighter.CastSpell(spellRecord.Id, ally.Cell.Id);
                }
            }
        }
    }
}
