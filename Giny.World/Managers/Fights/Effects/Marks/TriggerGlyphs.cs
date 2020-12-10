using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Marks
{
    [SpellEffectHandler(EffectsEnum.Effect_TriggerGlyphs)]
    public class TriggerGlyphs : SpellEffectHandler
    {
        public TriggerGlyphs(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            foreach (Glyph glyph in Source.GetMarks<Glyph>().Where(x => x.ContainsCell(TargetCell.Id)))
            {
                glyph.Trigger(Source, MarkTriggerType.Instant);
            }
        }
    }
}
