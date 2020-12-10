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
        public ServerEntityLook Look
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
            base.Target.ChangeLook(Look.Clone(), Cast.Source);
        }

        public override void Dispell()
        {
            LookBuff lookBuff = this.Target.GetBuffs().OfType<LookBuff>().LastOrDefault(x => x != this);

            if (lookBuff != null)
            {
                lookBuff.Apply();
            }
            else
            {
                Target.ChangeLook(Target.BaseLook.Clone(), Cast.Source);

            }
        }

        public override short GetDelta()
        {
            return 0;
        }
    }
}
