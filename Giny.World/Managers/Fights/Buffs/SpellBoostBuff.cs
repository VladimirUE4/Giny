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
    public class SpellBoostBuff : Buff
    {
        public short Delta
        {
            get;
            set;
        }
        public short BoostedSpellId
        {
            get;
            set;
        }
        public SpellBoostBuff(int id, short boostedSpellId, short delta, SpellCast cast, Fighter target, EffectDice effect, FightDispellableEnum dispellable) : base(id, cast, target, effect, dispellable)
        {
            this.BoostedSpellId = boostedSpellId;
            this.Delta = delta;
        }

        public override void Apply()
        {

        }

        public override void Dispell()
        {

        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporarySpellBoostEffect()
            {
                effectId = Effect.EffectId,
                delta = Delta,
                boostedSpellId = BoostedSpellId,
                dispelable = (byte)Dispellable,
                parentBoostUid = 0,
                spellId = BoostedSpellId, // Cast.SpellId
                targetId = Target.Id,
                turnDuration = (short)(Duration == -1 ? -1000 : Duration),//(short)Duration,
                uid = Id,
            };
        }

        public override short GetDelta()
        {
            throw new InvalidOperationException();
        }
    }
}
