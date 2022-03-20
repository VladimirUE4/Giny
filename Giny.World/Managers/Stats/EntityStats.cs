using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Breeds;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Experiences;
using Giny.World.Managers.Formulas;
using Giny.World.Records;
using Giny.World.Records.Breeds;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Stats
{
    [ProtoContract]
    public class EntityStats
    {
        public const short BaseSummonsCount = 1;

        public event Action LifePointsChanged;

        private int m_lifePoints;

        public int LifePoints
        {
            get
            {
                LifePointsChanged?.Invoke();
                return m_lifePoints;
            }
            set
            {
                m_lifePoints = value;
            }
        }

        [ProtoMember(1)]
        public int MaxLifePoints
        {
            get;
            set;
        }
        public double LifePercentage
        {
            get
            {
                return (LifePoints / (double)MaxLifePoints) * 100;
            }
        }
        public int MissingLife
        {
            get
            {
                return MaxLifePoints - LifePoints;
            }
        }
        [ProtoMember(2)]
        public short MaxEnergyPoints
        {
            get;
            set;
        }


        public short Energy
        {
            get;
            set;
        }
        public int TotalInitiative
        {
            get
            {
                return StatsFormulas.Instance.TotalInitiative(this);
            }
        }
        [ProtoMember(3)]
        public Characteristic Initiative
        {
            get;
            set;
        }
        [ProtoMember(4)]
        public RelativeCharacteristic Prospecting
        {
            get;
            set;
        }
        [ProtoMember(5)]
        public ApCharacteristic ActionPoints
        {
            get;
            set;
        }
        [ProtoMember(6)]
        public MpCharacteristic MovementPoints
        {
            get;
            set;
        }
        [ProtoMember(7)]
        public RangeCharacteristic Range
        {
            get;
            set;
        }
        [ProtoMember(8)]
        public Characteristic SummonableCreaturesBoost
        {
            get;
            set;
        }
        [ProtoMember(9)]
        public Characteristic Reflect
        {
            get;
            set;
        }
        [ProtoMember(10)]
        public Characteristic CriticalHit
        {
            get;
            set;
        }
        [ProtoMember(11)]
        public short CriticalHitWeapon
        {
            get;
            set;
        }
        [ProtoMember(12)]
        public Characteristic HealBonus
        {
            get;
            set;
        }
        [ProtoMember(13)]
        public Characteristic AllDamagesBonus
        {
            get;
            set;
        }
        [ProtoMember(14)]
        public Characteristic DamagesBonusPercent
        {
            get;
            set;
        }
        [ProtoMember(15)]
        public Characteristic WeaponDamagesBonusPercent
        {

            get;
            set;
        }
        [ProtoMember(16)]
        public Characteristic TrapBonus
        {
            get;
            set;
        }
        [ProtoMember(17)]
        public Characteristic TrapBonusPercent
        {
            get;
            set;
        }
        [ProtoMember(18)]
        public Characteristic GlyphBonusPercent
        {
            get;
            set;
        }
        [ProtoMember(19)]
        public Characteristic RuneBonusPercent
        {
            get;
            set;
        }
        [ProtoMember(20)]
        public Characteristic PermanentDamagePercent
        {
            get;
            set;
        }
        [ProtoMember(21)]
        public Characteristic PushDamageBonus
        {
            get;
            set;
        }
        [ProtoMember(22)]
        public Characteristic CriticalDamageBonus
        {
            get;
            set;
        }
        [ProtoMember(23)]
        public Characteristic NeutralDamageBonus
        {
            get;
            set;
        }

        [ProtoMember(24)]
        public Characteristic EarthDamageBonus
        {
            get;
            set;
        }
        [ProtoMember(25)]
        public Characteristic WaterDamageBonus
        {
            get;
            set;
        }
        [ProtoMember(26)]
        public Characteristic AirDamageBonus
        {
            get;
            set;
        }
        [ProtoMember(27)]
        public Characteristic FireDamageBonus
        {
            get;
            set;
        }
        [ProtoMember(28)]
        public PointDodgeCharacteristic DodgePAProbability
        {
            get;
            set;
        }
        [ProtoMember(29)]
        public PointDodgeCharacteristic DodgePMProbability
        {
            get;
            set;
        }
        [ProtoMember(30)]
        public ResistanceCharacteristic NeutralResistPercent
        {
            get;
            set;
        }
        [ProtoMember(31)]
        public ResistanceCharacteristic EarthResistPercent
        {
            get;
            set;
        }
        [ProtoMember(32)]
        public ResistanceCharacteristic WaterResistPercent
        {
            get;
            set;
        }
        [ProtoMember(33)]
        public ResistanceCharacteristic AirResistPercent
        {
            get;
            set;
        }
        [ProtoMember(34)]
        public ResistanceCharacteristic FireResistPercent
        {
            get;
            set;
        }
        [ProtoMember(35)]
        public Characteristic NeutralReduction
        {
            get;
            set;
        }
        [ProtoMember(36)]
        public Characteristic EarthReduction
        {
            get;
            set;
        }
        [ProtoMember(37)]
        public Characteristic WaterReduction
        {
            get;
            set;
        }
        [ProtoMember(38)]
        public Characteristic AirReduction
        {
            get;
            set;
        }
        [ProtoMember(39)]
        public Characteristic FireReduction
        {
            get;
            set;
        }
        [ProtoMember(40)]
        public Characteristic PushDamageReduction
        {
            get;
            set;
        }
        [ProtoMember(41)]
        public Characteristic CriticalDamageReduction
        {
            get;
            set;
        }
        [ProtoMember(42)]
        public short GlobalDamageReduction
        {
            get;
            set;
        }
        [ProtoMember(43)]
        public Characteristic Strength
        {
            get;
            set;
        }
        [ProtoMember(44)]
        public Characteristic Agility
        {
            get;
            set;
        }
        [ProtoMember(45)]
        public Characteristic Chance
        {
            get;
            set;
        }
        [ProtoMember(46)]
        public Characteristic Vitality
        {
            get;
            set;
        }
        [ProtoMember(47)]
        public Characteristic Wisdom
        {
            get;
            set;
        }
        [ProtoMember(48)]
        public Characteristic Intelligence
        {
            get;
            set;
        }
        [ProtoMember(49)]
        public RelativeCharacteristic TackleBlock
        {
            get;
            set;
        }
        [ProtoMember(50)]
        public RelativeCharacteristic TackleEvade
        {
            get;
            set;
        }
        [ProtoMember(51)]
        public RelativeCharacteristic APAttack
        {
            get;
            set;
        }
        [ProtoMember(52)]
        public RelativeCharacteristic MPAttack
        {
            get;
            set;
        }
        [ProtoMember(53)]
        public Characteristic MeleeDamageDonePercent
        {
            get;
            set;
        }
        [ProtoMember(54)]
        public Characteristic MeleeDamageResistancePercent
        {
            get;
            set;
        }
        [ProtoMember(55)]
        public Characteristic RangedDamageDonePercent
        {
            get;
            set;
        }
        [ProtoMember(56)]
        public Characteristic RangedDamageResistancePercent
        {
            get;
            set;
        }
        [ProtoMember(57)]
        public Characteristic SpellDamageDonePercent
        {
            get;
            set;
        }
        [ProtoMember(58)]
        public Characteristic SpellDamageResistancePercent
        {
            get;
            set;
        }
        [ProtoMember(59)]
        public Characteristic WeaponDamageDonePercent
        {
            get;
            set;
        }
        [ProtoMember(60)]
        public Characteristic WeaponDamageResistancePercent
        {
            get;
            set;
        }
        [ProtoMember(61)]
        public Characteristic WeightBonus
        {
            get;
            set;
        }

        public void Initialize()
        {
            this.LifePoints = this.MaxLifePoints;
            this.Energy = this.MaxEnergyPoints;

            this.DodgePAProbability.Bind(this.Wisdom);
            this.APAttack.Bind(this.Wisdom);

            this.DodgePMProbability.Bind(this.Wisdom);
            this.MPAttack.Bind(this.Wisdom);

            this.TackleBlock.Bind(this.Agility);
            this.TackleEvade.Bind(this.Agility);

            this.Prospecting.Bind(this.Chance);
        }
        public Characteristic GetCharacteristic(StatsBoostEnum statId)
        {
            switch (statId)
            {
                case StatsBoostEnum.STRENGTH:
                    return Strength;
                case StatsBoostEnum.VITALITY:
                    return Vitality;
                case StatsBoostEnum.WISDOM:
                    return Wisdom;
                case StatsBoostEnum.CHANCE:
                    return Chance;
                case StatsBoostEnum.AGILITY:
                    return Agility;
                case StatsBoostEnum.INTELLIGENCE:
                    return Intelligence;
            }
            return null;
        }

        public int Total()
        {
            return Strength.Total() + Chance.Total() + Intelligence.Total() + Agility.Total();
        }

        public CharacterCharacteristic[] GetCharacterCharacteristics()
        {
            return new CharacterCharacteristic[0]; 
        }
        public CharacterCharacteristicsInformations GetCharacterCharacteristicsInformations(Character character)
        {
            var alignementInfos = new ActorExtendedAlignmentInformations()
            {
                aggressable = 0,
                alignmentGrade = 0,
                alignmentSide = 0,
                alignmentValue = 0,
                characterPower = 0,
                honor = 0,
                honorGradeFloor = 0,
                honorNextGradeFloor = 0,
            };

            return new CharacterCharacteristicsInformations()
            {
                alignmentInfos = alignementInfos,
                experienceBonusLimit = 0,
                characteristics = GetCharacterCharacteristics(),
                probationTime = 0,
                spellModifications = new CharacterSpellModification[0],
                criticalHitWeapon = CriticalHitWeapon,
                experience = character.Record.Experience,
                kamas = character.Record.Kamas,
                experienceLevelFloor = character.LowerBoundExperience,
                experienceNextLevelFloor = character.UpperBoundExperience,
            };
        }
        public static EntityStats New(short level, byte breedId)
        {
            BreedRecord breed = BreedRecord.GetBreed(breedId);

            var stats = new EntityStats()
            {
                ActionPoints = ApCharacteristic.New(ConfigFile.Instance.StartAp),
                MovementPoints = MpCharacteristic.New(ConfigFile.Instance.StartMp),
                Agility = Characteristic.Zero(),
                AirDamageBonus = Characteristic.Zero(),
                AirReduction = Characteristic.Zero(),
                AirResistPercent = ResistanceCharacteristic.Zero(),
                AllDamagesBonus = Characteristic.Zero(),
                DamagesBonusPercent = Characteristic.Zero(),
                Chance = Characteristic.Zero(),
                CriticalDamageBonus = Characteristic.Zero(),
                CriticalDamageReduction = Characteristic.Zero(),
                CriticalHit = Characteristic.Zero(),
                Initiative = Characteristic.Zero(),
                CriticalHitWeapon = 0,
                DodgePAProbability = PointDodgeCharacteristic.Zero(),
                DodgePMProbability = PointDodgeCharacteristic.Zero(),
                EarthDamageBonus = Characteristic.Zero(),
                EarthReduction = Characteristic.Zero(),
                EarthResistPercent = ResistanceCharacteristic.Zero(),
                FireDamageBonus = Characteristic.Zero(),
                FireReduction = Characteristic.Zero(),
                FireResistPercent = ResistanceCharacteristic.Zero(),
                GlobalDamageReduction = 0,
                GlyphBonusPercent = Characteristic.Zero(),
                RuneBonusPercent = Characteristic.Zero(),
                PermanentDamagePercent = Characteristic.Zero(),
                HealBonus = Characteristic.Zero(),
                Intelligence = Characteristic.Zero(),
                LifePoints = BreedManager.BreedDefaultLife,
                MaxLifePoints = BreedManager.BreedDefaultLife,
                MaxEnergyPoints = (short)(level * 100),
                Energy = (short)(level * 100),
                NeutralDamageBonus = Characteristic.Zero(),
                NeutralReduction = Characteristic.Zero(),
                NeutralResistPercent = ResistanceCharacteristic.Zero(),
                Prospecting = RelativeCharacteristic.New(BreedManager.BreedDefaultProspecting),
                PushDamageBonus = Characteristic.Zero(),
                PushDamageReduction = Characteristic.Zero(),
                Range = RangeCharacteristic.Zero(),
                Reflect = Characteristic.Zero(),
                Strength = Characteristic.Zero(),
                SummonableCreaturesBoost = Characteristic.New(BaseSummonsCount),
                TrapBonus = Characteristic.Zero(),
                TrapBonusPercent = Characteristic.Zero(),
                Vitality = Characteristic.Zero(),
                WaterDamageBonus = Characteristic.Zero(),
                WaterReduction = Characteristic.Zero(),
                WaterResistPercent = ResistanceCharacteristic.Zero(),
                WeaponDamagesBonusPercent = Characteristic.Zero(),
                Wisdom = Characteristic.Zero(),
                TackleBlock = RelativeCharacteristic.Zero(),
                TackleEvade = RelativeCharacteristic.Zero(),
                APAttack = RelativeCharacteristic.Zero(),
                MPAttack = RelativeCharacteristic.Zero(),
                MeleeDamageDonePercent = Characteristic.Zero(),
                MeleeDamageResistancePercent = Characteristic.Zero(),
                RangedDamageDonePercent = Characteristic.Zero(),
                RangedDamageResistancePercent = Characteristic.Zero(),
                SpellDamageDonePercent = Characteristic.Zero(),
                SpellDamageResistancePercent = Characteristic.Zero(),
                WeaponDamageDonePercent = Characteristic.Zero(),
                WeaponDamageResistancePercent = Characteristic.Zero(),
                WeightBonus = Characteristic.Zero(),
            };

            stats.Initialize();

            return stats;
        }

    }
}
