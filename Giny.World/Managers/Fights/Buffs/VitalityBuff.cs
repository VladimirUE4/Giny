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
        private const ActionsEnum ActionId = ActionsEnum.ACTION_CHARACTER_BOOST_VITALITY;

        private short Delta
        {
            get;
            set;
        }
        public VitalityBuff(int id, short delta, SpellCast cast, Fighter target, EffectDice effect, FightDispellableEnum dispellable) :
            base(id, cast, target, effect, dispellable, (short)ActionId)
        {
            this.Delta = delta;
        }

        public override void Apply()
        {
            Target.Stats.AddVitality(Delta);
        }

        public override void Dispell()
        {
            Target.Stats.RemoveVitality(Delta);
        }

        public override short GetDelta()
        {
            return Delta;
        }
    }
}
