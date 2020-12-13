using Giny.Core.DesignPattern;
using Giny.Protocol.Enums;
using Giny.World.Managers.Actions;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Debuffs
{
    [WIP]
    [SpellEffectHandler(EffectsEnum.Effect_SubVitalityPercent_1048)]
    [SpellEffectHandler(EffectsEnum.Effect_SubVitalityPercent_2845)]
    public class SubVitalityPercent : SpellEffectHandler
    {
        private const ActionsEnum ActionId = ActionsEnum.ACTION_CHARACTER_DEBOOST_VITALITY;

        public SubVitalityPercent(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            foreach (var target in targets)
            {
                short delta = (short)(-target.Stats.MaxLifePoints * (double)Effect.Min / 100.0d);
                this.AddVitalityBuff(target, delta, FightDispellableEnum.DISPELLABLE, ActionId);
            }
        }
    }
}
