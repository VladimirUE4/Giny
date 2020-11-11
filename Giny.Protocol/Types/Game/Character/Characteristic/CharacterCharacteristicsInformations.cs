using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterCharacteristicsInformations  
    { 
        public const ushort Id = 1165;
        public virtual ushort TypeId => Id;

        public long experience;
        public long experienceLevelFloor;
        public long experienceNextLevelFloor;
        public long experienceBonusLimit;
        public long kamas;
        public short statsPoints;
        public short additionnalPoints;
        public short spellsPoints;
        public ActorExtendedAlignmentInformations alignmentInfos;
        public int lifePoints;
        public int maxLifePoints;
        public short energyPoints;
        public short maxEnergyPoints;
        public short actionPointsCurrent;
        public short movementPointsCurrent;
        public CharacterBaseCharacteristic initiative;
        public CharacterBaseCharacteristic prospecting;
        public CharacterBaseCharacteristic actionPoints;
        public CharacterBaseCharacteristic movementPoints;
        public CharacterBaseCharacteristic strength;
        public CharacterBaseCharacteristic vitality;
        public CharacterBaseCharacteristic wisdom;
        public CharacterBaseCharacteristic chance;
        public CharacterBaseCharacteristic agility;
        public CharacterBaseCharacteristic intelligence;
        public CharacterBaseCharacteristic range;
        public CharacterBaseCharacteristic summonableCreaturesBoost;
        public CharacterBaseCharacteristic reflect;
        public CharacterBaseCharacteristic criticalHit;
        public short criticalHitWeapon;
        public CharacterBaseCharacteristic criticalMiss;
        public CharacterBaseCharacteristic healBonus;
        public CharacterBaseCharacteristic allDamagesBonus;
        public CharacterBaseCharacteristic weaponDamagesBonusPercent;
        public CharacterBaseCharacteristic damagesBonusPercent;
        public CharacterBaseCharacteristic trapBonus;
        public CharacterBaseCharacteristic trapBonusPercent;
        public CharacterBaseCharacteristic glyphBonusPercent;
        public CharacterBaseCharacteristic runeBonusPercent;
        public CharacterBaseCharacteristic permanentDamagePercent;
        public CharacterBaseCharacteristic tackleBlock;
        public CharacterBaseCharacteristic tackleEvade;
        public CharacterBaseCharacteristic PAAttack;
        public CharacterBaseCharacteristic PMAttack;
        public CharacterBaseCharacteristic pushDamageBonus;
        public CharacterBaseCharacteristic criticalDamageBonus;
        public CharacterBaseCharacteristic neutralDamageBonus;
        public CharacterBaseCharacteristic earthDamageBonus;
        public CharacterBaseCharacteristic waterDamageBonus;
        public CharacterBaseCharacteristic airDamageBonus;
        public CharacterBaseCharacteristic fireDamageBonus;
        public CharacterBaseCharacteristic dodgePALostProbability;
        public CharacterBaseCharacteristic dodgePMLostProbability;
        public CharacterBaseCharacteristic neutralElementResistPercent;
        public CharacterBaseCharacteristic earthElementResistPercent;
        public CharacterBaseCharacteristic waterElementResistPercent;
        public CharacterBaseCharacteristic airElementResistPercent;
        public CharacterBaseCharacteristic fireElementResistPercent;
        public CharacterBaseCharacteristic neutralElementReduction;
        public CharacterBaseCharacteristic earthElementReduction;
        public CharacterBaseCharacteristic waterElementReduction;
        public CharacterBaseCharacteristic airElementReduction;
        public CharacterBaseCharacteristic fireElementReduction;
        public CharacterBaseCharacteristic pushDamageReduction;
        public CharacterBaseCharacteristic criticalDamageReduction;
        public CharacterBaseCharacteristic pvpNeutralElementResistPercent;
        public CharacterBaseCharacteristic pvpEarthElementResistPercent;
        public CharacterBaseCharacteristic pvpWaterElementResistPercent;
        public CharacterBaseCharacteristic pvpAirElementResistPercent;
        public CharacterBaseCharacteristic pvpFireElementResistPercent;
        public CharacterBaseCharacteristic pvpNeutralElementReduction;
        public CharacterBaseCharacteristic pvpEarthElementReduction;
        public CharacterBaseCharacteristic pvpWaterElementReduction;
        public CharacterBaseCharacteristic pvpAirElementReduction;
        public CharacterBaseCharacteristic pvpFireElementReduction;
        public CharacterBaseCharacteristic meleeDamageDonePercent;
        public CharacterBaseCharacteristic meleeDamageReceivedPercent;
        public CharacterBaseCharacteristic rangedDamageDonePercent;
        public CharacterBaseCharacteristic rangedDamageReceivedPercent;
        public CharacterBaseCharacteristic weaponDamageDonePercent;
        public CharacterBaseCharacteristic weaponDamageReceivedPercent;
        public CharacterBaseCharacteristic spellDamageDonePercent;
        public CharacterBaseCharacteristic spellDamageReceivedPercent;
        public CharacterSpellModification[] spellModifications;
        public int probationTime;

        public CharacterCharacteristicsInformations()
        {
        }
        public CharacterCharacteristicsInformations(long experience,long experienceLevelFloor,long experienceNextLevelFloor,long experienceBonusLimit,long kamas,short statsPoints,short additionnalPoints,short spellsPoints,ActorExtendedAlignmentInformations alignmentInfos,int lifePoints,int maxLifePoints,short energyPoints,short maxEnergyPoints,short actionPointsCurrent,short movementPointsCurrent,CharacterBaseCharacteristic initiative,CharacterBaseCharacteristic prospecting,CharacterBaseCharacteristic actionPoints,CharacterBaseCharacteristic movementPoints,CharacterBaseCharacteristic strength,CharacterBaseCharacteristic vitality,CharacterBaseCharacteristic wisdom,CharacterBaseCharacteristic chance,CharacterBaseCharacteristic agility,CharacterBaseCharacteristic intelligence,CharacterBaseCharacteristic range,CharacterBaseCharacteristic summonableCreaturesBoost,CharacterBaseCharacteristic reflect,CharacterBaseCharacteristic criticalHit,short criticalHitWeapon,CharacterBaseCharacteristic criticalMiss,CharacterBaseCharacteristic healBonus,CharacterBaseCharacteristic allDamagesBonus,CharacterBaseCharacteristic weaponDamagesBonusPercent,CharacterBaseCharacteristic damagesBonusPercent,CharacterBaseCharacteristic trapBonus,CharacterBaseCharacteristic trapBonusPercent,CharacterBaseCharacteristic glyphBonusPercent,CharacterBaseCharacteristic runeBonusPercent,CharacterBaseCharacteristic permanentDamagePercent,CharacterBaseCharacteristic tackleBlock,CharacterBaseCharacteristic tackleEvade,CharacterBaseCharacteristic PAAttack,CharacterBaseCharacteristic PMAttack,CharacterBaseCharacteristic pushDamageBonus,CharacterBaseCharacteristic criticalDamageBonus,CharacterBaseCharacteristic neutralDamageBonus,CharacterBaseCharacteristic earthDamageBonus,CharacterBaseCharacteristic waterDamageBonus,CharacterBaseCharacteristic airDamageBonus,CharacterBaseCharacteristic fireDamageBonus,CharacterBaseCharacteristic dodgePALostProbability,CharacterBaseCharacteristic dodgePMLostProbability,CharacterBaseCharacteristic neutralElementResistPercent,CharacterBaseCharacteristic earthElementResistPercent,CharacterBaseCharacteristic waterElementResistPercent,CharacterBaseCharacteristic airElementResistPercent,CharacterBaseCharacteristic fireElementResistPercent,CharacterBaseCharacteristic neutralElementReduction,CharacterBaseCharacteristic earthElementReduction,CharacterBaseCharacteristic waterElementReduction,CharacterBaseCharacteristic airElementReduction,CharacterBaseCharacteristic fireElementReduction,CharacterBaseCharacteristic pushDamageReduction,CharacterBaseCharacteristic criticalDamageReduction,CharacterBaseCharacteristic pvpNeutralElementResistPercent,CharacterBaseCharacteristic pvpEarthElementResistPercent,CharacterBaseCharacteristic pvpWaterElementResistPercent,CharacterBaseCharacteristic pvpAirElementResistPercent,CharacterBaseCharacteristic pvpFireElementResistPercent,CharacterBaseCharacteristic pvpNeutralElementReduction,CharacterBaseCharacteristic pvpEarthElementReduction,CharacterBaseCharacteristic pvpWaterElementReduction,CharacterBaseCharacteristic pvpAirElementReduction,CharacterBaseCharacteristic pvpFireElementReduction,CharacterBaseCharacteristic meleeDamageDonePercent,CharacterBaseCharacteristic meleeDamageReceivedPercent,CharacterBaseCharacteristic rangedDamageDonePercent,CharacterBaseCharacteristic rangedDamageReceivedPercent,CharacterBaseCharacteristic weaponDamageDonePercent,CharacterBaseCharacteristic weaponDamageReceivedPercent,CharacterBaseCharacteristic spellDamageDonePercent,CharacterBaseCharacteristic spellDamageReceivedPercent,CharacterSpellModification[] spellModifications,int probationTime)
        {
            this.experience = experience;
            this.experienceLevelFloor = experienceLevelFloor;
            this.experienceNextLevelFloor = experienceNextLevelFloor;
            this.experienceBonusLimit = experienceBonusLimit;
            this.kamas = kamas;
            this.statsPoints = statsPoints;
            this.additionnalPoints = additionnalPoints;
            this.spellsPoints = spellsPoints;
            this.alignmentInfos = alignmentInfos;
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.energyPoints = energyPoints;
            this.maxEnergyPoints = maxEnergyPoints;
            this.actionPointsCurrent = actionPointsCurrent;
            this.movementPointsCurrent = movementPointsCurrent;
            this.initiative = initiative;
            this.prospecting = prospecting;
            this.actionPoints = actionPoints;
            this.movementPoints = movementPoints;
            this.strength = strength;
            this.vitality = vitality;
            this.wisdom = wisdom;
            this.chance = chance;
            this.agility = agility;
            this.intelligence = intelligence;
            this.range = range;
            this.summonableCreaturesBoost = summonableCreaturesBoost;
            this.reflect = reflect;
            this.criticalHit = criticalHit;
            this.criticalHitWeapon = criticalHitWeapon;
            this.criticalMiss = criticalMiss;
            this.healBonus = healBonus;
            this.allDamagesBonus = allDamagesBonus;
            this.weaponDamagesBonusPercent = weaponDamagesBonusPercent;
            this.damagesBonusPercent = damagesBonusPercent;
            this.trapBonus = trapBonus;
            this.trapBonusPercent = trapBonusPercent;
            this.glyphBonusPercent = glyphBonusPercent;
            this.runeBonusPercent = runeBonusPercent;
            this.permanentDamagePercent = permanentDamagePercent;
            this.tackleBlock = tackleBlock;
            this.tackleEvade = tackleEvade;
            this.PAAttack = PAAttack;
            this.PMAttack = PMAttack;
            this.pushDamageBonus = pushDamageBonus;
            this.criticalDamageBonus = criticalDamageBonus;
            this.neutralDamageBonus = neutralDamageBonus;
            this.earthDamageBonus = earthDamageBonus;
            this.waterDamageBonus = waterDamageBonus;
            this.airDamageBonus = airDamageBonus;
            this.fireDamageBonus = fireDamageBonus;
            this.dodgePALostProbability = dodgePALostProbability;
            this.dodgePMLostProbability = dodgePMLostProbability;
            this.neutralElementResistPercent = neutralElementResistPercent;
            this.earthElementResistPercent = earthElementResistPercent;
            this.waterElementResistPercent = waterElementResistPercent;
            this.airElementResistPercent = airElementResistPercent;
            this.fireElementResistPercent = fireElementResistPercent;
            this.neutralElementReduction = neutralElementReduction;
            this.earthElementReduction = earthElementReduction;
            this.waterElementReduction = waterElementReduction;
            this.airElementReduction = airElementReduction;
            this.fireElementReduction = fireElementReduction;
            this.pushDamageReduction = pushDamageReduction;
            this.criticalDamageReduction = criticalDamageReduction;
            this.pvpNeutralElementResistPercent = pvpNeutralElementResistPercent;
            this.pvpEarthElementResistPercent = pvpEarthElementResistPercent;
            this.pvpWaterElementResistPercent = pvpWaterElementResistPercent;
            this.pvpAirElementResistPercent = pvpAirElementResistPercent;
            this.pvpFireElementResistPercent = pvpFireElementResistPercent;
            this.pvpNeutralElementReduction = pvpNeutralElementReduction;
            this.pvpEarthElementReduction = pvpEarthElementReduction;
            this.pvpWaterElementReduction = pvpWaterElementReduction;
            this.pvpAirElementReduction = pvpAirElementReduction;
            this.pvpFireElementReduction = pvpFireElementReduction;
            this.meleeDamageDonePercent = meleeDamageDonePercent;
            this.meleeDamageReceivedPercent = meleeDamageReceivedPercent;
            this.rangedDamageDonePercent = rangedDamageDonePercent;
            this.rangedDamageReceivedPercent = rangedDamageReceivedPercent;
            this.weaponDamageDonePercent = weaponDamageDonePercent;
            this.weaponDamageReceivedPercent = weaponDamageReceivedPercent;
            this.spellDamageDonePercent = spellDamageDonePercent;
            this.spellDamageReceivedPercent = spellDamageReceivedPercent;
            this.spellModifications = spellModifications;
            this.probationTime = probationTime;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            if (experience < 0 || experience > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + experience + ") on element experience.");
            }

            writer.WriteVarLong((long)experience);
            if (experienceLevelFloor < 0 || experienceLevelFloor > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + experienceLevelFloor + ") on element experienceLevelFloor.");
            }

            writer.WriteVarLong((long)experienceLevelFloor);
            if (experienceNextLevelFloor < 0 || experienceNextLevelFloor > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + experienceNextLevelFloor + ") on element experienceNextLevelFloor.");
            }

            writer.WriteVarLong((long)experienceNextLevelFloor);
            if (experienceBonusLimit < 0 || experienceBonusLimit > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + experienceBonusLimit + ") on element experienceBonusLimit.");
            }

            writer.WriteVarLong((long)experienceBonusLimit);
            if (kamas < 0 || kamas > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + kamas + ") on element kamas.");
            }

            writer.WriteVarLong((long)kamas);
            if (statsPoints < 0)
            {
                throw new Exception("Forbidden value (" + statsPoints + ") on element statsPoints.");
            }

            writer.WriteVarShort((short)statsPoints);
            if (additionnalPoints < 0)
            {
                throw new Exception("Forbidden value (" + additionnalPoints + ") on element additionnalPoints.");
            }

            writer.WriteVarShort((short)additionnalPoints);
            if (spellsPoints < 0)
            {
                throw new Exception("Forbidden value (" + spellsPoints + ") on element spellsPoints.");
            }

            writer.WriteVarShort((short)spellsPoints);
            alignmentInfos.Serialize(writer);
            if (lifePoints < 0)
            {
                throw new Exception("Forbidden value (" + lifePoints + ") on element lifePoints.");
            }

            writer.WriteVarInt((int)lifePoints);
            if (maxLifePoints < 0)
            {
                throw new Exception("Forbidden value (" + maxLifePoints + ") on element maxLifePoints.");
            }

            writer.WriteVarInt((int)maxLifePoints);
            if (energyPoints < 0)
            {
                throw new Exception("Forbidden value (" + energyPoints + ") on element energyPoints.");
            }

            writer.WriteVarShort((short)energyPoints);
            if (maxEnergyPoints < 0)
            {
                throw new Exception("Forbidden value (" + maxEnergyPoints + ") on element maxEnergyPoints.");
            }

            writer.WriteVarShort((short)maxEnergyPoints);
            writer.WriteVarShort((short)actionPointsCurrent);
            writer.WriteVarShort((short)movementPointsCurrent);
            initiative.Serialize(writer);
            prospecting.Serialize(writer);
            actionPoints.Serialize(writer);
            movementPoints.Serialize(writer);
            strength.Serialize(writer);
            vitality.Serialize(writer);
            wisdom.Serialize(writer);
            chance.Serialize(writer);
            agility.Serialize(writer);
            intelligence.Serialize(writer);
            range.Serialize(writer);
            summonableCreaturesBoost.Serialize(writer);
            reflect.Serialize(writer);
            criticalHit.Serialize(writer);
            if (criticalHitWeapon < 0)
            {
                throw new Exception("Forbidden value (" + criticalHitWeapon + ") on element criticalHitWeapon.");
            }

            writer.WriteVarShort((short)criticalHitWeapon);
            criticalMiss.Serialize(writer);
            healBonus.Serialize(writer);
            allDamagesBonus.Serialize(writer);
            weaponDamagesBonusPercent.Serialize(writer);
            damagesBonusPercent.Serialize(writer);
            trapBonus.Serialize(writer);
            trapBonusPercent.Serialize(writer);
            glyphBonusPercent.Serialize(writer);
            runeBonusPercent.Serialize(writer);
            permanentDamagePercent.Serialize(writer);
            tackleBlock.Serialize(writer);
            tackleEvade.Serialize(writer);
            PAAttack.Serialize(writer);
            PMAttack.Serialize(writer);
            pushDamageBonus.Serialize(writer);
            criticalDamageBonus.Serialize(writer);
            neutralDamageBonus.Serialize(writer);
            earthDamageBonus.Serialize(writer);
            waterDamageBonus.Serialize(writer);
            airDamageBonus.Serialize(writer);
            fireDamageBonus.Serialize(writer);
            dodgePALostProbability.Serialize(writer);
            dodgePMLostProbability.Serialize(writer);
            neutralElementResistPercent.Serialize(writer);
            earthElementResistPercent.Serialize(writer);
            waterElementResistPercent.Serialize(writer);
            airElementResistPercent.Serialize(writer);
            fireElementResistPercent.Serialize(writer);
            neutralElementReduction.Serialize(writer);
            earthElementReduction.Serialize(writer);
            waterElementReduction.Serialize(writer);
            airElementReduction.Serialize(writer);
            fireElementReduction.Serialize(writer);
            pushDamageReduction.Serialize(writer);
            criticalDamageReduction.Serialize(writer);
            pvpNeutralElementResistPercent.Serialize(writer);
            pvpEarthElementResistPercent.Serialize(writer);
            pvpWaterElementResistPercent.Serialize(writer);
            pvpAirElementResistPercent.Serialize(writer);
            pvpFireElementResistPercent.Serialize(writer);
            pvpNeutralElementReduction.Serialize(writer);
            pvpEarthElementReduction.Serialize(writer);
            pvpWaterElementReduction.Serialize(writer);
            pvpAirElementReduction.Serialize(writer);
            pvpFireElementReduction.Serialize(writer);
            meleeDamageDonePercent.Serialize(writer);
            meleeDamageReceivedPercent.Serialize(writer);
            rangedDamageDonePercent.Serialize(writer);
            rangedDamageReceivedPercent.Serialize(writer);
            weaponDamageDonePercent.Serialize(writer);
            weaponDamageReceivedPercent.Serialize(writer);
            spellDamageDonePercent.Serialize(writer);
            spellDamageReceivedPercent.Serialize(writer);
            writer.WriteShort((short)spellModifications.Length);
            for (uint _i84 = 0;_i84 < spellModifications.Length;_i84++)
            {
                (spellModifications[_i84] as CharacterSpellModification).Serialize(writer);
            }

            if (probationTime < 0)
            {
                throw new Exception("Forbidden value (" + probationTime + ") on element probationTime.");
            }

            writer.WriteInt((int)probationTime);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            CharacterSpellModification _item84 = null;
            experience = (long)reader.ReadVarUhLong();
            if (experience < 0 || experience > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + experience + ") on element of CharacterCharacteristicsInformations.experience.");
            }

            experienceLevelFloor = (long)reader.ReadVarUhLong();
            if (experienceLevelFloor < 0 || experienceLevelFloor > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + experienceLevelFloor + ") on element of CharacterCharacteristicsInformations.experienceLevelFloor.");
            }

            experienceNextLevelFloor = (long)reader.ReadVarUhLong();
            if (experienceNextLevelFloor < 0 || experienceNextLevelFloor > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + experienceNextLevelFloor + ") on element of CharacterCharacteristicsInformations.experienceNextLevelFloor.");
            }

            experienceBonusLimit = (long)reader.ReadVarUhLong();
            if (experienceBonusLimit < 0 || experienceBonusLimit > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + experienceBonusLimit + ") on element of CharacterCharacteristicsInformations.experienceBonusLimit.");
            }

            kamas = (long)reader.ReadVarUhLong();
            if (kamas < 0 || kamas > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + kamas + ") on element of CharacterCharacteristicsInformations.kamas.");
            }

            statsPoints = (short)reader.ReadVarUhShort();
            if (statsPoints < 0)
            {
                throw new Exception("Forbidden value (" + statsPoints + ") on element of CharacterCharacteristicsInformations.statsPoints.");
            }

            additionnalPoints = (short)reader.ReadVarUhShort();
            if (additionnalPoints < 0)
            {
                throw new Exception("Forbidden value (" + additionnalPoints + ") on element of CharacterCharacteristicsInformations.additionnalPoints.");
            }

            spellsPoints = (short)reader.ReadVarUhShort();
            if (spellsPoints < 0)
            {
                throw new Exception("Forbidden value (" + spellsPoints + ") on element of CharacterCharacteristicsInformations.spellsPoints.");
            }

            alignmentInfos = new ActorExtendedAlignmentInformations();
            alignmentInfos.Deserialize(reader);
            lifePoints = (int)reader.ReadVarUhInt();
            if (lifePoints < 0)
            {
                throw new Exception("Forbidden value (" + lifePoints + ") on element of CharacterCharacteristicsInformations.lifePoints.");
            }

            maxLifePoints = (int)reader.ReadVarUhInt();
            if (maxLifePoints < 0)
            {
                throw new Exception("Forbidden value (" + maxLifePoints + ") on element of CharacterCharacteristicsInformations.maxLifePoints.");
            }

            energyPoints = (short)reader.ReadVarUhShort();
            if (energyPoints < 0)
            {
                throw new Exception("Forbidden value (" + energyPoints + ") on element of CharacterCharacteristicsInformations.energyPoints.");
            }

            maxEnergyPoints = (short)reader.ReadVarUhShort();
            if (maxEnergyPoints < 0)
            {
                throw new Exception("Forbidden value (" + maxEnergyPoints + ") on element of CharacterCharacteristicsInformations.maxEnergyPoints.");
            }

            actionPointsCurrent = (short)reader.ReadVarShort();
            movementPointsCurrent = (short)reader.ReadVarShort();
            initiative = new CharacterBaseCharacteristic();
            initiative.Deserialize(reader);
            prospecting = new CharacterBaseCharacteristic();
            prospecting.Deserialize(reader);
            actionPoints = new CharacterBaseCharacteristic();
            actionPoints.Deserialize(reader);
            movementPoints = new CharacterBaseCharacteristic();
            movementPoints.Deserialize(reader);
            strength = new CharacterBaseCharacteristic();
            strength.Deserialize(reader);
            vitality = new CharacterBaseCharacteristic();
            vitality.Deserialize(reader);
            wisdom = new CharacterBaseCharacteristic();
            wisdom.Deserialize(reader);
            chance = new CharacterBaseCharacteristic();
            chance.Deserialize(reader);
            agility = new CharacterBaseCharacteristic();
            agility.Deserialize(reader);
            intelligence = new CharacterBaseCharacteristic();
            intelligence.Deserialize(reader);
            range = new CharacterBaseCharacteristic();
            range.Deserialize(reader);
            summonableCreaturesBoost = new CharacterBaseCharacteristic();
            summonableCreaturesBoost.Deserialize(reader);
            reflect = new CharacterBaseCharacteristic();
            reflect.Deserialize(reader);
            criticalHit = new CharacterBaseCharacteristic();
            criticalHit.Deserialize(reader);
            criticalHitWeapon = (short)reader.ReadVarUhShort();
            if (criticalHitWeapon < 0)
            {
                throw new Exception("Forbidden value (" + criticalHitWeapon + ") on element of CharacterCharacteristicsInformations.criticalHitWeapon.");
            }

            criticalMiss = new CharacterBaseCharacteristic();
            criticalMiss.Deserialize(reader);
            healBonus = new CharacterBaseCharacteristic();
            healBonus.Deserialize(reader);
            allDamagesBonus = new CharacterBaseCharacteristic();
            allDamagesBonus.Deserialize(reader);
            weaponDamagesBonusPercent = new CharacterBaseCharacteristic();
            weaponDamagesBonusPercent.Deserialize(reader);
            damagesBonusPercent = new CharacterBaseCharacteristic();
            damagesBonusPercent.Deserialize(reader);
            trapBonus = new CharacterBaseCharacteristic();
            trapBonus.Deserialize(reader);
            trapBonusPercent = new CharacterBaseCharacteristic();
            trapBonusPercent.Deserialize(reader);
            glyphBonusPercent = new CharacterBaseCharacteristic();
            glyphBonusPercent.Deserialize(reader);
            runeBonusPercent = new CharacterBaseCharacteristic();
            runeBonusPercent.Deserialize(reader);
            permanentDamagePercent = new CharacterBaseCharacteristic();
            permanentDamagePercent.Deserialize(reader);
            tackleBlock = new CharacterBaseCharacteristic();
            tackleBlock.Deserialize(reader);
            tackleEvade = new CharacterBaseCharacteristic();
            tackleEvade.Deserialize(reader);
            PAAttack = new CharacterBaseCharacteristic();
            PAAttack.Deserialize(reader);
            PMAttack = new CharacterBaseCharacteristic();
            PMAttack.Deserialize(reader);
            pushDamageBonus = new CharacterBaseCharacteristic();
            pushDamageBonus.Deserialize(reader);
            criticalDamageBonus = new CharacterBaseCharacteristic();
            criticalDamageBonus.Deserialize(reader);
            neutralDamageBonus = new CharacterBaseCharacteristic();
            neutralDamageBonus.Deserialize(reader);
            earthDamageBonus = new CharacterBaseCharacteristic();
            earthDamageBonus.Deserialize(reader);
            waterDamageBonus = new CharacterBaseCharacteristic();
            waterDamageBonus.Deserialize(reader);
            airDamageBonus = new CharacterBaseCharacteristic();
            airDamageBonus.Deserialize(reader);
            fireDamageBonus = new CharacterBaseCharacteristic();
            fireDamageBonus.Deserialize(reader);
            dodgePALostProbability = new CharacterBaseCharacteristic();
            dodgePALostProbability.Deserialize(reader);
            dodgePMLostProbability = new CharacterBaseCharacteristic();
            dodgePMLostProbability.Deserialize(reader);
            neutralElementResistPercent = new CharacterBaseCharacteristic();
            neutralElementResistPercent.Deserialize(reader);
            earthElementResistPercent = new CharacterBaseCharacteristic();
            earthElementResistPercent.Deserialize(reader);
            waterElementResistPercent = new CharacterBaseCharacteristic();
            waterElementResistPercent.Deserialize(reader);
            airElementResistPercent = new CharacterBaseCharacteristic();
            airElementResistPercent.Deserialize(reader);
            fireElementResistPercent = new CharacterBaseCharacteristic();
            fireElementResistPercent.Deserialize(reader);
            neutralElementReduction = new CharacterBaseCharacteristic();
            neutralElementReduction.Deserialize(reader);
            earthElementReduction = new CharacterBaseCharacteristic();
            earthElementReduction.Deserialize(reader);
            waterElementReduction = new CharacterBaseCharacteristic();
            waterElementReduction.Deserialize(reader);
            airElementReduction = new CharacterBaseCharacteristic();
            airElementReduction.Deserialize(reader);
            fireElementReduction = new CharacterBaseCharacteristic();
            fireElementReduction.Deserialize(reader);
            pushDamageReduction = new CharacterBaseCharacteristic();
            pushDamageReduction.Deserialize(reader);
            criticalDamageReduction = new CharacterBaseCharacteristic();
            criticalDamageReduction.Deserialize(reader);
            pvpNeutralElementResistPercent = new CharacterBaseCharacteristic();
            pvpNeutralElementResistPercent.Deserialize(reader);
            pvpEarthElementResistPercent = new CharacterBaseCharacteristic();
            pvpEarthElementResistPercent.Deserialize(reader);
            pvpWaterElementResistPercent = new CharacterBaseCharacteristic();
            pvpWaterElementResistPercent.Deserialize(reader);
            pvpAirElementResistPercent = new CharacterBaseCharacteristic();
            pvpAirElementResistPercent.Deserialize(reader);
            pvpFireElementResistPercent = new CharacterBaseCharacteristic();
            pvpFireElementResistPercent.Deserialize(reader);
            pvpNeutralElementReduction = new CharacterBaseCharacteristic();
            pvpNeutralElementReduction.Deserialize(reader);
            pvpEarthElementReduction = new CharacterBaseCharacteristic();
            pvpEarthElementReduction.Deserialize(reader);
            pvpWaterElementReduction = new CharacterBaseCharacteristic();
            pvpWaterElementReduction.Deserialize(reader);
            pvpAirElementReduction = new CharacterBaseCharacteristic();
            pvpAirElementReduction.Deserialize(reader);
            pvpFireElementReduction = new CharacterBaseCharacteristic();
            pvpFireElementReduction.Deserialize(reader);
            meleeDamageDonePercent = new CharacterBaseCharacteristic();
            meleeDamageDonePercent.Deserialize(reader);
            meleeDamageReceivedPercent = new CharacterBaseCharacteristic();
            meleeDamageReceivedPercent.Deserialize(reader);
            rangedDamageDonePercent = new CharacterBaseCharacteristic();
            rangedDamageDonePercent.Deserialize(reader);
            rangedDamageReceivedPercent = new CharacterBaseCharacteristic();
            rangedDamageReceivedPercent.Deserialize(reader);
            weaponDamageDonePercent = new CharacterBaseCharacteristic();
            weaponDamageDonePercent.Deserialize(reader);
            weaponDamageReceivedPercent = new CharacterBaseCharacteristic();
            weaponDamageReceivedPercent.Deserialize(reader);
            spellDamageDonePercent = new CharacterBaseCharacteristic();
            spellDamageDonePercent.Deserialize(reader);
            spellDamageReceivedPercent = new CharacterBaseCharacteristic();
            spellDamageReceivedPercent.Deserialize(reader);
            uint _spellModificationsLen = (uint)reader.ReadUShort();
            for (uint _i84 = 0;_i84 < _spellModificationsLen;_i84++)
            {
                _item84 = new CharacterSpellModification();
                _item84.Deserialize(reader);
                spellModifications[_i84] = _item84;
            }

            probationTime = (int)reader.ReadInt();
            if (probationTime < 0)
            {
                throw new Exception("Forbidden value (" + probationTime + ") on element of CharacterCharacteristicsInformations.probationTime.");
            }

        }


    }
}








