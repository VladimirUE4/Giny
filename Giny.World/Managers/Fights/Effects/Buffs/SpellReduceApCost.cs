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

namespace Giny.World.Managers.Fights.Effects.Buffs
{
    [SpellEffectHandler(EffectsEnum.Effect_SpellReduceApCost)]
    public class SpellReduceApCost : SpellEffectHandler
    {
        public SpellReduceApCost(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override int Priority => 0;

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            short spellId = (short)Effect.Min;
            short delta = (short)Effect.Value;

            foreach (var target in targets)
            {
                if (target.HasSpell(spellId))
                {
                    int id = target.BuffIdProvider.Pop();
                    SpellReduceApCostBuff buff = new SpellReduceApCostBuff(id, spellId, delta, CastHandler.Cast,
                        target, Effect, FightDispellableEnum.DISPELLABLE);
                    target.AddBuff(buff);
                }
            }
        }
    }
}
