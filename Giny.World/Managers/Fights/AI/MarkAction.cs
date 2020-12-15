﻿using Giny.Core.Extensions;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public class MarkAction : AIAction
    {
        public MarkAction(AIFighter fighter) : base(fighter)
        {
        }

        public override double ComputePriority()
        {
            return 0.9d;
        }

        public override void Execute()
        {
            var target = Fighter.EnemyTeam.CloserFighter(Fighter);

            foreach (var spellRecord in GetSpells(SpellCategoryEnum.Mark).Shuffle())
            {
                var targetPoint = GetTargetPoint(spellRecord.Id, x => Fighter.Fight.IsCellFree(x.CellId));

                if (targetPoint != null)
                {
                    Fighter.CastSpell(spellRecord.Id, targetPoint.CellId);
                }
            }
        }
    }
}
