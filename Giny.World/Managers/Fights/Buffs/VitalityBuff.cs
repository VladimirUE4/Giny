using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Actions;
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
    public class VitalityBuff : Buff
    {
        private short Delta
        {
            get;
            set;
        }
        public VitalityBuff(int id, short delta, SpellCast cast, Fighter target, EffectDice effect, FightDispellableEnum dispellable,
            ActionsEnum actionId) :
            base(id, cast, target, effect, dispellable, (short)actionId)
        {
            this.Delta = delta;
        }

        public override void Apply()
        {
            if (Delta < 0)
            {
                Target.Stats.RemoveVitality(Math.Abs(Delta));
            }
            else
            {
                Target.Stats.AddVitality(Delta);
            }
        }

        public override void Dispell()
        {
            if (Delta < 0)
            {
                Target.Stats.AddVitality(Delta);
            }
            else
            {
                Target.Stats.RemoveVitality(Math.Abs(Delta));
            }
        }

        public override short GetDelta()
        {
            return Math.Abs(Delta);
        }
    }
}
