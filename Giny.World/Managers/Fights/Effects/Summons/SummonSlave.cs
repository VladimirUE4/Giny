using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Summons
{
    [SpellEffectHandler(EffectsEnum.Effect_SummonSlave)]
    public class SummonSlave : SpellEffectHandler
    {
        public SummonSlave(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            if (!(Source is CharacterFighter))
            {
                return;
            }

            if (!Source.CanSummon())
            {
                return;
            }

            SummonedMonster fighter = CreateSummon((short)Effect.Min, (byte)Effect.Max);
            fighter.SetController((CharacterFighter)Source);
            Source.Fight.AddSummon(Source, fighter);
        }
    }
}
