using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.World.Managers.Fights.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Custom
{
    public class Zobal
    {
        [SpellCastHandler(SpellEnum.TirelessMask)]
        [SpellCastHandler(SpellEnum.IntrepidMask)]
        [SpellCastHandler(SpellEnum.PoltroonMask13406)]
        [SpellCastHandler(SpellEnum.PsychopathMask13388)]
        [SpellCastHandler(SpellEnum.HystericalMask)]
        [SpellCastHandler(SpellEnum.CowardMask13387)]
        public class MaskSpell : DefaultSpellCastHandler
        {
            public MaskSpell(SpellCast cast) : base(cast)
            {

            }
            protected override IEnumerable<SpellEffectHandler> OrderHandlers()
            {
                EffectsEnum[] order = new EffectsEnum[]
                {
                    EffectsEnum.Effect_RemoveSpellEffects,
                    EffectsEnum.Effect_AddState,
                    EffectsEnum.Effect_ChangeAppearance_335,
                    EffectsEnum.Effect_CooldownSet,
                };

                return OrderByEffects(order);
            }
        }
    }
}
