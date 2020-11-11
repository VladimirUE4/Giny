using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.ORM;
using Giny.Protocol.Enums;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Misc
{
    public class SpellCategoryManager : Singleton<SpellCategoryManager>
    {
        private static readonly EffectsEnum[] DamageEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_DamageFire,
            EffectsEnum.Effect_DamageWater,
            EffectsEnum.Effect_DamageNeutral,
            EffectsEnum.Effect_DamageAir,
            EffectsEnum.Effect_DamageEarth,
            EffectsEnum.Effect_StealHPAir,
            EffectsEnum.Effect_StealHPFire,
            EffectsEnum.Effect_StealHPWater,
            EffectsEnum.Effect_StealHPFix,
            EffectsEnum.Effect_StealHPNeutral,
        };
        private static readonly EffectsEnum[] HealingEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_HealHP_108,
        };
        private static readonly EffectsEnum[] BuffEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_AddAP_111,
        };

        [StartupInvoke("Spell category manager", StartupInvokePriority.Last)]
        public void Initialize()
        {
            foreach (var spell in SpellRecord.GetSpellRecords())
            {
                AssignCategory(spell);
            }
        }


        private void AssignCategory(SpellRecord record)
        {
            SpellCategoryEnum category = SpellCategoryEnum.None;

            SpellLevelRecord level = record.Levels.LastOrDefault();

            if (level.Effects.Any(x => DamageEffects.Contains(x.EffectEnum)))
            {
                category = SpellCategoryEnum.Damages;
            }
            if (level.Effects.Any(x => HealingEffects.Contains(x.EffectEnum)))
            {
                category |= SpellCategoryEnum.Healing;
            }

            record.Category = category;

            record.UpdateInstantElement();

            Logger.Write(record.Name + " assigned to category : " + category);
        }
    }
}
