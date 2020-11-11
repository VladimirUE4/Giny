using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Look;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Buffs
{
    public class LookBuff : Buff
    {
        private ServerEntityLook Look
        {
            get;
            set;
        }
        public LookBuff(int id, ServerEntityLook look, SpellCast cast, Fighter target, EffectDice effect, FightDispellableEnum dispellable, short? customActionId = null) : base(id, cast, target, effect, dispellable, customActionId)
        {
            this.Look = look;
        }

        public override void Apply()
        {
            base.Target.ChangeLook(Look, Cast.Source);
        }

        public override void Dispell()
        {
            base.Target.ChangeLook(Target.BaseLook.Clone(), Cast.Source);
        }

        public override short GetDelta()
        {
            return 0;
        }
    }
}
