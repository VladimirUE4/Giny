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

namespace Giny.DatabasePatcher.Patchs
{
    public class SpellCategoryManager
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
            EffectsEnum.Effect_DamageFirePerAP,
            EffectsEnum.Effect_DamageWaterPerAP,
            EffectsEnum.Effect_SwitchPosition,
            EffectsEnum.Effect_SwitchPosition_1023,
            EffectsEnum.Effect_SymetricPointTeleport,
            EffectsEnum.Effect_PushBack,
            EffectsEnum.Effect_PullForward,
        };
        private static readonly EffectsEnum[] HealingEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_HealHP_108,
            EffectsEnum.Effect_HealHP_143,
            EffectsEnum.Effect_HealHP_81,
            EffectsEnum.Effect_AddVitality,
            EffectsEnum.Effect_AddVitalityPercent,
            EffectsEnum.Effect_HealHP_407,
        };
        private static readonly EffectsEnum[] BuffEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_AddAP_111,
            EffectsEnum.Effect_AddMP_128,
            EffectsEnum.Effect_AddMP,
            EffectsEnum.Effect_AddShieldPercent,
            EffectsEnum.Effect_AddShield,
            EffectsEnum.Effect_AddState,
        };
        private static readonly EffectsEnum[] MarkEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_Trap,
            EffectsEnum.Effect_Glyph_1165,
            EffectsEnum.Effect_GlyphAura,
        };
        private static readonly EffectsEnum[] SummonEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_Summon,
            EffectsEnum.Effect_KillAndSummon,
        };
        private static readonly EffectsEnum[] TeleportEffects = new EffectsEnum[]
        {
            EffectsEnum.Effect_Teleport,
        };

        public static void Initialize()
        {
            Logger.WriteColor1("Database Patcher > Assigning spell categories....");

            foreach (var spell in SpellRecord.GetSpellRecords())
            {
                AssignCategory(spell);
            }
        }


        private static void AssignCategory(SpellRecord record)
        {
            SpellCategoryEnum category = SpellCategoryEnum.None;

            SpellLevelRecord level = record.Levels.LastOrDefault();

            if (level.Effects.Any(x => SummonEffects.Contains(x.EffectEnum)))
            {
                category |= SpellCategoryEnum.Summon;
            }
            if (level.Effects.Any(x => DamageEffects.Contains(x.EffectEnum)))
            {
                category = SpellCategoryEnum.Damages;
            }
            if (level.Effects.Any(x => HealingEffects.Contains(x.EffectEnum)))
            {
                category |= SpellCategoryEnum.Healing;
            }
            if (level.Effects.Any(x => BuffEffects.Contains(x.EffectEnum)))
            {
                category |= SpellCategoryEnum.Buff;
            }
            if (level.Effects.Any(x => TeleportEffects.Contains(x.EffectEnum)))
            {
                category |= SpellCategoryEnum.Teleport;
            }
            if (category == SpellCategoryEnum.None)
            {
                category = SpellCategoryEnum.Damages;
            }

            record.Category = category;

            record.UpdateInstantElement();
             
        }
    }
}
