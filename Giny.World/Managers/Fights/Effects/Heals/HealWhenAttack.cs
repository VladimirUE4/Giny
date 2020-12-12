using Giny.Core.DesignPattern;
using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Cast.Units;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Heals
{
    [SpellEffectHandler(EffectsEnum.Effect_HealWhenAttack)]
    public class HealWhenAttack : SpellEffectHandler
    {
        public HealWhenAttack(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            foreach (var target in targets)
            {
                AddTriggerBuff(target, FightDispellableEnum.DISPELLABLE,
                    Trigger.Singleton(TriggerType.AfterDamaged), Apply);
            }
        }

        private bool Apply(TriggerBuff buff, ITriggerToken token)
        {
            Damage damage = (Damage)token;
            short delta = (short)(damage.Computed * (Effect.Value / 100d));
            buff.Target.Heal(new Healing(Source, buff.Target, delta));
            return false;
        }
    }
}
