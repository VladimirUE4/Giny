using Giny.Core.DesignPattern;
using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Cast.Units;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Other
{
    [SpellEffectHandler(EffectsEnum.Effect_DamageMultiplier)]
    public class DamageMultiplier : SpellEffectHandler
    {
        private double Ratio
        {
            get;
            set;
        }
        public DamageMultiplier(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {

        }

        [WIP("Bond, le trigger s'applique après damages infligé, correct ? ")]
        protected override void Apply(IEnumerable<Fighter> targets)
        {
            var ratio = Effect.Min / 100d;
            Damage damages = GetTriggerToken<Damage>();
            damages.Computed =  (short)(damages.Computed.Value * (ratio - 1));
            damages.Target.InflictDamage(damages);
        }
    }
}
    