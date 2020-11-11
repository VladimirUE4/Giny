using Giny.IO.D2OClasses;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Maps.Shapes;
using Giny.World.Records.Maps;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Marks
{
    public class Trap : Mark
    {
        public override bool StopMovement => true;

        public override GameActionMarkTypeEnum Type => GameActionMarkTypeEnum.TRAP;

        public Trap(int id, EffectDice effect, MarkTriggerType triggers, Zone zone, Color color, Fighter source, CellRecord centerCell, SpellRecord spellRecord, SpellLevelRecord spellLevel) :
            base(id, effect, zone, triggers, color, source, centerCell, spellRecord, spellLevel)
        {
         
        }

        public override bool IsVisibleFor(CharacterFighter fighter)
        {
            return Source.IsFriendlyWith(Source);
        }

        public override void Trigger(Fighter target, MarkTriggerType triggerType)
        {
            Source.Fight.RemoveMark(this);
            SpellCast cast = new SpellCast(Source, TriggerSpell, CenterCell);
            cast.CastCell = CenterCell;
            cast.Force = true;
            Source.CastSpell(cast);
        }
    }
}
