using Giny.Core.DesignPattern;
using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Movements
{
    [WIP]
    [SpellEffectHandler(EffectsEnum.Effect_1101)]
    public class TeleportToRune : SpellEffectHandler
    {
        public TeleportToRune(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            var rune = Source.GetMarks<Rune>().LastOrDefault();

            if (rune != null)
            {
                Telefrag telefrag = Source.Teleport(Source, rune.CenterCell,CanTrigger());

                if (telefrag != null)
                {
                    CastHandler.AddTelefrag(telefrag);
                }

                Source.Fight.RemoveMark(rune);

            }
            else
            {
                Source.Fight.Warn("Unable to teleport to rune...");
            }
        }
    }
}
