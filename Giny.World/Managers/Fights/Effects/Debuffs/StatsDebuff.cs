using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects.Debuffs
{
    [SpellEffectHandler(EffectsEnum.Effect_SubDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_SubWisdom)]
    [SpellEffectHandler(EffectsEnum.Effect_SubAgility)]
    [SpellEffectHandler(EffectsEnum.Effect_SubDamageBonusPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_SubStrength)]
    [SpellEffectHandler(EffectsEnum.Effect_SubChance)]
    [SpellEffectHandler(EffectsEnum.Effect_SubIntelligence)]
    [SpellEffectHandler(EffectsEnum.Effect_SubRange)]
    [SpellEffectHandler(EffectsEnum.Effect_SubMeleeDamageDonePercent)]
    [SpellEffectHandler(EffectsEnum.Effect_SubCriticalHit)]
    [SpellEffectHandler(EffectsEnum.Effect_SubRangedDamageDonePercent)]
    [SpellEffectHandler(EffectsEnum.Effect_SubDodgeMPProbability)]
    [SpellEffectHandler(EffectsEnum.Effect_SubDodgeAPProbability)]
    [SpellEffectHandler(EffectsEnum.Effect_SubMeleeResistance)]
    [SpellEffectHandler(EffectsEnum.Effect_SubRangedResistance)]
    [SpellEffectHandler(EffectsEnum.Effect_SubSpellResistance)]
    [SpellEffectHandler(EffectsEnum.Effect_SubLock)]
    [SpellEffectHandler(EffectsEnum.Effect_SubFireResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_SubEarthResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_SubNeutralResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_SubAirResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_SubWaterResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_SubEvade)]
    [SpellEffectHandler(EffectsEnum.Effect_SubAPAttack)]
    [SpellEffectHandler(EffectsEnum.Effect_SubMPAttack)]
    [SpellEffectHandler(EffectsEnum.Effect_SubEarthElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_SubAirElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_SubFireElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_SubWaterElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_SubNeutralElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_SubPushDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_SubPushDamageReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_SubHealBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_SubCriticalDamageReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_SubSpellDamageDonePercent)]
    public class StatsDebuff : SpellEffectHandler
    {
        public const FightDispellableEnum Dispellable = FightDispellableEnum.DISPELLABLE;

        public StatsDebuff(EffectDice effect, SpellCastHandler castHandler) :
            base(effect, castHandler)
        {

        }

        protected override void Apply(IEnumerable<Fighter> targets)
        {
            short delta = Effect.GetDelta();

            foreach (var target in targets)
            {
                Characteristic characteristic = GetEffectCaracteristic(target);
                AddStatBuff(target, (short)-delta, characteristic, Dispellable);
            }

        }

        private Characteristic GetEffectCaracteristic(Fighter target)
        {
            switch (Effect.EffectEnum)
            {
                case EffectsEnum.Effect_SubRange:
                    return target.Stats.Range;

                case EffectsEnum.Effect_SubDamageBonusPercent:
                    return target.Stats.DamagesBonusPercent;

                case EffectsEnum.Effect_SubAgility:
                    return target.Stats.Agility;

                case EffectsEnum.Effect_SubChance:
                    return target.Stats.Chance;

                case EffectsEnum.Effect_SubIntelligence:
                    return target.Stats.Intelligence;

                case EffectsEnum.Effect_SubStrength:
                    return target.Stats.Strength;

                case EffectsEnum.Effect_SubWisdom:
                    return target.Stats.Wisdom;

                case EffectsEnum.Effect_SubMeleeDamageDonePercent:
                    return target.Stats.MeleeDamageDonePercent;

                case EffectsEnum.Effect_SubRangedDamageDonePercent:
                    return target.Stats.RangedDamageDonePercent;

                case EffectsEnum.Effect_SubDamageBonus:
                    return target.Stats.AllDamagesBonus;

                case EffectsEnum.Effect_SubCriticalHit:
                    return target.Stats.CriticalHit;

                case EffectsEnum.Effect_SubDodgeMPProbability:
                    return target.Stats.DodgePMProbability;

                case EffectsEnum.Effect_SubDodgeAPProbability:
                    return target.Stats.DodgePAProbability;

                case EffectsEnum.Effect_SubMeleeResistance:
                    return target.Stats.MeleeDamageResistancePercent;

                case EffectsEnum.Effect_SubRangedResistance:
                    return target.Stats.RangedDamageResistancePercent;

                case EffectsEnum.Effect_SubSpellResistance:
                    return target.Stats.SpellDamageResistancePercent;

                case EffectsEnum.Effect_SubLock:
                    return target.Stats.TackleBlock;

                case EffectsEnum.Effect_SubFireResistPercent:
                    return target.Stats.FireResistPercent;

                case EffectsEnum.Effect_SubEarthResistPercent:
                    return target.Stats.EarthResistPercent;

                case EffectsEnum.Effect_SubNeutralResistPercent:
                    return target.Stats.NeutralResistPercent;

                case EffectsEnum.Effect_SubAirResistPercent:
                    return target.Stats.AirResistPercent;

                case EffectsEnum.Effect_SubWaterResistPercent:
                    return target.Stats.WaterResistPercent;

                case EffectsEnum.Effect_SubEvade:
                    return target.Stats.TackleEvade;

                case EffectsEnum.Effect_SubAPAttack:
                    return target.Stats.APAttack;

                case EffectsEnum.Effect_SubMPAttack:
                    return target.Stats.MPAttack;

                case EffectsEnum.Effect_SubEarthElementReduction:
                    return target.Stats.EarthReduction;

                case EffectsEnum.Effect_SubAirElementReduction:
                    return target.Stats.AirReduction;

                case EffectsEnum.Effect_SubFireElementReduction:
                    return target.Stats.FireReduction;

                case EffectsEnum.Effect_SubWaterElementReduction:
                    return target.Stats.WaterReduction;

                case EffectsEnum.Effect_SubNeutralElementReduction:
                    return target.Stats.NeutralReduction;

                case EffectsEnum.Effect_SubPushDamageBonus:
                    return target.Stats.PushDamageBonus;

                case EffectsEnum.Effect_SubPushDamageReduction:
                    return target.Stats.PushDamageReduction;

                case EffectsEnum.Effect_SubHealBonus:
                    return target.Stats.HealBonus;

                case EffectsEnum.Effect_SubCriticalDamageReduction:
                    return target.Stats.CriticalDamageReduction;

                case EffectsEnum.Effect_SubSpellDamageDonePercent:
                    return target.Stats.SpellDamageDonePercent;

                default:
                    target.Fight.Warn(Effect.EffectEnum + " cannot be applied to stat buff.");
                    return null;
            }
        }
    }
}
