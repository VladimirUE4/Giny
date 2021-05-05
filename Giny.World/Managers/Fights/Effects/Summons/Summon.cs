﻿using Giny.Core.DesignPattern;
using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;

using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Summons
{
    [SpellEffectHandler(EffectsEnum.Effect_Summon)]
    public class Summon : SpellEffectHandler
    {
        public Summon(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            if (Source.Fight.IsCellFree(TargetCell) && Source.CanSummon())
            {
                var fighter = CreateSummon((short)Effect.Min, (byte)Effect.Max);
                Source.Fight.AddSummon(Source, fighter);
            }

        }
    }
}
