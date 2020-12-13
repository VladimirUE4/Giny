using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Movements
{
    [SpellEffectHandler(EffectsEnum.Effect_SymetricTargetTeleport)]
    public class SymetricTargetTeleport : SpellEffectHandler
    {
        public SymetricTargetTeleport(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            foreach (var target in targets)
            {
                var targetPoint = new MapPoint((2 * target.Cell.Point.X - Source.Cell.Point.X), (2 * target.Cell.Point.Y - Source.Cell.Point.Y));

                if (MapPoint.IsInMap(targetPoint.X, targetPoint.Y))
                {
                    var telefrag = Source.Teleport(Source, Source.Fight.Map.GetCell(targetPoint), CanTrigger());

                    if (telefrag != null)
                    {
                        this.CastHandler.AddTelefrag(telefrag);
                    }
                }
            }
        }
    }
}
