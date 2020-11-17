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
    [SpellEffectHandler(EffectsEnum.Effect_AddDodgeMPProbability)]
    [SpellEffectHandler(EffectsEnum.Effect_AddDodgeAPProbability)]
    [SpellEffectHandler(EffectsEnum.Effect_AddMeleeResistance)]
    [SpellEffectHandler(EffectsEnum.Effect_AddRangedResistance)]
    [SpellEffectHandler(EffectsEnum.Effect_AddSpellResistance)]
    [SpellEffectHandler(EffectsEnum.Effect_AddLock)]
    [SpellEffectHandler(EffectsEnum.Effect_AddWaterDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddFireDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddAirDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddEarthDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddNeutralDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddAPAttack)]
    [SpellEffectHandler(EffectsEnum.Effect_AddMPAttack)]
    [SpellEffectHandler(EffectsEnum.Effect_AddFireResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_AddWaterResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_AddEarthResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_AddAirResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_AddNeutralResistPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_AddDodge)]
    [SpellEffectHandler(EffectsEnum.Effect_AddDamageBonusPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_AddNeutralElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_AddFireElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_AddAirElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_AddEarthElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_AddWaterElementReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_AddTrapBonusPercent)]
    [SpellEffectHandler(EffectsEnum.Effect_WeaponDamageDonePercent)]
    [SpellEffectHandler(EffectsEnum.Effect_AddPushDamageReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_AddTrapBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddHealBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddPushDamageBonus)]
    [SpellEffectHandler(EffectsEnum.Effect_AddCriticalDamageReduction)]
    [SpellEffectHandler(EffectsEnum.Effect_AddCriticalDamageBonus)]
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

                case EffectsEnum.Effect_AddDodgeMPProbability:
                    return target.Stats.DodgePMProbability;

                case EffectsEnum.Effect_AddDodgeAPProbability:
                    return target.Stats.DodgePAProbability;

                case EffectsEnum.Effect_AddMeleeResistance:
                    return target.Stats.MeleeDamageResistancePercent;

                case EffectsEnum.Effect_AddRangedResistance:
                    return target.Stats.RangedDamageResistancePercent;

                case EffectsEnum.Effect_AddSpellResistance:
                    return target.Stats.SpellDamageResistancePercent;

                case EffectsEnum.Effect_AddLock:
                    return target.Stats.TackleBlock;

                case EffectsEnum.Effect_AddWaterDamageBonus:
                    return target.Stats.WaterDamageBonus;

                case EffectsEnum.Effect_AddFireDamageBonus:
                    return target.Stats.FireDamageBonus;

                case EffectsEnum.Effect_AddAirDamageBonus:
                    return target.Stats.AirDamageBonus;

                case EffectsEnum.Effect_AddEarthDamageBonus:
                    return target.Stats.EarthDamageBonus;

                case EffectsEnum.Effect_AddNeutralDamageBonus:
                    return target.Stats.NeutralDamageBonus;

                case EffectsEnum.Effect_AddAPAttack:
                    return target.Stats.APAttack;

                case EffectsEnum.Effect_AddMPAttack:
                    return target.Stats.MPAttack;

                case EffectsEnum.Effect_AddFireResistPercent:
                    return target.Stats.FireResistPercent;

                case EffectsEnum.Effect_AddWaterResistPercent:
                    return target.Stats.WaterResistPercent;

                case EffectsEnum.Effect_AddEarthResistPercent:
                    return target.Stats.EarthResistPercent;

                case EffectsEnum.Effect_AddAirResistPercent:
                    return target.Stats.AirResistPercent;

                case EffectsEnum.Effect_AddNeutralResistPercent:
                    return target.Stats.NeutralResistPercent;

                case EffectsEnum.Effect_AddDodge:
                    return target.Stats.TackleEvade;

                case EffectsEnum.Effect_AddDamageBonusPercent:
                    return target.Stats.DamagesBonusPercent;

                case EffectsEnum.Effect_AddNeutralElementReduction:
                    return target.Stats.NeutralReduction;

                case EffectsEnum.Effect_AddFireElementReduction:
                    return target.Stats.FireReduction;

                case EffectsEnum.Effect_AddAirElementReduction:
                    return target.Stats.AirReduction;

                case EffectsEnum.Effect_AddEarthElementReduction:
                    return target.Stats.EarthReduction;

                case EffectsEnum.Effect_AddWaterElementReduction:
                    return target.Stats.WaterReduction;

                case EffectsEnum.Effect_AddTrapBonusPercent:
                    return target.Stats.TrapBonusPercent;

                case EffectsEnum.Effect_WeaponDamageDonePercent:
                    return target.Stats.WeaponDamageDonePercent;

                case EffectsEnum.Effect_AddPushDamageReduction:
                    return target.Stats.PushDamageReduction;

                case EffectsEnum.Effect_AddTrapBonus:
                    return target.Stats.TrapBonus;

                case EffectsEnum.Effect_AddHealBonus:
                    return target.Stats.HealBonus;

                case EffectsEnum.Effect_AddPushDamageBonus:
                    return target.Stats.PushDamageBonus;

                case EffectsEnum.Effect_AddCriticalDamageReduction:
                    return target.Stats.CriticalDamageReduction;

                case EffectsEnum.Effect_AddCriticalDamageBonus:
                    return target.Stats.CriticalDamageBonus;

                default:
                    target.Fight.Warn(Effect.EffectEnum + " cannot be applied to stat buff.");
                    return null;
            }
        }
    }
}
