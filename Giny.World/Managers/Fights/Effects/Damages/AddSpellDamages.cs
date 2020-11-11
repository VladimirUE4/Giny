using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Damages
{
    [SpellEffectHandler(EffectsEnum.Effect_2828)]
    public class AddSpellDamages : SpellEffectHandler
    {
        public AddSpellDamages(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override int Priority => 0;

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            var test = Effect;
            foreach (var target in targets)
            {

            }
        }
    }
}
