﻿using Giny.World.Managers.Fights.Fighters;
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
            foreach (var spellRecord in Fighter.Record.SpellRecords.Where(x => x.Value.Category.HasFlag(SpellCategoryEnum.Buff)))
            {
                foreach (var ally in Fighter.Team.GetFighters<Fighter>())
                {
                    Fighter.CastSpell(spellRecord.Key, ally.Cell.Id);
                }
            }
        }
    }
}
