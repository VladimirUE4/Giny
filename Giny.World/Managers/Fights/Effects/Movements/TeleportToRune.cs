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
    [SpellEffectHandler(EffectsEnum.Effect_1101)]
    public class TeleportToRune : SpellEffectHandler
    {
        public TeleportToRune(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override int Priority => 0;

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            foreach (var target in targets)
            {
                var rune = target.Fight.GetMarks(target).OfType<Rune>().FirstOrDefault();

                Telefrag telefrag = target.Teleport(Source, rune.CenterCell);

                if (telefrag != null)
                {
                    CastHandler.AddTelefrag(telefrag);
                }

                target.Fight.RemoveMark(rune);


            }
        }
    }
}
