using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Buffs
{
    [SpellEffectHandler(EffectsEnum.Effect_SpellBoost)]
    public class SpellBoost : SpellEffectHandler
    {
        public SpellBoost(EffectDice effect, SpellCastHandler castHandler) :
            base(effect, castHandler)
        {
            
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            foreach (var target in targets)
            {
                if (target.HasSpell((short)Effect.Min))
                {
                    SpellBoostBuff buff = new SpellBoostBuff(target.BuffIdProvider.Pop(), (short)Effect.Min, (short)Effect.Value, CastHandler.Cast, target, Effect, FightDispellableEnum.DISPELLABLE);
                    target.AddBuff(buff);
                }
            }
        }
         
    }
}
