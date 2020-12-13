using Giny.Protocol.Enums;
using Giny.Protocol.Types;
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
    public class DisableStateBuff : Buff
    {
        public short StateId
        {
            get;
            private set;
        }
        public DisableStateBuff(int id, short stateId, SpellCast cast, Fighter target, EffectDice effect, FightDispellableEnum dispellable, short? customActionId = null) : base(id, cast, target, effect, dispellable, customActionId)
        {
            this.StateId = stateId;
        }

        public override void Apply()
        {
           
        }

        public override void Dispell()
        {

        }

        public override short GetDelta()
        {
            return StateId;
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostStateEffect()
            {
                delta = -100,
                dispelable = (byte)Dispellable,
                effectId = Effect.EffectId,
                parentBoostUid = 0,
                spellId = Cast.SpellId,
                stateId = StateId,
                targetId = Target.Id,
                turnDuration = (short)Duration,
                uid = Id,
            };
        }
    }
}
