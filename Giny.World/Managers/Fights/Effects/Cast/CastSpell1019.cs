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
    [SpellEffectHandler(EffectsEnum.Effect_CastSpell_1019)]
    public class CastSpell1019 : SpellEffectHandler
    {
        public CastSpell1019(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            Spell spell = CreateCastedSpell();

            ITriggerToken token = this.GetTriggerToken<ITriggerToken>();


            if (token != null) // Dérobade
            {
                var source = token.GetSource();
                SpellCast cast = new SpellCast(Source, spell, source.Cell, CastHandler.Cast);
                cast.Token = this.GetTriggerToken<ITriggerToken>();
                cast.Force = true;
                cast.Silent = true;
                Source.CastSpell(cast);
            }
            else
            {
                foreach (var target in targets) // Ratrapry (Prisma) verify source.
                {
                    SpellCast cast = new SpellCast(Source, spell, target.Cell, CastHandler.Cast);
                    cast.Token = this.GetTriggerToken<ITriggerToken>();
                    cast.Force = true;
                    cast.Silent = true;
                    Source.CastSpell(cast);
                }
            }


        }
    }
}
