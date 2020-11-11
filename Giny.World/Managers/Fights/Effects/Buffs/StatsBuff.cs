using Giny.Core.Time;
using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Stats;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Buffs
{
    [SpellEffectHandler(EffectsEnum.Effect_AddDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddRange_136)]
    [SpellEffectHandler(EffectsEnum.Effect_AddWisdom)]
    [SpellEffectHandler(EffectsEnum.Effect_AddAgility)]
    [SpellEffectHandler(EffectsEnum.Effect_IncreaseDamage_138)]
    [SpellEffectHandler(EffectsEnum.Effect_AddStrength)]
    [SpellEffectHandler(EffectsEnum.Effect_AddChance)]
    [SpellEffectHandler(EffectsEnum.Effect_AddIntelligence)]
    [SpellEffectHandler(EffectsEnum.Effect_AddRange)]
    [SpellEffectHandler(EffectsEnum.Effect_AddWeaponDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_MeleeDamageDonePercent)]
    [SpellEffectHandler(EffectsEnum.Effect_AddCriticalHit)]
    [SpellEffectHandler(EffectsEnum.Effect_RangedDamageDonePercent)]
    public class StatsBuff : SpellEffectHandler
    {
        public const FightDispellableEnum Dispellable = FightDispellableEnum.DISPELLABLE;

        public StatsBuff(EffectDice effect, SpellCastHandler castHandler) :
            base(effect, castHandler)
        {

        }

        protected override int Priority => 1;

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            short delta = Effect.GetDelta();

            foreach (var target in targets)
            {
                Characteristic characteristic = GetEffectCaracteristic(target);
                AddStatBuff(target, delta, characteristic, Dispellable);
            }

        }

        private Characteristic GetEffectCaracteristic(Fighter target)
        {
            switch (Effect.EffectEnum)
            {
                case EffectsEnum.Effect_AddRange:
                    return target.Stats.Range;

                case EffectsEnum.Effect_AddRange_136:
                    return target.Stats.Range;

                case EffectsEnum.Effect_IncreaseDamage_138:
                    return target.Stats.DamagesBonusPercent;

                case EffectsEnum.Effect_AddAgility:
                    return target.Stats.Agility;

                case EffectsEnum.Effect_AddChance:
                    return target.Stats.Chance;

                case EffectsEnum.Effect_AddIntelligence:
                    return target.Stats.Intelligence;

                case EffectsEnum.Effect_AddStrength:
                    return target.Stats.Strength;

                case EffectsEnum.Effect_AddWisdom:
                    return target.Stats.Wisdom;

                case EffectsEnum.Effect_AddWeaponDamageBonus:
                    return target.Stats.WeaponDamagesBonusPercent;

                case EffectsEnum.Effect_MeleeDamageDonePercent:
                    return target.Stats.MeleeDamageDonePercent;

                case EffectsEnum.Effect_RangedDamageDonePercent:
                    return target.Stats.RangedDamageDonePercent;

                case EffectsEnum.Effect_AddDamageBonus:
                    return target.Stats.AllDamagesBonus;

                case EffectsEnum.Effect_AddCriticalHit:
                    return target.Stats.CriticalHit;

                default:
                    target.Fight.Warn(Effect.EffectEnum + " cannot be applied to stat buff.");
                    return null;
            }
        }
    }
}
