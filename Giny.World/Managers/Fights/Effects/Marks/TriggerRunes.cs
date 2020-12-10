using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Marks;
using Giny.World.Managers.Fights.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Marks
{
    [SpellEffectHandler(EffectsEnum.Effect_TriggerRunes)]
    public class TriggerRunes : SpellEffectHandler
    {
        public TriggerRunes(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            foreach (var trigger in Source.GetMarks<Rune>().ToArray())
            {
                using (Source.Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP))
                {
                    Fighter target = Source.Fight.GetFighter(trigger.CenterCell.Id);
                    trigger.Trigger(target, MarkTriggerType.None);
                }
            }

        }
    }
}
