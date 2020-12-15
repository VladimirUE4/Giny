using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Summons
{
    [SpellEffectHandler(EffectsEnum.Effect_KillAndSummonSlave)]
    public class KillAndSummonSlave : SpellEffectHandler
    {
        public KillAndSummonSlave(EffectDice effect, SpellCastHandler castHandler) : base(effect, castHandler)
        {
        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            if (!(Source is CharacterFighter))
            {
                return;
            }

            foreach (var target in targets)
            {
                if (target.IsSummoned() && target.GetSummoner() == Source)
                {
                    target.Die(Source);

                    SummonedMonster summon = CreateSummon((short)Effect.Min);
                    summon.SetController((CharacterFighter)Source);
                    Source.Fight.AddSummon(Source, summon);
                }
            }
        }
    }
}
