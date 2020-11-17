﻿using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Buffs
{
    public class StateBuff : Buff , ITriggerToken
    {
        public SpellStateRecord Record
        {
            get;
            private set;
        }
        public StateBuff(int id, SpellStateRecord record, SpellCast cast, Fighter target, EffectDice effect, FightDispellableEnum dispellable) : base(id, cast, target, effect, dispellable)
        {
            this.Record = record;
        }

        public override void Apply()
        {

        }

        public override void Dispell()
        {

        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostStateEffect()
            {
                delta = (short)Record.Id,
                dispelable = (byte)Dispellable,
                effectId = Effect.EffectId,
                parentBoostUid = 0,
                spellId = Cast.SpellId,
                stateId = (short)Record.Id,
                targetId = Target.Id,
                turnDuration = (short)Duration,
                uid = Id,
            };
        }

        public override short GetDelta()
        {
            throw new NotImplementedException();
        }
    }
}
