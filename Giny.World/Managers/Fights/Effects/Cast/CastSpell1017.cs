using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Cast
{
    [SpellEffectHandler(EffectsEnum.Effect_CastSpell_1017)]
    public class CastSpell1017 : SpellEffectHandler
    {
        protected override int Priority => 0;

        public CastSpell1017(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            SpellRecord spellRecord = SpellRecord.GetSpellRecord((short)Effect.Min);
            SpellLevelRecord level = spellRecord.GetLevel((byte)Effect.Max);
            Spell spell = new Spell(spellRecord, level);



            ITriggerToken token = this.GetTriggerToken<ITriggerToken>();

            if (token != null) // coup pour coup.
            {
                SpellCast cast = new SpellCast(Source, spell, token.GetSource().Cell, CastHandler.Cast);
                cast.Token = this.GetTriggerToken<ITriggerToken>();
                cast.Force = true;
                Source.CastSpell(cast);
            }
            else
            {
              

                foreach (var target in targets)
                {
                    SpellCast cast = new SpellCast(target, spell, Source.Cell, CastHandler.Cast);
                    cast.Token = this.GetTriggerToken<ITriggerToken>();
                    cast.Force = true;
                    target.CastSpell(cast);
                }
            }
        }
    }
}
