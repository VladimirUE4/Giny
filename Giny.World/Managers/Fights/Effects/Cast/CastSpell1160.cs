﻿using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Cast
{
    [SpellEffectHandler(EffectsEnum.Effect_CastSpell_1160)]
    public class CastSpell1160 : SpellEffectHandler
    {
        public CastSpell1160(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            SpellRecord spellRecord = SpellRecord.GetSpellRecord((short)Effect.Min);
            SpellLevelRecord level = spellRecord.GetLevel((byte)Effect.Max);

            Spell spell = new Spell(spellRecord, level);

            foreach (var target in targets)
            {
                SpellCast cast = new SpellCast(Source, spell, target.Cell, CastHandler.Cast);
                cast.Token = this.GetTriggerToken<ITriggerToken>();
                cast.Force = true;
                cast.Silent = true;
                Source.CastSpell(cast);
            }
           
        }
    }
}
