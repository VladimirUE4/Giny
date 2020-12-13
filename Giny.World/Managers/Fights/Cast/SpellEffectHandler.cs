using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Maps;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.World.Managers.Effects.Targets;
using Giny.World.Managers.Maps.Shapes;
using Giny.Core.DesignPattern;
using Giny.IO.D2OTypes;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Effects;
using Giny.World.Records.Effects;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Stats;
using Giny.Protocol.Enums;
using Giny.World.Records.Monsters;
using Giny.World.Managers.Fights.Triggers;
using Giny.World.Managers.Actions;

namespace Giny.World.Managers.Fights.Cast
{
    [WIP("dispellable should not be constants.")]
    public abstract class SpellEffectHandler
    {
        public Fighter Source
        {
            get
            {
                return CastHandler.Cast.Source;
            }
        }
        protected CellRecord CastCell
        {
            get
            {
                return CastHandler.Cast.CastCell;
            }
        }
        protected CellRecord TargetCell
        {
            get
            {
                return CastHandler.Cast.TargetCell;
            }
        }



        public EffectDice Effect
        {
            get;
            private set;
        }
        protected bool Critical
        {
            get
            {
                return CastHandler.Cast.IsCriticalHit;
            }
        }
        public IEnumerable<TargetCriterion> Targets
        {
            get;
            set;
        }
        public SpellCastHandler CastHandler
        {
            get;
            private set;
        }
        public bool IsCastByPortal
        {
            get;
            set;
        } = false;

        public virtual bool RefreshTargets => true;

        protected virtual bool Reveals => false;

        public Zone Zone
        {
            get;
            private set;
        }
        private ITriggerToken TriggerToken
        {
            get;
            set;
        }
        private IEnumerable<Fighter> AffectedFighters
        {
            get;
            set;
        }

        public SpellEffectHandler(EffectDice effect, SpellCastHandler castHandler)
        {
            Targets = effect.GetTargets();
            this.CastHandler = castHandler;
            Effect = effect;
            Zone = Effect.GetZone(CastCell.Point.OrientationTo(TargetCell.Point));
            this.AffectedFighters = GetAffectedFighters();
        }

        [WIP("Dont like this post condition to fill cells (no genercity)")]
        private IEnumerable<Fighter> GetAffectedFighters()
        {
            List<CellRecord> affectedCells = GetAffectedCells();

            /* foreach (var cell in affectedCells)
            {
                Source.Fight.Send(new Giny.Protocol.Messages.ShowCellMessage(cell.Id, cell.Id));
            } */

            if (Targets.Any(x => x is TargetTypeCriterion && ((TargetTypeCriterion)x).TargetType == SpellTargetType.SELF_ONLY) && !affectedCells.Contains(Source.Cell))
                affectedCells.Add(Source.Cell);

            return Source.Fight.GetFighters(affectedCells).Where(entry => entry.Alive && !entry.IsCarried() && IsValidTarget(entry)).ToArray();
        }

        public bool IsValidTarget(Fighter actor)
        {
            return Targets.ToLookup(x => x.GetType()).All(x => x.First().IsDisjonction ?
               x.Any(y => y.IsTargetValid(actor, this)) : x.All(y => y.IsTargetValid(actor, this)));
        }
        protected List<CellRecord> GetAffectedCells()
        {
            return Zone.GetCells(TargetCell, Source.Fight.Map).ToList();
        }
        /// <summary>
        /// prevent some stack overflow issues.
        /// </summary>
        /// <returns></returns>
        public bool CanTrigger()
        {
            return GetTriggerToken<ITriggerToken>() == null;
        }
        [WIP("usage?")]
        public virtual bool CanApply()
        {
            return true;
        }

        protected Spell CreateCastedSpell()
        {
            SpellRecord spellRecord = SpellRecord.GetSpellRecord((short)Effect.Min);
            SpellLevelRecord level = spellRecord.GetLevel((byte)Effect.Max);
            return new Spell(spellRecord, level);
        }
        public bool RevealsInvisible()
        {
            return Reveals && Trigger.IsInstant(Effect.Triggers);
        }
        public void Execute()
        {
            if (RefreshTargets || Targets.Any(x => x.CheckWhenExecute))
            {
                AffectedFighters = GetAffectedFighters();
            }

            Execute(AffectedFighters);
        }
        public void Execute(IEnumerable<Fighter> targets)
        {
            if (Effect.Triggers.Any(x => x.Type == TriggerType.Unknown))
            {
                Source.Fight.Warn("Unknown trigger(s) : " + Effect.RawTriggers + " cannot cast effect " + Effect.EffectEnum);
                return;
            }

            if (Effect.Delay > 0)
            {
                foreach (var target in targets)
                {
                    AddTriggerBuff(target, FightDispellableEnum.REALLY_NOT_DISPELLABLE, Trigger.Singleton(TriggerType.Delayed), delegate (TriggerBuff buff, ITriggerToken token)
                      {
                          InternalApply(new Fighter[] { target });
                          return false;

                      }, (short)Effect.Delay);
                }

            }
            else
            {
                InternalApply(targets);
            }

        }
        private void InternalApply(IEnumerable<Fighter> targets)
        {
            if (Trigger.IsInstant(Effect.Triggers))
            {
                Apply(targets);
            }
            else
            {
                foreach (var target in targets)
                {
                    AddTriggerBuff(target, FightDispellableEnum.REALLY_NOT_DISPELLABLE, Effect.Triggers, delegate (TriggerBuff buff, ITriggerToken token)
                    {
                        this.TriggerToken = token;
                        Apply(new Fighter[] { target });
                        return false;

                    }, 0);
                }
            }

        }
        protected abstract void Apply(IEnumerable<Fighter> targets);

        protected SummonedMonster CreateSummon(short monsterId)
        {
            MonsterRecord record = MonsterRecord.GetMonsterRecord(monsterId);
            SummonedMonster fighter = new SummonedMonster(Source, record, this, CastHandler.Cast.Spell.Level.Grade, TargetCell);
            return fighter;
        }


        [WIP("useless?")]
        public T GetTriggerToken<T>() where T : ITriggerToken
        {
            return (T)TriggerToken;
        }
        [WIP("useless?")]
        public void SetTriggerToken(ITriggerToken token)
        {
            TriggerToken = token;
        }
        protected TriggerBuff AddTriggerBuff(Fighter target, FightDispellableEnum dispellable, IEnumerable<Trigger> triggers, TriggerBuff.TriggerBuffApplyHandler applyTrigger,
            short delay)
        {
            return AddTriggerBuff(target, dispellable, triggers, applyTrigger, null, delay);
        }

        protected TriggerBuff AddTriggerBuff(Fighter target, FightDispellableEnum dispellable, IEnumerable<Trigger> triggers, TriggerBuff.TriggerBuffApplyHandler applyTrigger)
        {
            return AddTriggerBuff(target, dispellable, triggers, applyTrigger, 0);
        }
        protected TriggerBuff AddTriggerBuff(Fighter target, FightDispellableEnum dispellable, IEnumerable<Trigger> triggers, TriggerBuff.TriggerBuffApplyHandler applyTrigger,
           TriggerBuff.TriggerBuffRemoveHandler removeTrigger, short delay)
        {
            int id = target.BuffIdProvider.Pop();
            TriggerBuff triggerBuff = new TriggerBuff(id, triggers, applyTrigger, removeTrigger, delay, target, this, dispellable);
            target.AddBuff(triggerBuff);
            return triggerBuff;
        }
        public StatBuff AddStatBuff(Fighter target, short value, Characteristic characteristic, FightDispellableEnum dispellable, short? customActionId = null)
        {
            int id = target.BuffIdProvider.Pop();
            StatBuff statBuff = new StatBuff(id, target, this, Critical, dispellable, characteristic, value, customActionId);
            target.AddBuff(statBuff);
            return statBuff;
        }
        public StateBuff AddStateBuff(Fighter target, SpellStateRecord record, FightDispellableEnum dispellable)
        {
            int id = target.BuffIdProvider.Pop();
            StateBuff buff = new StateBuff(id, record, target, this, dispellable);
            target.AddBuff(buff);
            return buff;
        }
        public VitalityBuff AddVitalityBuff(Fighter target, short delta, FightDispellableEnum dispellable, ActionsEnum actionId)
        {
            int id = target.BuffIdProvider.Pop();
            VitalityBuff buff = new VitalityBuff(id, delta, target, this, dispellable, actionId);
            target.AddBuff(buff);
            return buff;
        }
        public void OnTokenMissing<T>() where T : ITriggerToken
        {
            Source.Fight.Warn("Unable to compute effect (" + Effect.EffectEnum + "). Token is missing (" + typeof(T).Name + ")");
        }
        public override string ToString()
        {
            return Effect.EffectEnum + " Z:" + Effect.RawZone + " TM:" + Effect.TargetMask + " TRIG:" + Effect.RawTriggers;
        }
    }
}
