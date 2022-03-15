using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights.Cast.Units;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Monsters;
using Giny.World.Managers.Stats;
using Giny.World.Records.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Stats
{
    public class FighterStats : EntityStats
    {
        public const short NaturalErosion = 10;

        public const short MaxErosion = 50;

        public GameActionFightInvisibilityStateEnum InvisibilityState
        {
            get;
            set;
        }
        public int ShieldPoints
        {
            get;
            private set;
        }

        public short ApUsed
        {
            get;
            private set;
        }

        public short MpUsed
        {
            get;
            private set;
        }
        public short Erosion
        {
            get;
            private set;
        }

        public int ErodedLife
        {
            get
            {
                return BaseMaxLife - MaxLifePoints;
            }
        }
        public int BaseMaxLife
        {
            get;
            set;
        }

        public short DamageMultiplier
        {
            get;
            set;
        }
        public short SpellDamageBonusPercent
        {
            get;
            set;
        }
        public short FinalDamagePercent
        {
            get;
            set;
        }

        public void AddErosion(short amount)
        {
            this.Erosion += amount;

            if (Erosion > MaxErosion)
            {
                Erosion = MaxErosion;
            }
        }
        public void RemoveErosion(short amount)
        {
            this.Erosion -= amount;

            if (Erosion < 0)
            {
                Erosion = 0;
            }
        }
        public void AddShield(short delta)
        {
            this.ShieldPoints += delta;
        }
        public void RemoveShield(int delta)
        {
            this.ShieldPoints -= delta;

            if (this.ShieldPoints < 0)
            {
                ShieldPoints = 0;
            }
        }
        public void AddMaxVitality(short delta)
        {
            this.BaseMaxLife += delta;
            this.MaxLifePoints += delta;
            this.LifePoints += delta;
        }
        public void RemoveMaxVitality(short delta)
        {
            this.BaseMaxLife -= delta;
            this.MaxLifePoints -= delta;
            this.LifePoints -= delta;

            if (LifePoints < 0)
            {
                LifePoints = 0;
            }
            if (MaxLifePoints < 0)
            {
                MaxLifePoints = 0;
            }
        }
        public void RemoveVitality(short delta)
        {
            this.LifePoints -= delta;

            if (LifePoints < 0)
            {
                LifePoints = 0;
            }
        }
        public void AddVitality(short delta)
        {
            this.LifePoints += delta;

            if (LifePoints >= MaxLifePoints)
            {
                LifePoints = MaxLifePoints;
            }
        }
        public void SetShield(short delta)
        {
            if (delta >= 0)
            {
                ShieldPoints = delta;
            }
            else
            {
                return;
                throw new Exception("Invalid shield value.");
            }
        }
        public void ResetUsedPoints()
        {
            MovementPoints.Context += MpUsed;
            ActionPoints.Context += ApUsed;
            MpUsed = 0;
            ApUsed = 0;
        }
        public void GainAp(short amount)
        {
            ActionPoints.Context += amount;
            ApUsed -= amount;

        }
        public void GainMp(short amount)
        {
            MovementPoints.Context += amount;
            MpUsed -= amount;
        }

        public void UseMp(short amount)
        {
            MovementPoints.Context -= amount;
            MpUsed += amount;
        }
        public void UseAp(short amount)
        {
            ActionPoints.Context -= amount;
            ApUsed += amount;
        }

        public FighterStats(Character character)
        {
            this.ActionPoints = (ApCharacteristic)character.Stats.ActionPoints.Clone();
            this.Agility = character.Stats.Agility.Clone();
            this.AirDamageBonus = character.Stats.AirDamageBonus.Clone();
            this.AirReduction = character.Stats.AirReduction.Clone();
            this.AirResistPercent = (ResistanceCharacteristic)character.Stats.AirResistPercent.Clone();
            this.AllDamagesBonus = character.Stats.AllDamagesBonus.Clone();
            this.Chance = character.Stats.Chance.Clone();
            this.CriticalDamageBonus = character.Stats.CriticalDamageBonus.Clone();
            this.CriticalDamageReduction = character.Stats.CriticalDamageReduction.Clone();
            this.CriticalHit = character.Stats.CriticalHit.Clone();
            this.CriticalHitWeapon = character.Stats.CriticalHitWeapon;
            this.DamagesBonusPercent = character.Stats.DamagesBonusPercent.Clone();
            this.DodgePAProbability = (PointDodgeCharacteristic)character.Stats.DodgePAProbability.Clone();
            this.DodgePMProbability = (PointDodgeCharacteristic)character.Stats.DodgePMProbability.Clone();
            this.EarthDamageBonus = character.Stats.EarthDamageBonus.Clone();
            this.EarthReduction = character.Stats.EarthReduction.Clone();
            this.EarthResistPercent = (ResistanceCharacteristic)character.Stats.EarthResistPercent.Clone();
            this.Energy = character.Stats.Energy;
            this.FireDamageBonus = character.Stats.FireDamageBonus.Clone();
            this.FireReduction = character.Stats.FireReduction.Clone();
            this.FireResistPercent = (ResistanceCharacteristic)character.Stats.FireResistPercent.Clone();
            this.GlobalDamageReduction = character.Stats.GlobalDamageReduction;
            this.GlyphBonusPercent = character.Stats.GlyphBonusPercent.Clone();
            this.HealBonus = character.Stats.HealBonus.Clone();
            this.Initiative = character.Stats.Initiative;
            this.Intelligence = character.Stats.Intelligence.Clone();
            this.LifePoints = character.Stats.LifePoints;
            this.MaxLifePoints = character.Stats.MaxLifePoints;
            this.MaxEnergyPoints = character.Stats.MaxEnergyPoints;
            this.MeleeDamageDonePercent = character.Stats.MeleeDamageDonePercent.Clone();
            this.MeleeDamageResistancePercent = character.Stats.MeleeDamageResistancePercent.Clone();
            this.MovementPoints = (MpCharacteristic)character.Stats.MovementPoints.Clone();
            this.NeutralDamageBonus = character.Stats.NeutralDamageBonus.Clone();
            this.NeutralReduction = character.Stats.NeutralReduction.Clone();
            this.NeutralResistPercent = (ResistanceCharacteristic)character.Stats.NeutralResistPercent.Clone();
            this.APAttack = (RelativeCharacteristic)character.Stats.APAttack.Clone();
            this.PermanentDamagePercent = character.Stats.PermanentDamagePercent.Clone();
            this.MPAttack = (RelativeCharacteristic)character.Stats.MPAttack.Clone();
            this.Prospecting = character.Stats.Prospecting;
            this.PushDamageBonus = character.Stats.PushDamageBonus.Clone();
            this.PushDamageReduction = character.Stats.PushDamageReduction.Clone();
            this.Range = (RangeCharacteristic)character.Stats.Range.Clone();
            this.RangedDamageDonePercent = character.Stats.RangedDamageDonePercent.Clone();
            this.RangedDamageResistancePercent = character.Stats.RangedDamageResistancePercent.Clone();
            this.Reflect = character.Stats.Reflect.Clone();
            this.RuneBonusPercent = character.Stats.RuneBonusPercent.Clone();
            this.SpellDamageDonePercent = character.Stats.SpellDamageDonePercent.Clone();
            this.SpellDamageResistancePercent = character.Stats.SpellDamageResistancePercent.Clone();
            this.Strength = character.Stats.Strength.Clone();
            this.SummonableCreaturesBoost = character.Stats.SummonableCreaturesBoost.Clone();
            this.TackleBlock = (RelativeCharacteristic)character.Stats.TackleBlock.Clone();
            this.TackleEvade = (RelativeCharacteristic)character.Stats.TackleEvade.Clone();
            this.TrapBonus = character.Stats.TrapBonus.Clone();
            this.TrapBonusPercent = character.Stats.TrapBonusPercent.Clone();
            this.Vitality = character.Stats.Vitality.Clone();
            this.WaterDamageBonus = character.Stats.WaterDamageBonus.Clone();
            this.WaterReduction = character.Stats.WaterReduction.Clone();
            this.WaterResistPercent = (ResistanceCharacteristic)character.Stats.WaterResistPercent.Clone();
            this.WeaponDamageDonePercent = character.Stats.WeaponDamageDonePercent.Clone();
            this.WeaponDamageResistancePercent = character.Stats.WeaponDamageResistancePercent.Clone();
            this.WeaponDamagesBonusPercent = character.Stats.WeaponDamagesBonusPercent.Clone();
            this.WeightBonus = character.Stats.WeightBonus.Clone();
            this.Wisdom = character.Stats.Wisdom.Clone();
            InvisibilityState = GameActionFightInvisibilityStateEnum.VISIBLE;
            this.BaseMaxLife = MaxLifePoints;
            this.Erosion = NaturalErosion;

        }
        public FighterStats(FighterStats other)
        {
            this.ActionPoints = (ApCharacteristic)other.ActionPoints.Clone();
            this.Agility = other.Agility.Clone();
            this.AirDamageBonus = other.AirDamageBonus.Clone();
            this.AirReduction = other.AirReduction.Clone();
            this.AirResistPercent = (ResistanceCharacteristic)other.AirResistPercent.Clone();
            this.AllDamagesBonus = other.AllDamagesBonus.Clone();
            this.Chance = other.Chance.Clone();
            this.CriticalDamageBonus = other.CriticalDamageBonus.Clone();
            this.CriticalDamageReduction = other.CriticalDamageReduction.Clone();
            this.CriticalHit = other.CriticalHit.Clone();
            this.CriticalHitWeapon = other.CriticalHitWeapon;
            this.DamagesBonusPercent = other.DamagesBonusPercent.Clone();
            this.DodgePAProbability = (PointDodgeCharacteristic)other.DodgePAProbability.Clone();
            this.DodgePMProbability = (PointDodgeCharacteristic)other.DodgePMProbability.Clone();
            this.EarthDamageBonus = other.EarthDamageBonus.Clone();
            this.EarthReduction = other.EarthReduction.Clone();
            this.EarthResistPercent = (ResistanceCharacteristic)other.EarthResistPercent.Clone();
            this.Energy = other.Energy;
            this.FireDamageBonus = other.FireDamageBonus.Clone();
            this.FireReduction = other.FireReduction.Clone();
            this.FireResistPercent = (ResistanceCharacteristic)other.FireResistPercent.Clone();
            this.GlobalDamageReduction = other.GlobalDamageReduction;
            this.GlyphBonusPercent = other.GlyphBonusPercent.Clone();
            this.HealBonus = other.HealBonus.Clone();
            this.Initiative = other.Initiative;
            this.Intelligence = other.Intelligence.Clone();
            this.LifePoints = other.LifePoints;
            this.MaxLifePoints = other.MaxLifePoints;
            this.MaxEnergyPoints = other.MaxEnergyPoints;
            this.MeleeDamageDonePercent = other.MeleeDamageDonePercent.Clone();
            this.MeleeDamageResistancePercent = other.MeleeDamageResistancePercent.Clone();
            this.MovementPoints = (MpCharacteristic)other.MovementPoints.Clone();
            this.NeutralDamageBonus = other.NeutralDamageBonus.Clone();
            this.NeutralReduction = other.NeutralReduction.Clone();
            this.NeutralResistPercent = (ResistanceCharacteristic)other.NeutralResistPercent.Clone();
            this.APAttack = (RelativeCharacteristic)other.APAttack.Clone();
            this.PermanentDamagePercent = other.PermanentDamagePercent.Clone();
            this.MPAttack = (RelativeCharacteristic)other.MPAttack.Clone();
            this.Prospecting = other.Prospecting;
            this.PushDamageBonus = other.PushDamageBonus.Clone();
            this.PushDamageReduction = other.PushDamageReduction.Clone();
            this.Range = (RangeCharacteristic)other.Range.Clone();
            this.RangedDamageDonePercent = other.RangedDamageDonePercent.Clone();
            this.RangedDamageResistancePercent = other.RangedDamageResistancePercent.Clone();
            this.Reflect = other.Reflect.Clone();
            this.RuneBonusPercent = other.RuneBonusPercent.Clone();
            this.SpellDamageDonePercent = other.SpellDamageDonePercent.Clone();
            this.SpellDamageResistancePercent = other.SpellDamageResistancePercent.Clone();
            this.Strength = other.Strength.Clone();
            this.SummonableCreaturesBoost = other.SummonableCreaturesBoost.Clone();
            this.TackleBlock = (RelativeCharacteristic)other.TackleBlock.Clone();
            this.TackleEvade = (RelativeCharacteristic)other.TackleEvade.Clone();
            this.TrapBonus = other.TrapBonus.Clone();
            this.TrapBonusPercent = other.TrapBonusPercent.Clone();
            this.Vitality = other.Vitality.Clone();
            this.WaterDamageBonus = other.WaterDamageBonus.Clone();
            this.WaterReduction = other.WaterReduction.Clone();
            this.WaterResistPercent = (ResistanceCharacteristic)other.WaterResistPercent.Clone();
            this.WeaponDamageDonePercent = other.WeaponDamageDonePercent.Clone();
            this.WeaponDamageResistancePercent = other.WeaponDamageResistancePercent.Clone();
            this.WeaponDamagesBonusPercent = other.WeaponDamagesBonusPercent.Clone();
            this.WeightBonus = other.WeightBonus.Clone();
            this.Wisdom = other.Wisdom.Clone();
            InvisibilityState = GameActionFightInvisibilityStateEnum.VISIBLE;
            this.BaseMaxLife = MaxLifePoints;
            this.Erosion = NaturalErosion;
            this.Initialize();
        }
        /*
         * Todo : Summoned / SummonerId
         */
        public FighterStats(MonsterGrade monsterGrade,double coeff = 1d)
        {
            this.ActionPoints = ApCharacteristic.New(monsterGrade.ActionPoints);
            this.AirDamageBonus = Characteristic.Zero();
            this.AirReduction = Characteristic.Zero();
            this.AirResistPercent = ResistanceCharacteristic.New(monsterGrade.AirResistance);
            this.AllDamagesBonus = Characteristic.Zero();


            this.CriticalDamageBonus = Characteristic.Zero();
            this.CriticalDamageReduction = Characteristic.Zero();
            this.CriticalHit = Characteristic.Zero();
            this.CriticalHitWeapon = 0;
            this.DamagesBonusPercent = Characteristic.Zero();

            this.DodgePAProbability = PointDodgeCharacteristic.New(monsterGrade.ApDodge);
            this.DodgePMProbability = PointDodgeCharacteristic.New(monsterGrade.MpDodge);
            this.EarthDamageBonus = Characteristic.Zero();
            this.EarthReduction = Characteristic.Zero();
            this.EarthResistPercent = ResistanceCharacteristic.New(monsterGrade.EarthResistance);
            this.Energy = 0;
            this.FireDamageBonus = Characteristic.Zero();
            this.FireReduction = Characteristic.Zero();
            this.FireResistPercent = ResistanceCharacteristic.New(monsterGrade.FireResistance);
            this.GlobalDamageReduction = 0;
            this.GlyphBonusPercent = Characteristic.Zero();
            this.HealBonus = Characteristic.Zero();
            this.Initiative = Characteristic.Zero();
            this.Intelligence = Characteristic.New(monsterGrade.Intelligence);
            this.Wisdom = Characteristic.New((short)(monsterGrade.Wisdom * coeff));
            this.Chance = Characteristic.New((short)(monsterGrade.Chance * coeff));
            this.Agility = Characteristic.New((short)(monsterGrade.Agility * coeff));
            this.Strength = Characteristic.New((short)(monsterGrade.Strength * coeff));
            this.Vitality = Characteristic.New((short)(monsterGrade.Vitality * coeff));
            this.MaxLifePoints = (int)(monsterGrade.LifePoints * coeff);
            this.MaxEnergyPoints = 0;

            this.MovementPoints = MpCharacteristic.New(monsterGrade.MovementPoints);
            this.NeutralDamageBonus = Characteristic.Zero();
            this.NeutralReduction = Characteristic.Zero();
            this.NeutralResistPercent = ResistanceCharacteristic.New(monsterGrade.NeutralResistance);
            this.APAttack = RelativeCharacteristic.Zero();
            this.PermanentDamagePercent = Characteristic.Zero();
            this.MPAttack = RelativeCharacteristic.Zero();
            this.Prospecting = RelativeCharacteristic.Zero();
            this.PushDamageBonus = Characteristic.Zero();
            this.PushDamageReduction = Characteristic.Zero();

            this.Range = RangeCharacteristic.Zero();

            this.Reflect = Characteristic.New(monsterGrade.DamageReflect);
            this.RuneBonusPercent = Characteristic.Zero();


            this.SummonableCreaturesBoost = Characteristic.New(BaseSummonsCount);
            this.TackleBlock = RelativeCharacteristic.Zero();
            this.TackleEvade = RelativeCharacteristic.Zero();
            this.TrapBonus = Characteristic.Zero();
            this.TrapBonusPercent = Characteristic.Zero();

            this.WaterDamageBonus = Characteristic.Zero();
            this.WaterReduction = Characteristic.Zero();
            this.WaterResistPercent = ResistanceCharacteristic.New(monsterGrade.WaterResistance);
            this.WeaponDamagesBonusPercent = Characteristic.Zero();

            this.MeleeDamageDonePercent = Characteristic.Zero();
            this.MeleeDamageResistancePercent = Characteristic.Zero();
            this.WeaponDamageDonePercent = Characteristic.Zero();
            this.WeaponDamageResistancePercent = Characteristic.Zero();
            this.RangedDamageDonePercent = Characteristic.Zero();
            this.RangedDamageResistancePercent = Characteristic.Zero();
            this.SpellDamageDonePercent = Characteristic.Zero();
            this.SpellDamageResistancePercent = Characteristic.Zero();
            this.WeightBonus = Characteristic.Zero();
            InvisibilityState = GameActionFightInvisibilityStateEnum.VISIBLE;
            this.BaseMaxLife = MaxLifePoints;
            this.LifePoints = MaxLifePoints;
            this.Erosion = NaturalErosion;
            this.Initialize();
        }

        public GameFightCharacteristics GetGameFightCharacteristics(Fighter owner, CharacterFighter target)
        {
            Fighter summoner = owner.GetSummoner();

            if (!owner.Fight.Started)
            {
                return new GameFightMinimalStatsPreparation()
                {
                    actionPoints = ActionPoints.TotalInContext(),
                    airElementReduction = AirReduction.TotalInContext(),
                    airElementResistPercent = AirResistPercent.TotalInContext(),
                    baseMaxLifePoints = BaseMaxLife,
                    criticalDamageFixedResist = CriticalDamageReduction.TotalInContext(),
                    dodgePALostProbability = DodgePAProbability.TotalInContext(),
                    dodgePMLostProbability = DodgePMProbability.TotalInContext(),
                    initiative = TotalInitiative,
                    maxActionPoints = ActionPoints.Total(),
                    earthElementReduction = EarthReduction.TotalInContext(),
                    earthElementResistPercent = EarthResistPercent.TotalInContext(),
                    fireElementReduction = FireReduction.TotalInContext(),
                    fireElementResistPercent = FireResistPercent.TotalInContext(),
                    fixedDamageReflection = Reflect.TotalInContext(),
                    invisibilityState = (byte)owner.GetInvisibilityStateFor(target),
                    lifePoints = LifePoints,
                    maxLifePoints = MaxLifePoints,
                    maxMovementPoints = MovementPoints.Total(),
                    movementPoints = MovementPoints.TotalInContext(),
                    neutralElementReduction = NeutralReduction.TotalInContext(),
                    neutralElementResistPercent = NeutralResistPercent.TotalInContext(),
                    permanentDamagePercent = PermanentDamagePercent.TotalInContext(),
                    pushDamageFixedResist = PushDamageReduction.TotalInContext(),
                    waterElementReduction = WaterReduction.TotalInContext(),
                    waterElementResistPercent = WaterResistPercent.TotalInContext(),
                    pvpEarthElementReduction = 0,
                    pvpEarthElementResistPercent = 0,
                    pvpFireElementReduction = 0,
                    pvpAirElementReduction = 0,
                    pvpNeutralElementReduction = 0,
                    pvpNeutralElementResistPercent = 0,
                    pvpWaterElementReduction = 0,
                    pvpWaterElementResistPercent = 0,
                    pvpAirElementResistPercent = 0,
                    pvpFireElementResistPercent = 0,
                    shieldPoints = ShieldPoints,
                    summoned = owner.IsSummoned(),
                    summoner = summoner != null ? summoner.Id : 0,
                    tackleBlock = TackleBlock.TotalInContext(),
                    tackleEvade = TackleEvade.TotalInContext(),
                    rangedDamageReceivedPercent = (short)(100 - RangedDamageResistancePercent.TotalInContext()),
                    meleeDamageReceivedPercent = (short)(100 - MeleeDamageResistancePercent.TotalInContext()),
                    spellDamageReceivedPercent = (short)(100 - SpellDamageResistancePercent.TotalInContext()),
                    weaponDamageReceivedPercent = (short)(100 - WeaponDamageResistancePercent.TotalInContext()),
                };
            }
            else
            {
                return new GameFightMinimalStats()
                {
                    actionPoints = ActionPoints.TotalInContext(),
                    airElementReduction = AirReduction.TotalInContext(),
                    airElementResistPercent = AirResistPercent.TotalInContext(),
                    baseMaxLifePoints = BaseMaxLife,
                    criticalDamageFixedResist = CriticalDamageReduction.TotalInContext(),
                    dodgePALostProbability = DodgePAProbability.TotalInContext(),
                    dodgePMLostProbability = DodgePMProbability.TotalInContext(),
                    maxActionPoints = ActionPoints.Total(),
                    earthElementReduction = EarthReduction.TotalInContext(),
                    earthElementResistPercent = EarthResistPercent.TotalInContext(),
                    fireElementReduction = FireReduction.TotalInContext(),
                    fireElementResistPercent = FireResistPercent.TotalInContext(),
                    fixedDamageReflection = Reflect.TotalInContext(),
                    invisibilityState = (byte)owner.GetInvisibilityStateFor(target),
                    lifePoints = LifePoints,
                    maxLifePoints = MaxLifePoints,
                    shieldPoints = ShieldPoints,
                    maxMovementPoints = MovementPoints.Total(),
                    movementPoints = MovementPoints.TotalInContext(),
                    neutralElementReduction = NeutralReduction.TotalInContext(),
                    neutralElementResistPercent = NeutralResistPercent.TotalInContext(),
                    permanentDamagePercent = PermanentDamagePercent.TotalInContext(),
                    pushDamageFixedResist = PushDamageReduction.TotalInContext(),
                    pvpEarthElementReduction = 0,
                    pvpEarthElementResistPercent = 0,
                    pvpFireElementReduction = 0,
                    pvpFireElementResistPercent = 0,
                    pvpNeutralElementReduction = 0,
                    pvpAirElementResistPercent = 0,
                    pvpAirElementReduction = 0,
                    pvpWaterElementReduction = 0,
                    pvpNeutralElementResistPercent = 0,
                    pvpWaterElementResistPercent = 0,
                    rangedDamageReceivedPercent = (short)(100 - RangedDamageResistancePercent.TotalInContext()),
                    meleeDamageReceivedPercent = (short)(100 - MeleeDamageResistancePercent.TotalInContext()),
                    spellDamageReceivedPercent = (short)(100 - SpellDamageResistancePercent.TotalInContext()),
                    weaponDamageReceivedPercent = (short)(100 - WeaponDamageResistancePercent.TotalInContext()),
                    summoned = owner.IsSummoned(),
                    summoner = summoner != null ? summoner.Id : 0,
                    tackleBlock = TackleBlock.TotalInContext(),
                    tackleEvade = TackleEvade.TotalInContext(),
                    waterElementReduction = WaterReduction.TotalInContext(),
                    waterElementResistPercent = WaterResistPercent.TotalInContext(),
                };
            }
        }

    }
}
