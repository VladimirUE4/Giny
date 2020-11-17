﻿using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Buffs
{
    public class StatBuff : Buff
    {
        public short Value
        {
            get;
            private set;
        }
        public Characteristic Characteristic
        {
            get;
            set;
        }
        public short Delta
        {
            get;
            private set;
        }

        public StatBuff(int id, SpellCast cast, Fighter target, EffectDice effect, bool critical, FightDispellableEnum dispelable,
             Characteristic characteristic, short delta, short? customActionId = null)
            : base(id, cast, target, effect, dispelable, customActionId)
        {
            this.Characteristic = characteristic;
            this.Delta = delta;
        }

        public override void Apply()
        {
            Characteristic.Context += Delta;
        }

        public override void Dispell()
        {
            Characteristic.Context -= Delta;
        }

        public override short GetDelta()
        {
            return Delta;
        }
    }
}
