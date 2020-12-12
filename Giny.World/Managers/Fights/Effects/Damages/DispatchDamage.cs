using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Cast.Units;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Damages
{
    [SpellEffectHandler(EffectsEnum.Effect_DispatchDamages)]
    public class DispatchDamage : SpellEffectHandler
    {
        public override bool RefreshTargets => false;

        public DispatchDamage(EffectDice effect, SpellCastHandler castHandler) :
            base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            Damage damages = GetTriggerToken<Damage>();

            if (damages != null)
            {
                damages.Computed = (short)(damages.Computed * (Effect.Min / 100d));

                foreach (var fighter in Source.GetMeleeFighters())
                {
                    fighter.InflictDamage(damages);
                }
            }
            else
            {
                OnTokenMissing<Damage>();
            }
        }
    }
}
