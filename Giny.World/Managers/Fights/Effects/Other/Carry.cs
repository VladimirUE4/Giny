using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Other
{
    [SpellEffectHandler(EffectsEnum.Effect_Carry)]
    public class Carry : SpellEffectHandler
    {
        public const short CarrySpellState = 3;

        public const short CarriedSpellState = 8;

        public Carry(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {

            foreach (var target in targets)
            {
                if (!Source.IsCarrying() && target.CanBeCarried())
                {
                    Source.Carry(target);

                    AddStateBuff(Source, SpellStateRecord.GetSpellStateRecord(CarrySpellState), FightDispellableEnum.REALLY_NOT_DISPELLABLE, -1);
                    AddStateBuff(target, SpellStateRecord.GetSpellStateRecord(CarriedSpellState), FightDispellableEnum.REALLY_NOT_DISPELLABLE, -1);
                }
            }

           
        }
    }
}
