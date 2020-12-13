using Giny.Core.Extensions;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public class MoveToTarget : AIAction
    {
        public MoveToTarget(AIFighter fighter) : base(fighter)
        {

        }

        public override double ComputePriority()
        {
            return 0.5d;
        }

        public override void Execute()
        {
            var target = Fighter.EnemyTeam.CloserFighter(Fighter);

            if (target.IsMeleeWith(Fighter))
            {
                return;
            }

            foreach (var spellRecord in GetSpells(SpellCategoryEnum.Teleport).Shuffle())
            {
                var targetPoint = target.Cell.Point.GetNearPoints().FirstOrDefault(x => Fighter.Fight.IsCellFree(x.CellId));

                if (targetPoint != null)
                {
                    Fighter.CastSpell(spellRecord.Id, targetPoint.CellId);
                }
            }

            var path = Fighter.FindPath(target);
            Fighter.Move(path);
        }
    }
}
