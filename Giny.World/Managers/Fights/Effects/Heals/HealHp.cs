using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Cast.Units;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Heals
{
    [SpellEffectHandler(EffectsEnum.Effect_HealHP_108)]
    public class HealHp : SpellEffectHandler
    {
        public HealHp(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            short jet = Effect.GetDelta();

            short delta = (short)(jet * (double)(100d + Source.Stats.Intelligence.TotalInContext()) / 100d + Source.Stats.HealBonus.TotalInContext());

            foreach (var target in targets)
            {
                target.Heal(new Healing(Source, target, delta));
            }
        }
    }
}
