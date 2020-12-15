using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightMinimalStats  
    { 
        public const ushort Id = 7930;
        public virtual ushort TypeId => Id;

        public int lifePoints;
        public int maxLifePoints;
        public int baseMaxLifePoints;
        public int permanentDamagePercent;
        public int shieldPoints;
        public short actionPoints;
        public short maxActionPoints;
        public short movementPoints;
        public short maxMovementPoints;
        public double summoner;
        public bool summoned;
        public short neutralElementResistPercent;
        public short earthElementResistPercent;
        public short waterElementResistPercent;
        public short airElementResistPercent;
        public short fireElementResistPercent;
        public short neutralElementReduction;
        public short earthElementReduction;
        public short waterElementReduction;
        public short airElementReduction;
        public short fireElementReduction;
        public short criticalDamageFixedResist;
        public short pushDamageFixedResist;
        public short pvpNeutralElementResistPercent;
        public short pvpEarthElementResistPercent;
        public short pvpWaterElementResistPercent;
        public short pvpAirElementResistPercent;
        public short pvpFireElementResistPercent;
        public short pvpNeutralElementReduction;
        public short pvpEarthElementReduction;
        public short pvpWaterElementReduction;
        public short pvpAirElementReduction;
        public short pvpFireElementReduction;
        public short dodgePALostProbability;
        public short dodgePMLostProbability;
        public short tackleBlock;
        public short tackleEvade;
        public short fixedDamageReflection;
        public byte invisibilityState;
        public short meleeDamageReceivedPercent;
        public short rangedDamageReceivedPercent;
        public short weaponDamageReceivedPercent;
        public short spellDamageReceivedPercent;

        public GameFightMinimalStats()
        {
        }
        public GameFightMinimalStats(int lifePoints,int maxLifePoints,int baseMaxLifePoints,int permanentDamagePercent,int shieldPoints,short actionPoints,short maxActionPoints,short movementPoints,short maxMovementPoints,double summoner,bool summoned,short neutralElementResistPercent,short earthElementResistPercent,short waterElementResistPercent,short airElementResistPercent,short fireElementResistPercent,short neutralElementReduction,short earthElementReduction,short waterElementReduction,short airElementReduction,short fireElementReduction,short criticalDamageFixedResist,short pushDamageFixedResist,short pvpNeutralElementResistPercent,short pvpEarthElementResistPercent,short pvpWaterElementResistPercent,short pvpAirElementResistPercent,short pvpFireElementResistPercent,short pvpNeutralElementReduction,short pvpEarthElementReduction,short pvpWaterElementReduction,short pvpAirElementReduction,short pvpFireElementReduction,short dodgePALostProbability,short dodgePMLostProbability,short tackleBlock,short tackleEvade,short fixedDamageReflection,byte invisibilityState,short meleeDamageReceivedPercent,short rangedDamageReceivedPercent,short weaponDamageReceivedPercent,short spellDamageReceivedPercent)
        {
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.baseMaxLifePoints = baseMaxLifePoints;
            this.permanentDamagePercent = permanentDamagePercent;
            this.shieldPoints = shieldPoints;
            this.actionPoints = actionPoints;
            this.maxActionPoints = maxActionPoints;
            this.movementPoints = movementPoints;
            this.maxMovementPoints = maxMovementPoints;
            this.summoner = summoner;
            this.summoned = summoned;
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
            this.criticalDamageFixedResist = criticalDamageFixedResist;
            this.pushDamageFixedResist = pushDamageFixedResist;
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
            this.dodgePALostProbability = dodgePALostProbability;
            this.dodgePMLostProbability = dodgePMLostProbability;
            this.tackleBlock = tackleBlock;
            this.tackleEvade = tackleEvade;
            this.fixedDamageReflection = fixedDamageReflection;
            this.invisibilityState = invisibilityState;
            this.meleeDamageReceivedPercent = meleeDamageReceivedPercent;
            this.rangedDamageReceivedPercent = rangedDamageReceivedPercent;
            this.weaponDamageReceivedPercent = weaponDamageReceivedPercent;
            this.spellDamageReceivedPercent = spellDamageReceivedPercent;
        }
        public virtual void Serialize(IDataWriter writer)
        {
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
            if (baseMaxLifePoints < 0)
            {
                throw new Exception("Forbidden value (" + baseMaxLifePoints + ") on element baseMaxLifePoints.");
            }

            writer.WriteVarInt((int)baseMaxLifePoints);
            if (permanentDamagePercent < 0)
            {
                throw new Exception("Forbidden value (" + permanentDamagePercent + ") on element permanentDamagePercent.");
            }

            writer.WriteVarInt((int)permanentDamagePercent);
            if (shieldPoints < 0)
            {
                throw new Exception("Forbidden value (" + shieldPoints + ") on element shieldPoints.");
            }

            writer.WriteVarInt((int)shieldPoints);
            writer.WriteVarShort((short)actionPoints);
            writer.WriteVarShort((short)maxActionPoints);
            writer.WriteVarShort((short)movementPoints);
            writer.WriteVarShort((short)maxMovementPoints);
            if (summoner < -9.00719925474099E+15 || summoner > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + summoner + ") on element summoner.");
            }

            writer.WriteDouble((double)summoner);
            writer.WriteBoolean((bool)summoned);
            writer.WriteVarShort((short)neutralElementResistPercent);
            writer.WriteVarShort((short)earthElementResistPercent);
            writer.WriteVarShort((short)waterElementResistPercent);
            writer.WriteVarShort((short)airElementResistPercent);
            writer.WriteVarShort((short)fireElementResistPercent);
            writer.WriteVarShort((short)neutralElementReduction);
            writer.WriteVarShort((short)earthElementReduction);
            writer.WriteVarShort((short)waterElementReduction);
            writer.WriteVarShort((short)airElementReduction);
            writer.WriteVarShort((short)fireElementReduction);
            writer.WriteVarShort((short)criticalDamageFixedResist);
            writer.WriteVarShort((short)pushDamageFixedResist);
            writer.WriteVarShort((short)pvpNeutralElementResistPercent);
            writer.WriteVarShort((short)pvpEarthElementResistPercent);
            writer.WriteVarShort((short)pvpWaterElementResistPercent);
            writer.WriteVarShort((short)pvpAirElementResistPercent);
            writer.WriteVarShort((short)pvpFireElementResistPercent);
            writer.WriteVarShort((short)pvpNeutralElementReduction);
            writer.WriteVarShort((short)pvpEarthElementReduction);
            writer.WriteVarShort((short)pvpWaterElementReduction);
            writer.WriteVarShort((short)pvpAirElementReduction);
            writer.WriteVarShort((short)pvpFireElementReduction);
            if (dodgePALostProbability < 0)
            {
                throw new Exception("Forbidden value (" + dodgePALostProbability + ") on element dodgePALostProbability.");
            }

            writer.WriteVarShort((short)dodgePALostProbability);
            if (dodgePMLostProbability < 0)
            {
                throw new Exception("Forbidden value (" + dodgePMLostProbability + ") on element dodgePMLostProbability.");
            }

            writer.WriteVarShort((short)dodgePMLostProbability);
            writer.WriteVarShort((short)tackleBlock);
            writer.WriteVarShort((short)tackleEvade);
            writer.WriteVarShort((short)fixedDamageReflection);
            writer.WriteByte((byte)invisibilityState);
            if (meleeDamageReceivedPercent < 0)
            {
                throw new Exception("Forbidden value (" + meleeDamageReceivedPercent + ") on element meleeDamageReceivedPercent.");
            }

            writer.WriteVarShort((short)meleeDamageReceivedPercent);
            if (rangedDamageReceivedPercent < 0)
            {
                throw new Exception("Forbidden value (" + rangedDamageReceivedPercent + ") on element rangedDamageReceivedPercent.");
            }

            writer.WriteVarShort((short)rangedDamageReceivedPercent);
            if (weaponDamageReceivedPercent < 0)
            {
                throw new Exception("Forbidden value (" + weaponDamageReceivedPercent + ") on element weaponDamageReceivedPercent.");
            }

            writer.WriteVarShort((short)weaponDamageReceivedPercent);
            if (spellDamageReceivedPercent < 0)
            {
                throw new Exception("Forbidden value (" + spellDamageReceivedPercent + ") on element spellDamageReceivedPercent.");
            }

            writer.WriteVarShort((short)spellDamageReceivedPercent);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            lifePoints = (int)reader.ReadVarUhInt();
            if (lifePoints < 0)
            {
                throw new Exception("Forbidden value (" + lifePoints + ") on element of GameFightMinimalStats.lifePoints.");
            }

            maxLifePoints = (int)reader.ReadVarUhInt();
            if (maxLifePoints < 0)
            {
                throw new Exception("Forbidden value (" + maxLifePoints + ") on element of GameFightMinimalStats.maxLifePoints.");
            }

            baseMaxLifePoints = (int)reader.ReadVarUhInt();
            if (baseMaxLifePoints < 0)
            {
                throw new Exception("Forbidden value (" + baseMaxLifePoints + ") on element of GameFightMinimalStats.baseMaxLifePoints.");
            }

            permanentDamagePercent = (int)reader.ReadVarUhInt();
            if (permanentDamagePercent < 0)
            {
                throw new Exception("Forbidden value (" + permanentDamagePercent + ") on element of GameFightMinimalStats.permanentDamagePercent.");
            }

            shieldPoints = (int)reader.ReadVarUhInt();
            if (shieldPoints < 0)
            {
                throw new Exception("Forbidden value (" + shieldPoints + ") on element of GameFightMinimalStats.shieldPoints.");
            }

            actionPoints = (short)reader.ReadVarShort();
            maxActionPoints = (short)reader.ReadVarShort();
            movementPoints = (short)reader.ReadVarShort();
            maxMovementPoints = (short)reader.ReadVarShort();
            summoner = (double)reader.ReadDouble();
            if (summoner < -9.00719925474099E+15 || summoner > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + summoner + ") on element of GameFightMinimalStats.summoner.");
            }

            summoned = (bool)reader.ReadBoolean();
            neutralElementResistPercent = (short)reader.ReadVarShort();
            earthElementResistPercent = (short)reader.ReadVarShort();
            waterElementResistPercent = (short)reader.ReadVarShort();
            airElementResistPercent = (short)reader.ReadVarShort();
            fireElementResistPercent = (short)reader.ReadVarShort();
            neutralElementReduction = (short)reader.ReadVarShort();
            earthElementReduction = (short)reader.ReadVarShort();
            waterElementReduction = (short)reader.ReadVarShort();
            airElementReduction = (short)reader.ReadVarShort();
            fireElementReduction = (short)reader.ReadVarShort();
            criticalDamageFixedResist = (short)reader.ReadVarShort();
            pushDamageFixedResist = (short)reader.ReadVarShort();
            pvpNeutralElementResistPercent = (short)reader.ReadVarShort();
            pvpEarthElementResistPercent = (short)reader.ReadVarShort();
            pvpWaterElementResistPercent = (short)reader.ReadVarShort();
            pvpAirElementResistPercent = (short)reader.ReadVarShort();
            pvpFireElementResistPercent = (short)reader.ReadVarShort();
            pvpNeutralElementReduction = (short)reader.ReadVarShort();
            pvpEarthElementReduction = (short)reader.ReadVarShort();
            pvpWaterElementReduction = (short)reader.ReadVarShort();
            pvpAirElementReduction = (short)reader.ReadVarShort();
            pvpFireElementReduction = (short)reader.ReadVarShort();
            dodgePALostProbability = (short)reader.ReadVarUhShort();
            if (dodgePALostProbability < 0)
            {
                throw new Exception("Forbidden value (" + dodgePALostProbability + ") on element of GameFightMinimalStats.dodgePALostProbability.");
            }

            dodgePMLostProbability = (short)reader.ReadVarUhShort();
            if (dodgePMLostProbability < 0)
            {
                throw new Exception("Forbidden value (" + dodgePMLostProbability + ") on element of GameFightMinimalStats.dodgePMLostProbability.");
            }

            tackleBlock = (short)reader.ReadVarShort();
            tackleEvade = (short)reader.ReadVarShort();
            fixedDamageReflection = (short)reader.ReadVarShort();
            invisibilityState = (byte)reader.ReadByte();
            if (invisibilityState < 0)
            {
                throw new Exception("Forbidden value (" + invisibilityState + ") on element of GameFightMinimalStats.invisibilityState.");
            }

            meleeDamageReceivedPercent = (short)reader.ReadVarUhShort();
            if (meleeDamageReceivedPercent < 0)
            {
                throw new Exception("Forbidden value (" + meleeDamageReceivedPercent + ") on element of GameFightMinimalStats.meleeDamageReceivedPercent.");
            }

            rangedDamageReceivedPercent = (short)reader.ReadVarUhShort();
            if (rangedDamageReceivedPercent < 0)
            {
                throw new Exception("Forbidden value (" + rangedDamageReceivedPercent + ") on element of GameFightMinimalStats.rangedDamageReceivedPercent.");
            }

            weaponDamageReceivedPercent = (short)reader.ReadVarUhShort();
            if (weaponDamageReceivedPercent < 0)
            {
                throw new Exception("Forbidden value (" + weaponDamageReceivedPercent + ") on element of GameFightMinimalStats.weaponDamageReceivedPercent.");
            }

            spellDamageReceivedPercent = (short)reader.ReadVarUhShort();
            if (spellDamageReceivedPercent < 0)
            {
                throw new Exception("Forbidden value (" + spellDamageReceivedPercent + ") on element of GameFightMinimalStats.spellDamageReceivedPercent.");
            }

        }


    }
}








