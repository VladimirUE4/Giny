using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Actions;
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
    public class SpellReduceApCostBuff : Buff
    {
        public const ActionsEnum ActionEnum = ActionsEnum.ACTION_BOOST_SPELL_AP_COST;

        private short SpellId
        {
            get;
            set;
        }
        private short Delta
        {
            get;
            set;
        }
        private Spell TargetSpell
        {
            get;
            set;
        }
        public SpellReduceApCostBuff(int id, short spellId, short delta, Fighter target, SpellEffectHandler effectHandler, FightDispellableEnum dispellable) : base(id, target, effectHandler, dispellable, (short)ActionEnum)
        {
            this.SpellId = spellId;
            this.TargetSpell = Target.GetSpell(spellId);
            this.Delta = delta;
        }

        public override void Apply()
        {
            Target.ReduceApCost(TargetSpell.Level, Delta);
        }

        public override void Dispell()
        {
            Target.ReduceApCost(TargetSpell.Level, (short)(-Delta));
        }

        public override short GetDelta()
        {
            return Delta;
        }
        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            Target.Fight.Warn(SpellId.ToString());
            return new FightTemporarySpellBoostEffect()
            {
                boostedSpellId = SpellId,
                delta = Delta,
                dispelable = (byte)Dispellable,
                effectId = Effect.EffectId,
                parentBoostUid = 0,
                spellId = SpellId,
                targetId = Target.Id,
                turnDuration = (short)(Duration == -1 ? -1000 : Duration),
                uid = Id,
            };
        }
    }
}
