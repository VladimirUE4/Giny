using Giny.Core.Extensions;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Maps;
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
            MapPoint targetPoint = Fighter.Cell.Point.GetNearPoints().FirstOrDefault(x => Fighter.Fight.IsCellFree(x.CellId));

            if (targetPoint != null)
            {
                foreach (var spellRecord in GetSpells(SpellCategoryEnum.Summon).Shuffle())
                {
                    Fighter.CastSpell(spellRecord.Id, targetPoint.CellId);
                }
            }
        }
    }
}
