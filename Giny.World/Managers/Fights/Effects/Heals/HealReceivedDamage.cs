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
    [SpellEffectHandler(EffectsEnum.Effect_HealReceivedDamages)]
    public class HealReceivedDamage : SpellEffectHandler
    {
        public HealReceivedDamage(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override int Priority => 0;

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            short delta = (short)(this.CastHandler.Cast.GetTotalDamageDealt() * (Effect.Min / 100d));
         
            foreach (var target in targets)
            {
                target.Heal(new Healing(Source, target, delta));
            }
        }
    }

}
