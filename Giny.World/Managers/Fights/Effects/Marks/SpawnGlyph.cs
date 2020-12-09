using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Marks;
using Giny.World.Managers.Maps.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Marks
{
    [SpellEffectHandler(EffectsEnum.Effect_Glyph)]
    public class SpawnGlyph : SpellEffectHandler
    {
        public SpawnGlyph(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            if (!Source.Fight.MarkExist<Glyph>(x => x.CenterCell == TargetCell))
            {
               /* Zone zone = Effect.GetZone();
                Color color = Color.FromArgb(Effect.Value);

                Glyph glyph = new Glyph(Source.Fight.PopNextMarkId(), Effect,
                     zone, MarkTriggerType.None, color, Source, TargetCell, CastHandler.Cast.Spell.Record, CastHandler.Cast.Spell.Level);

                Source.Fight.AddMark(glyph); */
            }
        }
    }
}
