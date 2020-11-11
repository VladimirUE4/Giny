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

        protected override int Priority => 0;

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            this.Ratio = Effect.Min / 100d;

            foreach (var target in targets)
            {
                this.AddTriggerBuff(target, FightDispellableEnum.DISPELLABLE_BY_DEATH, BuffTriggerType.BeforeDamaged,
                    BeforeDamaged);
            }
        }

        private bool BeforeDamaged(TriggerBuff buff, ITriggerToken token)
        {
            Damage damages = (Damage)token;
            damages.Computed = (short)(damages.Computed.Value * Ratio);
            Source.Fight.Warn(damages.Computed.ToString());
            return false;
        }
    }
}
