using Giny.Core.Extensions;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Maps;
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


        protected override void Apply()
        {
            CastSpells();

        }
        private void CastSpells()
        {
            List<SpellCast> allCasts = new List<SpellCast>();

            foreach (var cell in EnumeratePossiblePosition())
            {
                allCasts.AddRange(GetSpellCasts(cell));
            }

            allCasts.AddRange(GetSpellCasts(Fighter.Cell));

            var bestCast = allCasts.MaxBy(x => GetEfficiency(x));

            if (bestCast != null)
            {
                var path = Fighter.FindPath(bestCast.CastCell);
                Fighter.Move(path);

                if (bestCast.CastCell != Fighter.Cell)
                {

                }
                Fighter.CastSpell(bestCast);
                CastSpells();
            }
        }

        private double GetEfficiency(SpellCast cast)
        {
            double value = cast.CastCell.Point.ManhattanDistanceTo(Fighter.Cell.Point);

            if (!cast.Target.IsSummoned())
            {
                value += 10;
            }

            return value;
        }
        private List<SpellCast> GetSpellCasts(CellRecord cell)
        {
            List<SpellCast> casts = new List<SpellCast>();

            foreach (var spellRecord in GetSpells().Where(x => x.Category == SpellCategoryEnum.Agressive || x.Category == SpellCategoryEnum.Debuff).Shuffle())
            {
                var spell = Fighter.GetSpell(spellRecord.Id);

                if (spell.Level.MaxRange == 0)
                {
                    SpellCast cast = new SpellCast(Fighter, spell, Fighter.Cell);
                    cast.CastCell = Fighter.Cell;
                    cast.Target = Fighter;

                    if (Fighter.CanCastSpell(cast) == SpellCastResult.OK)
                    {
                        casts.Add(cast);
                    }
                }
            }

            foreach (var target in Fighter.EnemyTeam.GetFighters())
            {
                foreach (var spellRecord in GetSpells().Where(x => x.Category == SpellCategoryEnum.Agressive || x.Category == SpellCategoryEnum.Debuff).Shuffle())
                {
                    var spell = Fighter.GetSpell(spellRecord.Id);

                    SpellCast cast = new SpellCast(Fighter, spell, target.Cell);
                    cast.CastCell = cell;
                    cast.Target = target;

                    if (Fighter.CanCastSpell(cast) == SpellCastResult.OK)
                    {
                        casts.Add(cast);
                    }
                }
            }

            return casts;
        }
    }
}