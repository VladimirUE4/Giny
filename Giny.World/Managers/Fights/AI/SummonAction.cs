using Giny.Core.Extensions;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Maps.Shapes.Sets;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public class SummonAction : AIAction
    {
        public SummonAction(AIFighter fighter) : base(fighter)
        {

        }

        public override double ComputePriority()
        {
            return -1;
        }

        public override void Execute()
        {

            foreach (var spellRecord in GetSpells(SpellCategoryEnum.Summon).Shuffle())
            {
                MapPoint targetPoint = GetTargetPoint(spellRecord.Id, x => Fighter.Fight.IsCellFree(x.CellId));
                Fighter.CastSpell(spellRecord.Id, targetPoint.CellId);
            }
        }
    }
}
