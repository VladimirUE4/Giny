using Giny.Core.DesignPattern;
using Giny.Core.Time;
using Giny.Protocol.Custom.Enums;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Cast.Units
{
    public class Damage : ITriggerToken
    {
        public Fighter Source
        {
            get;
            private set;
        }
        public Fighter Target
        {
            get;
            private set;
        }
        public short BaseMinDamages
        {
            get;
            set;
        }
        public short BaseMaxDamages
        {
            get;
            set;
        }
        public short Delta
        {
            get;
            private set;
        }
        public short? Computed
        {
            get;
            set;
        }
        public EffectSchoolEnum EffectSchool
        {
            get;
            private set;
        }
        private SpellEffectHandler EffectHandler
        {
            get;
            set;
        }
        public bool IgnoreBoost
        {
            get;
            set;
        }

        public Damage(Fighter source, Fighter target, EffectSchoolEnum school, short min, short max, SpellEffectHandler effectHandler = null)
        {
            this.Source = source;
            this.Target = target;
            this.BaseMaxDamages = max;
            this.BaseMinDamages = min;
            this.EffectSchool = school;
            this.EffectHandler = effectHandler;
            this.IgnoreBoost = false;
        }
        public void Compute()
        {
            if (Computed.HasValue)
            {
                return;
            }
            if (EffectSchool == EffectSchoolEnum.Unknown)
            {
                throw new Exception("Unknown Effect school. Cannot compute damages.");
            }

            if (EffectSchool == EffectSchoolEnum.Fix)
            {
                Computed = new Jet(BaseMinDamages, BaseMaxDamages).Generate();
                return;
            }
            if (EffectSchool == EffectSchoolEnum.Pushback)
            {
                if (BaseMinDamages != BaseMaxDamages)
                {
                    throw new Exception("Invalid push damages.");
                }

                Computed = BaseMaxDamages;
                return;
            }

            if (BaseMinDamages <= 0)
            {
                Computed = 0;
                return;
            }

            Jet jet = EvaluateConcreteJet();

            if (!IgnoreBoost)
                ComputeCriticalDamageBonus(jet);

            ComputeShapeEfficiencyModifiers(jet);

            ComputeDamageResistances(jet);

            ComputeCriticalDamageReduction(jet);

            if (!IgnoreBoost)
                ComputeDamageDone(jet);

            if (!IgnoreBoost)
                ComputeFinalDamageBoost(jet);

            jet.ValidateBounds();

            Source.Fight.Reply("Min:" + jet.Min + " Max:" + jet.Max, System.Drawing.Color.Red);

            Computed = jet.Generate();

            this.EffectHandler.CastHandler.Cast.DamagesDealt += Computed.Value;
        }

        private void ComputeFinalDamageBoost(Jet jet)
        {
            jet.Min += jet.Min * (Source.Stats.FinalDamagePercent / 100d);
            jet.Max += jet.Max * (Source.Stats.FinalDamagePercent / 100d);
        }

        private void ComputeCriticalDamageBonus(Jet jet)
        {
            if (this.EffectHandler.CastHandler.Cast.IsCriticalHit)
            {
                jet.Min += this.Source.Stats.CriticalDamageBonus.TotalInContext();
                jet.Max += this.Source.Stats.CriticalDamageBonus.TotalInContext();
            }
        }
        private void ComputeCriticalDamageReduction(Jet jet)
        {
            if (this.EffectHandler.CastHandler.Cast.IsCriticalHit)
            {
                jet.Min -= this.Target.Stats.CriticalDamageReduction.TotalInContext();
                jet.Max -= this.Target.Stats.CriticalDamageReduction.TotalInContext();
            }
        }

        private void ComputeDamageDone(Jet jet)
        {
            if (Source.IsMeleeWith(Target))
            {
                jet.Min += (short)(jet.Min * (Source.Stats.MeleeDamageDonePercent.TotalInContext() / 100d));
                jet.Max += (short)(jet.Max * (Source.Stats.MeleeDamageDonePercent.TotalInContext() / 100d));

                jet.Min -= (short)(jet.Min * (Target.Stats.MeleeDamageResistancePercent.TotalInContext() / 100d));
                jet.Max -= (short)(jet.Max * (Target.Stats.MeleeDamageResistancePercent.TotalInContext() / 100d));
            }
            else
            {
                jet.Min += (short)(jet.Min * (Source.Stats.RangedDamageDonePercent.TotalInContext() / 100d));
                jet.Max += (short)(jet.Min * (Source.Stats.RangedDamageDonePercent.TotalInContext() / 100d));

                jet.Min -= (short)(jet.Min * (Target.Stats.RangedDamageResistancePercent.TotalInContext() / 100d));
                jet.Max -= (short)(jet.Max * (Target.Stats.RangedDamageResistancePercent.TotalInContext() / 100d));
            }

            if (this.EffectHandler.CastHandler.Cast.Weapon)
            {
                jet.Min += (short)(jet.Min * (Source.Stats.WeaponDamageDonePercent.TotalInContext() / 100d));
                jet.Max += (short)(jet.Max * (Source.Stats.WeaponDamageDonePercent.TotalInContext() / 100d));

                jet.Min -= (short)(jet.Min * (Target.Stats.WeaponDamageResistancePercent.TotalInContext() / 100d));
                jet.Max -= (short)(jet.Max * (Target.Stats.WeaponDamageResistancePercent.TotalInContext() / 100d));
            }

            if (this.IsSpellDamage())
            {
                jet.Min += (short)(jet.Min * (Source.Stats.SpellDamageDonePercent.TotalInContext() / 100d));
                jet.Max += (short)(jet.Max * (Source.Stats.SpellDamageDonePercent.TotalInContext() / 100d));

                jet.Min -= (short)(jet.Min * (Target.Stats.SpellDamageResistancePercent.TotalInContext() / 100d));
                jet.Max -= (short)(jet.Max * (Target.Stats.SpellDamageResistancePercent.TotalInContext() / 100d));
            }
        }
        private void ComputeShapeEfficiencyModifiers(Jet jet)
        {
            double efficiency = EffectHandler.Zone.GetShapeEfficiency(Target.Cell, EffectHandler.CastHandler.Cast.TargetCell);
            jet.Min = (short)(jet.Min * efficiency);
            jet.Max = (short)(jet.Max * efficiency);
        }

        private void ComputeDamageResistances(Jet jet)
        {
            int resistPercent = 0;
            int reduction = 0;

            switch (EffectSchool)
            {
                case EffectSchoolEnum.Earth:
                    resistPercent = Target.Stats.EarthResistPercent.TotalInContext();
                    reduction = Target.Stats.EarthReduction.TotalInContext();
                    break;
                case EffectSchoolEnum.Air:
                    resistPercent = Target.Stats.AirResistPercent.TotalInContext();
                    reduction = Target.Stats.AirReduction.TotalInContext();
                    break;
                case EffectSchoolEnum.Water:
                    resistPercent = Target.Stats.WaterResistPercent.TotalInContext();
                    reduction = Target.Stats.WaterReduction.TotalInContext();
                    break;
                case EffectSchoolEnum.Fire:
                    resistPercent = Target.Stats.FireResistPercent.TotalInContext();
                    reduction = Target.Stats.FireReduction.TotalInContext();
                    break;
                case EffectSchoolEnum.Neutral:
                    resistPercent = Target.Stats.NeutralResistPercent.TotalInContext();
                    reduction = Target.Stats.NeutralReduction.TotalInContext();
                    break;
            }

            jet.Min = (1.0d - (resistPercent / 100.0d)) * (jet.Min - reduction);
            jet.Max = (1.0d - (resistPercent / 100.0d)) * (jet.Max - reduction);
        }

        public Jet EvaluateConcreteJet()
        {
            short boost = Source.GetSpellBoost(EffectHandler.CastHandler.Cast.SpellId);

            if (BaseMaxDamages == 0 || BaseMaxDamages <= BaseMinDamages)
            {
                BaseMinDamages += boost;

                short delta = GetJetDelta(BaseMinDamages);

                return new Jet(delta, delta);
            }
            else
            {
                int jetMin = BaseMinDamages + boost;
                int jetMax = BaseMaxDamages + boost;

                short deltaMin = GetJetDelta((short)jetMin);
                short deltaMax = GetJetDelta((short)jetMax);

                return new Jet(deltaMin, deltaMax);
            }
        }
        public bool IsSpellDamage()
        {
            return EffectHandler != null && !EffectHandler.CastHandler.Cast.Weapon;
        }
        private short GetJetDelta(short jet)
        {
            double weaponDamageBonus = 0;
            double spellDamageBonus = 0;
            double damageBonusPercent = 0;
            double elementDamageBonus = 0;
            double allDamageBonus = 0;
            double elementDelta = 0;

            if (this.EffectHandler.CastHandler.Cast.Weapon)
            {
                weaponDamageBonus = Source.Stats.WeaponDamagesBonusPercent.TotalInContext();
            }
            else if (IsSpellDamage())
            {
                spellDamageBonus = Source.Stats.SpellDamageBonusPercent;
            }

            if (!IgnoreBoost)
            {
                allDamageBonus = Source.Stats.AllDamagesBonus.TotalInContext();
                damageBonusPercent = Source.Stats.DamagesBonusPercent.TotalInContext();
            }

            if (!IgnoreBoost)
            {

                switch (EffectSchool)
                {
                    case EffectSchoolEnum.Neutral:
                        elementDelta = Source.Stats.Strength.TotalInContext();
                        elementDamageBonus = Source.Stats.NeutralDamageBonus.TotalInContext();
                        break;
                    case EffectSchoolEnum.Earth:
                        elementDelta = Source.Stats.Strength.TotalInContext();
                        elementDamageBonus = Source.Stats.EarthDamageBonus.TotalInContext();
                        break;
                    case EffectSchoolEnum.Water:
                        elementDelta = Source.Stats.Chance.TotalInContext();
                        elementDamageBonus = Source.Stats.WaterDamageBonus.TotalInContext();
                        break;
                    case EffectSchoolEnum.Air:
                        elementDelta = Source.Stats.Agility.TotalInContext();
                        elementDamageBonus = Source.Stats.AirDamageBonus.TotalInContext();
                        break;
                    case EffectSchoolEnum.Fire:
                        elementDelta = Source.Stats.Intelligence.TotalInContext();
                        elementDamageBonus = Source.Stats.FireDamageBonus.TotalInContext();
                        break;
                    default:
                        elementDelta = jet;
                        break;
                }
            }




            double result = (double)(jet * (100d + elementDelta + damageBonusPercent + weaponDamageBonus + spellDamageBonus) / 100.0d + (allDamageBonus + elementDamageBonus));

            return (short)(result < jet ? jet : result);
        }

        public Fighter GetSource()
        {
            return Source;
        }
    }
}
