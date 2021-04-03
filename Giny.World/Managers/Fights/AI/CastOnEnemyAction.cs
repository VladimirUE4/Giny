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
    public class CastOnEnemyAction : AIAction
    {
        public CastOnEnemyAction(AIFighter fighter) : base(fighter)
        {
        }


        public override double ComputePriority()
        {
            return 1d;
        }

        public override void Execute()
        {
            var target = Fighter.EnemyTeam.CloserFighter(Fighter);

            if (target == null)
            {
                return;
            }
            foreach (var spellRecord in GetSpells(SpellCategoryEnum.Damages).Shuffle())
            {
                if (spellRecord.Levels.All(x => x.MaxRange == 0))
                {
                    Fighter.CastSpell(spellRecord.Id, Fighter.Cell.Id);
                }
                else
                {
                    Fighter.CastSpell(spellRecord.Id, target.Cell.Id);
                }
            }
        }
    }
}
