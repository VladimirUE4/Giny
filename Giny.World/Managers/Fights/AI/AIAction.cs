using Giny.Core.Extensions;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Maps.Shapes.Sets;
using Giny.World.Records.Maps;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public abstract class AIAction
    {
        protected AIFighter Fighter
        {
            get;
            private set;
        }

        public AIAction(AIFighter fighter)
        {
            this.Fighter = fighter;
        }

        /*
         * Execute action
         */
        public abstract void Execute();

        /*
         *  returns a priority factor between 0 and 1
         */
        public abstract double ComputePriority();

        protected IEnumerable<SpellRecord> GetSpells(SpellCategoryEnum category)
        {
            return Fighter.GetSpells().Where(x => x.Category.HasFlag(category));
        }

        protected MapPoint GetTargetPoint(short spellId, Func<MapPoint, bool> predicate)
        {
            var spell = Fighter.GetSpell(spellId);
            Set zone = Fighter.GetSpellZone(spell.Level, Fighter.Cell.Point);
            IEnumerable<MapPoint> points = zone.EnumerateValidPoints();
            MapPoint targetPoint = points.Shuffle().FirstOrDefault(predicate);
            return targetPoint;
        }
    }
}
