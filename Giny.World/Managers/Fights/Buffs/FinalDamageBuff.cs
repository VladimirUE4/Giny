using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Buffs
{
    public class FinalDamageBuff : Buff
    {
        private short Delta
        {
            get;
            set;
        }
        public FinalDamageBuff(int id, short delta, SpellCast cast, Fighter target, EffectDice effect, FightDispellableEnum dispellable, short? customActionId = null) : base(id, cast, target, effect, dispellable, customActionId)
        {
            this.Delta = delta;
        }

        public override void Apply()
        {
            Target.Stats.FinalDamagePercent += Delta;
        }

        public override void Dispell()
        {
            Target.Stats.FinalDamagePercent -= Delta;
        }

        public override short GetDelta()
        {
            return Delta;
        }
    }
}
