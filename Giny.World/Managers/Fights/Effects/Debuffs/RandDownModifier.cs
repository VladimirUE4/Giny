﻿using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Debuffs
{
    [SpellEffectHandler(EffectsEnum.Effect_RandDownModifier)]
    public class RandDownModifier : SpellEffectHandler
    {
        public RandDownModifier(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            foreach (var target in targets)
            {
                int buffId = target.BuffIdProvider.Pop();
                RandDownBuff buff = new RandDownBuff(buffId, CastHandler.Cast,
                    target, Effect, FightDispellableEnum.DISPELLABLE);
                target.AddBuff(buff);

            }
        }
    }
}
