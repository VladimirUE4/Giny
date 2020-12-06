﻿using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Marks;
using Giny.World.Managers.Maps.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Marks
{
    [SpellEffectHandler(EffectsEnum.Effect_Rune)]
    public class SpawnRune : SpellEffectHandler
    {
        public SpawnRune(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override int Priority => 0;

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            if (!Source.Fight.MarkExist<Rune>(x => x.CenterCell == TargetCell))
            {
                Zone zone = Effect.GetZone();
                Color color = Color.FromArgb(Effect.Value);

                Rune rune = new Rune(Source.Fight.PopNextMarkId(), Effect,
                    zone, MarkTriggerType.None, color,
                    Source, TargetCell, CastHandler.Cast.Spell.Record,
                    CastHandler.Cast.Spell.Level);

                Source.Fight.AddMark(rune);
            }
        }
    }
}