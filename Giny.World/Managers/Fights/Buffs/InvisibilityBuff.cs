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
    public class InvisibilityBuff : Buff
    {
        public InvisibilityBuff(int id, SpellCast cast, Fighter target, EffectDice effect, FightDispellableEnum dispellable, short? customActionId = null) : base(id, cast, target, effect, dispellable, customActionId)
        {
        }

        public override void Apply()
        {
            Target.SetInvisiblityState(GameActionFightInvisibilityStateEnum.INVISIBLE, Cast.Source);
        }

        public override void Dispell()
        {
            Target.SetInvisiblityState(GameActionFightInvisibilityStateEnum.VISIBLE, Cast.Source);
        }

        public override short GetDelta()
        {
            return 0;
        }
    }
}
