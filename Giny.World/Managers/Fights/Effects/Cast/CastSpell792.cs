﻿using Giny.Core.DesignPattern;
using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Maps;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Cast
{
    [WIP("cast spell ?")]
    [SpellEffectHandler(EffectsEnum.Effect_CastSpell792)]
    public class CastSpell792 : SpellEffectHandler
    {
        public CastSpell792(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override int Priority => 0;

        protected override void Apply(IEnumerable<Fighter> targets)
        {
           
            SpellRecord spellRecord = SpellRecord.GetSpellRecord((short)Effect.Min);
            SpellLevelRecord level = spellRecord.GetLevel((byte)Effect.Max);

            Spell spell = new Spell(spellRecord, level);

            foreach (var target in targets)
            {
                SpellCast cast = new SpellCast(target, spell, Source.Cell);
                cast.Token = this.GetTriggerToken<ITriggerToken>();
                cast.Force = true;
                cast.Silent = true;
                cast.SilentNetwork = true;
                Source.CastSpell(cast);
            }

        }
    }
}
