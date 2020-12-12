using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Pool;
using Giny.Core.Time;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Api;
using Giny.World.Handlers.Roleplay.Maps.Paths;
using Giny.World.Managers.Actions;
using Giny.World.Managers.Entities.Look;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Cast.Units;
using Giny.World.Managers.Fights.Effects.Damages;
using Giny.World.Managers.Fights.History;
using Giny.World.Managers.Fights.Marks;
using Giny.World.Managers.Fights.Results;
using Giny.World.Managers.Fights.Sequences;
using Giny.World.Managers.Fights.Stats;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Maps.Shapes.Sets;
using Giny.World.Managers.Spells;
using Giny.World.Records.Maps;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Fighters
{
    public abstract class Fighter : ITriggerToken
    {
        public event Action<Fighter> Moved;

        public int Id
        {
            get;
            set;
        }
        public abstract string Name
        {
            get;
        }
        public bool Alive
        {
            get;
            set;
        } = true;



        public Fight Fight
        {
            get;
            set;
        }
        public FightTeam Team
        {
            get;
            set;
        }
        public FightTeam EnemyTeam
        {
            get
            {
                return Team == Fight.BlueTeam ? Fight.RedTeam : Fight.BlueTeam;
            }
        }
        private bool IsMoving
        {
            get;
            set;
        }

        public CellRecord Cell
        {
            get;
            protected set;
        }


        public CellRecord TurnStartCell
        {
            get;
            set;
        }
        public DirectionsEnum Direction
        {
            get;
            private set;
        }
        public ServerEntityLook Look
        {
            get;
            set;
        }
        public ServerEntityLook BaseLook
        {
            get;
            private set;
        }
        public FighterStats Stats
        {
            get;
            set;
        }
        public DateTime? DeathTime
        {
            get;
            set;
        }
        public bool IsReady
        {
            get;
            set;
        } = false;

        public abstract short Level
        {
            get;
        }
        public CellRecord FightStartCell

        {
            get;
            private set;
        }

        public CellRecord RoleplayCell
        {
            get;
            set;
        }

        private List<Buff> Buffs
        {
            get;
            set;
        }
        public UniqueIdProvider BuffIdProvider
        {
            get;
            private set;
        }

        public bool IsFighterTurn => Fight.FighterPlaying == this;

        public bool Left
        {
            get;
            set;
        }
        public Loot Loot
        {
            get;
            private set;
        }

        public virtual bool CanDrop => false;

        public SpellHistory SpellHistory
        {
            get;
            private set;
        }
        public MovementHistory MovementHistory
        {
            get;
            private set;
        }
        public Fighter LastAttacker
        {
            get;
            private set;
        }
        public abstract bool Sex
        {
            get;
        }

        private Dictionary<short, short> m_spellsCosts
        {
            get;
            set;
        }
        public Fighter(FightTeam team, CellRecord roleplayCell)
        {
            this.Team = team;
            this.RoleplayCell = roleplayCell;
            this.Cell = Team.GetPlacementCell();
            this.Loot = new Loot();
            this.Buffs = new List<Buff>();
            this.BuffIdProvider = new UniqueIdProvider();
            this.SpellHistory = new SpellHistory(this);
            this.m_spellsCosts = new Dictionary<short, short>();
        }

        public bool IsCarried()
        {
            return false;
        }

        public virtual void Initialize()
        {
            this.TurnStartCell = this.Cell;
            this.MovementHistory = new MovementHistory(this);
            this.BaseLook = Look.Clone();
        }
        public void FindPlacementDirection()
        {
            Tuple<short, short> tuple = null;

            foreach (Fighter current in this.EnemyTeam.GetFighters<Fighter>())
            {
                MapPoint point = current.Cell.Point;
                if (tuple == null)
                {
                    tuple = Tuple.Create(current.Cell.Id, this.Cell.Point.ManhattanDistanceTo(point));
                }
                else
                {
                    if (this.Cell.Point.ManhattanDistanceTo(point) < tuple.Item2)
                    {
                        tuple = Tuple.Create(current.Cell.Id, this.Cell.Point.ManhattanDistanceTo(point));
                    }
                }
            }
            if (tuple == null)
            {
                this.Direction = DirectionsEnum.DIRECTION_SOUTH_WEST;
            }
            else
            {
                this.Direction = this.Cell.Point.OrientationTo(new MapPoint((short)tuple.Item1), false);
            }
        }



        public virtual IEnumerable<DroppedItem> RollLoot(IFightResult looter)
        {
            return new DroppedItem[0];
        }

        public virtual void OnMoveFailed(MovementFailedReason reason)
        {

        }
        protected virtual void OnTackled(short looseMp, short looseAp, IEnumerable<Fighter> tacklers)
        {
            Fight.Send(new GameActionFightTackledMessage()
            {
                actionId = 0,
                sourceId = this.Id,
                tacklersIds = tacklers.Select(x => (double)x.Id).ToArray(),
            });

            this.LooseAp(this, looseAp, ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_LOST);
            this.LooseMp(this, looseMp, ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST);
        }
        private List<CellRecord> ApplyTackle(List<CellRecord> path)
        {
            if (CantBeTackled() || IsInvisible())
            {
                return path;
            }

            IEnumerable<Fighter> tacklers = GetTacklers(Cell);

            Tackle tackle = GetTackle(tacklers);

            if (tackle.ApLoss > 0 || tackle.MpLoss > 0)
            {
                if (tackle.MpLoss >= Stats.MovementPoints.TotalInContext())
                {
                    return new List<CellRecord>();
                }

                OnTackled((short)tackle.MpLoss, (short)tackle.ApLoss, tacklers);
            }

            int index = 1;

            foreach (var cell in path.Skip(1))
            {
                tacklers = GetTacklers(cell);
                tackle = GetTackle(tacklers);

                if (tackle.Consistent())
                {
                    break;
                }

                index++;
            }

            path = path.Take(index + 1).ToList();

            return path.Take(1 + Stats.MovementPoints.TotalInContext()).ToList();
        }
        private IEnumerable<Fighter> GetTacklers(CellRecord cell)
        {
            return this.EnemyTeam.GetFighters<Fighter>().Where(x => x.IsMeleeWith(cell.Point) && !x.CantTackle());
        }
        private Tackle GetTackle(IEnumerable<Fighter> tacklers)
        {
            double result = 1;

            foreach (var tackler in tacklers)
            {
                result *= (Stats.TackleEvade.TotalInContext() + 2) / (2d * (tackler.Stats.TackleBlock.TotalInContext() + 2));
            }

            short looseAp = 0;
            short looseMp = 0;

            if (result < 1 && result > 0)
            {
                looseAp = (short)Math.Round(Stats.ActionPoints.TotalInContext() * (1 - result));
                looseMp = (short)Math.Round(Stats.MovementPoints.TotalInContext() * (1 - result));
            }

            return new Tackle(looseAp, looseMp);
        }
        public bool IsTackled()
        {
            return GetTackle(GetTacklers(Cell)).Consistent();
        }

        public virtual void Move(List<CellRecord> path)
        {
            path.Insert(0, this.Cell);

            if (path.Count <= 1)
            {
                return;
            }
            if (Fight.Ended || !Fight.Started)
                return;

            if (!path.Skip(1).All(x => Fight.IsCellFree(x)))
            {
                this.OnMoveFailed(MovementFailedReason.Obstacle);
                return;
            }

            for (int i = 1; i < path.Count; i++)
            {
                if (Fight.ShouldTriggerOnMove(path[i - 1].Id, path[i].Id))
                {
                    if (i + 1 <= path.Count)
                    {
                        path.RemoveRange(i + 1, path.Count - i - 1);
                    }
                    break;
                }
            }

            using (Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE))
            {
                short mpCost = (short)(path.Count - 1);

                if (mpCost <= Stats.MovementPoints.TotalInContext() && mpCost > 0 && Stats.MovementPoints.TotalInContext() > 0)
                {
                    path = ApplyTackle(path);

                    if (path.Count() > 0)
                    {
                        IsMoving = true;

                        mpCost = (short)(path.Count - 1);

                        DirectionsEnum direction = (DirectionsEnum)PathReader.GetDirection(path.Last().Id);

                        this.Cell = Fight.Map.GetCell(path.Last().Id);

                        this.Direction = direction;

                        if (Stats.InvisibilityState == GameActionFightInvisibilityStateEnum.INVISIBLE)
                        {
                            Team.Send(new GameMapMovementMessage(path.Select(x => x.Id).ToArray(), -1, Id));
                        }
                        else
                        {
                            Fight.Send(new GameMapMovementMessage(path.Select(x => x.Id).ToArray(), -1, Id));
                        }

                        this.MovementHistory.OnMove(path);

                        OnMove(this, true);

                        this.LooseMp(this, mpCost, ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE);

                        IsMoving = false;
                    }
                    else
                    {
                        this.OnMoveFailed(MovementFailedReason.Tackle);
                    }
                }
                else
                {
                    this.OnMoveFailed(MovementFailedReason.MissingMp);
                }

            }


        }



        public bool IsFriendlyWith(Fighter actor)
        {
            return actor.Team == this.Team;
        }
        public bool IsEnnemyWith(Fighter actor)
        {
            return !IsFriendlyWith(actor);
        }
        public void LooseMp(Fighter source, short amount, ActionsEnum action)
        {
            Stats.UseMp(amount);
            Fight.PointsVariation(source.Id, Id, action, (short)(-amount));
        }
        public void LooseAp(Fighter source, short amount, ActionsEnum action)
        {
            Stats.UseAp(amount);
            Fight.PointsVariation(source.Id, Id, action, (short)(-amount));
        }
        public void GainAp(Fighter source, short delta)
        {
            Stats.GainAp(delta);
            Fight.PointsVariation(source.Id, Id, ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_WIN, delta);
        }
        public void GainMp(Fighter source, short delta)
        {
            Stats.GainMp(delta);
            Fight.PointsVariation(source.Id, Id, ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_WIN, delta);
        }
        public virtual void PassTurn()
        {
            if (!IsFighterTurn)
                return;

            Fight.StopTurn();

        }
        public void ModifyPlacement(short cellId, bool send = true)
        {
            if (!Fight.Started)
            {
                lock (Fight)
                {
                    this.Cell = Fight.Map.GetCell(cellId);

                    if (send)
                    {
                        this.Fight.UpdateFightersPlacementDirection();
                        this.Fight.UpdateEntitiesPositions();
                    }
                }
            }
        }
        public short GetSpellBoost(short spellId)
        {
            return (short)GetBuffs<SpellBoostBuff>().Where(x => x.BoostedSpellId == spellId).Sum(x => x.Delta); // Sum, or we pick first one ?
        }
        public void DecrementAllCastedBuffsDuration()
        {
            foreach (Fighter current in this.Fight.GetFighters<Fighter>())
            {
                current.DecrementBuffsDuration(this);
            }
        }

        public void DecrementSummonsCastedBuffsDuration()
        {
            foreach (var summon in GetSummons())
            {
                summon.DecrementAllCastedBuffsDuration();
            }
        }

        public void DecrementSummonsCastedBuffsDelays()
        {
            foreach (var summon in GetSummons())
            {
                summon.DecrementAllCastedBuffsDelay();
            }
        }
        private IEnumerable<Fighter> GetSummons()
        {
            return Fight.GetFighters<Fighter>(x => x.IsSummoned() && x.GetSummoner() == this);
        }


        public void DecrementBuffsDuration(Fighter caster)
        {
            foreach (var buff in Buffs.ToArray())
            {
                if (buff.Cast.Source == caster && !buff.HasDelay())
                {
                    if (buff.DecrementDuration())
                    {
                        RemoveAndDispellBuff(buff);
                    }
                }
            }
        }
        public bool HasBuff<T>() where T : Buff
        {
            return GetBuffs<T>().Count() > 0;
        }

        public abstract void OnTurnBegin();

        public void DecrementAllCastedBuffsDelay()
        {
            foreach (var fighter in Fight.GetFighters<Fighter>())
            {
                foreach (var buff in fighter.GetBuffs<TriggerBuff>().Where(x => x.HasDelay()).ToArray())
                {
                    if (buff.Cast.Source == this && buff.DecrementDelay())
                    {
                        buff.Apply();

                        RemoveAndDispellBuff(buff);
                    }
                }
            }
        }
        public void RemoveAndDispellBuff(Buff buff)
        {
            this.RemoveBuff(buff);
            buff.Dispell();
        }

        public void RemoveBuffs<T>() where T : Buff
        {
            foreach (var buff in GetBuffs<T>().ToArray())
            {
                RemoveAndDispellBuff(buff);
            }
        }
        public void RemoveSpellEffects(Fighter source, short spellId)
        {
            IEnumerable<Buff> buffs = this.Buffs.Where(x => x.Cast.SpellId == spellId);

            foreach (var buff in buffs.ToArray())
            {
                RemoveAndDispellBuff(buff);
            }

            Fight.Send(new GameActionFightDispellSpellMessage()
            {
                actionId = 0,
                sourceId = source.Id,
                spellId = spellId,
                targetId = Id,
                verboseCast = true
            });
        }
        public void RemoveBuff(Buff buff)
        {
            this.Buffs.Remove(buff);

            Fight.Send(new GameActionFightDispellEffectMessage()
            {
                actionId = (short)ActionsEnum.ACTION_CHARACTER_BOOST_DISPELLED,
                boostUID = buff.Id,
                sourceId = buff.Target.Id, // ?
                targetId = buff.Target.Id,
                verboseCast = true,
            });
            BuffIdProvider.Push(buff.Id);
        }
        public void AddBuff(Buff buff)
        {
            if (BuffMaxStackReached(buff)) // WIP censer cumuler la durée ?
            {
                Buff oldBuff = Buffs.FirstOrDefault(x => IsSimilar(x, buff));
                RemoveAndDispellBuff(oldBuff);
            }

            Buffs.Add(buff);

            if (buff.GetTriggerType() == BuffTriggerType.Instant && !buff.HasDelay())
            {
                buff.Apply();
            }

            OnBuffAdded(buff);
        }
        public bool TriggerBuffs(BuffTriggerType type, ITriggerToken token)
        {
            bool result = false;

            foreach (var buff in GetBuffs<TriggerBuff>().Where(x => x.TriggerType == type && !x.HasDelay()).ToArray())
            {
                if (buff.Apply(token))
                {
                    result = true;
                }
            }

            return result;
        }
        private void OnBuffAdded(Buff buff)
        {
            var abstractFightDispellableEffect = buff.GetAbstractFightDispellableEffect();

            Fight.Send(new GameActionFightDispellableEffectMessage()
            {
                actionId = buff.GetActionId(),
                effect = abstractFightDispellableEffect,
                sourceId = buff.Cast.Source.Id,
            }); ;
        }
        public virtual void OnBuffDurationUpdated(Fighter source, short actionId, Buff buff, short delta)
        {
            Fight.Send(new GameActionFightModifyEffectsDurationMessage()
            {
                actionId = actionId,
                delta = (short)(-delta),
                sourceId = source.Id,
                targetId = Id,
            });
        }
        public IEnumerable<T> GetBuffs<T>() where T : Buff
        {
            return Buffs.OfType<T>();
        }
        public IEnumerable<Buff> GetBuffs()
        {
            return Buffs;
        }
        public bool BuffMaxStackReached(Buff buff)
        {
            bool result = buff.Cast.Spell.Level.MaxStack > 0 &&
                buff.Cast.Spell.Level.MaxStack <= this.Buffs.Count((Buff entry) => IsSimilar(entry, buff));

            return result;
        }
        private bool IsSimilar(Buff current, Buff reference)
        {
            bool result = current.Cast.SpellId == reference.Cast.SpellId &&
                 current.Effect.EffectId == reference.Effect.EffectId && current.Effect.Delay == reference.Effect.Delay
                 && current.GetTriggerType() == reference.GetTriggerType() && current.GetType().Name == reference.GetType().Name;

            if (current is StateBuff && reference is StateBuff)
            {
                return result && ((StateBuff)current).Record.Id == ((StateBuff)reference).Record.Id;
            }

            return result;
        }
        public void SwapPlacementPosition(Fighter target)
        {
            short cellId = this.Cell.Id;

            this.ModifyPlacement(target.Cell.Id, false);
            target.ModifyPlacement(cellId, false);

            this.Fight.UpdateFightersPlacementDirection();
            this.Fight.UpdateEntitiesPositions();
        }
        public virtual bool CanPlay()
        {
            return Alive;
        }
        public virtual IFightResult GetFightResult()
        {
            return new FightResult(this, this.GetFighterOutcome(), Loot);
        }
        public virtual int GetDroppedKamas()
        {
            return 0;
        }
        public FightOutcomeEnum GetFighterOutcome()
        {
            bool flag = this.Team.Alives == 0;
            bool flag2 = this.EnemyTeam.Alives == 0;
            FightOutcomeEnum result;
            if (!flag && flag2)
            {
                result = FightOutcomeEnum.RESULT_VICTORY;
            }
            else
            {
                if (flag && !flag2)
                {
                    result = FightOutcomeEnum.RESULT_LOST;
                }
                else
                {
                    result = FightOutcomeEnum.RESULT_DRAW;
                }
            }
            return result;
        }
        public short GetMPDistance(Fighter other)
        {
            return Cell.Point.DistanceTo(other.Cell.Point);
        }
        public virtual void OnJoined()
        {
            this.ShowFighter();
            this.Fight.UpdateFightersPlacementDirection();
            this.Fight.UpdateEntitiesPositions();
            Fight.UpdateTeams();

            FightApi.FighterJoined(this);
        }
        public void ShowFighter()
        {
            foreach (var characterFighter in Fight.GetFighters<CharacterFighter>())
            {
                ShowFighter(characterFighter);
            }
        }



        public void ShowFighter(CharacterFighter fighter)
        {
            fighter.Character.Client.Send(new GameFightShowFighterMessage(GetFightFighterInformations(fighter)));
        }

        public EntityDispositionInformations GetEntityDispositionInformations()
        {
            return new FightEntityDispositionInformations()
            {
                cellId = (short)Cell.Id,
                direction = (byte)Direction,
                carryingCharacterId = 0,
            };
        }

        public void BeforeTurnEnd()
        {
            using (Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END))
            {
                if (Alive)
                {
                    Fight.TriggerMarks(this, MarkTriggerType.OnTurnEnd);
                    TriggerBuffs(BuffTriggerType.OnTurnEnd, null);
                }

                OnTurnEnded();
            }
        }

        public abstract void OnTurnEnded();

        public IdentifiedEntityDispositionInformations GetIdentifiedEntityDispositionInformations()
        {
            return new IdentifiedEntityDispositionInformations()
            {
                cellId = Cell.Id,
                direction = (byte)Direction,
                id = Id,
            };
        }

        public bool IsInvisible()
        {
            return Stats.InvisibilityState == GameActionFightInvisibilityStateEnum.INVISIBLE;
        }


        public virtual bool CastSpell(short spellId, short cellId)
        {
            Spell spell = GetSpell(spellId);
            CellRecord cell = Fight.Map.GetCell(cellId);
            return this.CastSpell(new SpellCast(this, spell, Fight.Map.GetCell(cellId)));
        }

        public virtual bool CastSpell(SpellCast cast)
        {
            if (Fight.Ended)
                return false;

            if (!cast.Force && (!IsFighterTurn || !Alive))
                return false;

            var result = CanCastSpell(cast);

            if (result != SpellCastResult.OK && !cast.Force)
            {
                OnSpellCastFailed(cast);
                return false;
            }

            using (Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL))
            {
                cast.Critical = RollCriticalDice(cast.Spell.Level);

                SpellCastHandler handler = SpellManager.Instance.CreateSpellCastHandler(cast);

                if (!handler.Initialize())
                {
                    OnSpellCastFailed(cast);
                    return false;
                }

                OnSpellCasting(handler);

                if (!cast.ApFree)
                    LooseAp(this, GetApCost(cast.Spell.Level), ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE);

                if (!handler.Execute())
                {
                    Fight.Warn("Unable to cast spell : " + cast.Spell.Record.Name);
                }

                OnSpellCasted(handler);
                Fight.CheckDeads();
            }
            Fight.CheckFightEnd();

            return true;
        }


        private bool WontReveals()
        {
            IEnumerable<Glyph> effectiveGlyphs = GetEffectiveGlyphs().OfType<GlyphAura>();

            foreach (var glyph in effectiveGlyphs)
            {
                bool res = GetBuffs<InvisibilityBuff>().Any(x => x.Cast.MarkSource == glyph);

                if (res)
                {
                    return true;
                }
            }

            return false;
        }
        public void Reveals()
        {
            if (!IsInvisible())
            {
                return;
            }

            foreach (var buff in GetBuffs<InvisibilityBuff>().ToArray())
            {
                RemoveAndDispellBuff(buff);
            }

        }

        protected virtual void OnSpellCasted(SpellCastHandler handler)
        {
            this.SpellHistory.RegisterCastedSpell(handler.Cast.Spell.Level, this.Fight.GetFighter(handler.Cast.TargetCell.Id));

            foreach (var summon in GetSummons())
            {
                if (handler.GetEffectHandlers().Contains(summon.GetSummoningEffect()))
                {
                    Fight.TriggerMarks(summon, MarkTriggerType.OnMove);
                }
            }
        }

        [WIP(WIPState.Todo, "see stump")]
        private void OnSpellCasting(SpellCastHandler handler)
        {
            if (IsInvisible() && !handler.Cast.Force)
            {
                if (handler.RevealsInvisible() && !WontReveals())
                {
                    Reveals();
                }
                else
                {
                    OnDetected();
                }

            }

            Fighter target = Fight.GetFighter(handler.Cast.TargetCell.Id);

            Fight.Send(new GameActionFightSpellCastMessage()
            {
                actionId = (short)ActionsEnum.ACTION_FIGHT_CAST_SPELL,
                critical = (byte)handler.Cast.Critical,
                destinationCellId = handler.Cast.TargetCell.Id,
                portalsIds = new short[0],
                silentCast = handler.Cast.Silent,
                sourceId = this.Id,
                spellId = handler.Cast.Spell.Record.Id,
                spellLevel = handler.Cast.Spell.Level.Grade,
                targetId = target == null ? 0 : target.Id,
                verboseCast = true,
            });



        }

        public virtual FightSpellCastCriticalEnum RollCriticalDice(SpellLevelRecord spell)
        {
            FightSpellCastCriticalEnum critical = FightSpellCastCriticalEnum.NORMAL;

            if (HasRandDownModifier())
            {
                return critical;
            }

            var random = new AsyncRandom();

            if (spell.CriticalHitProbability != 0 && random.NextDouble() * 100 < spell.CriticalHitProbability + Stats.CriticalHit.TotalInContext())
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            return critical;
        }

        protected virtual void OnSpellCastFailed(SpellCast cast)
        {
            Fight.Send(new GameActionFightNoSpellCastMessage(cast.Spell.Record.Id == 0 ? 0 : (int)cast.Spell.Level.Id));
        }

        [WIP]
        public virtual SpellCastResult CanCastSpell(SpellCast cast)
        {
            if (cast.Force)
                return SpellCastResult.OK;

            if (!cast.IsConditionBypassed(SpellCastResult.CANNOT_PLAY) && (!IsFighterTurn || !Alive))
            {
                return SpellCastResult.CANNOT_PLAY;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.HAS_NOT_SPELL) && !HasSpell(cast.Spell.Record.Id))
            {
                return SpellCastResult.HAS_NOT_SPELL;
            }

            var spellLevel = cast.Spell.Level;

            if (!cast.IsConditionBypassed(SpellCastResult.UNWALKABLE_CELL) && (!cast.TargetCell.Walkable || cast.TargetCell.NonWalkableDuringFight))
            {
                return SpellCastResult.UNWALKABLE_CELL;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.NOT_ENOUGH_AP) && (this.Stats.ActionPoints.TotalInContext() < GetApCost(spellLevel)))
            {
                return SpellCastResult.NOT_ENOUGH_AP;
            }

            var cellfree = Fight.IsCellFree(cast.TargetCell);

            if (!cast.IsConditionBypassed(SpellCastResult.CELL_NOT_FREE) &&
                ((spellLevel.NeedFreeCell && !cellfree) || (spellLevel.NeedTakenCell && cellfree)))
            {
                return SpellCastResult.CELL_NOT_FREE;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.STATE_FORBIDDEN) && spellLevel.StatesForbidden.Any(HasState))
            {
                return SpellCastResult.STATE_FORBIDDEN;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.STATE_REQUIRED) &&
                spellLevel.StatesRequired.Any(state => !HasState(state)))
            {
                return SpellCastResult.STATE_REQUIRED;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.NOT_IN_ZONE) &&
                !IsInCastZone(spellLevel, cast.CastCell.Point, cast.TargetCell.Point))
            {
                return SpellCastResult.NOT_IN_ZONE;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.HISTORY_ERROR) &&
                   !SpellHistory.CanCastSpell(spellLevel, cast.TargetCell))
            {
                return SpellCastResult.HISTORY_ERROR;
            }

            if (!cast.IsConditionBypassed(SpellCastResult.NO_LOS) &&
                (cast.Spell.Level.CastTestLos && !Fight.CanBeSeen(cast.Source.Cell.Point, cast.TargetCell.Point)))
            {
                return SpellCastResult.NO_LOS;
            }

            return SpellCastResult.OK;
        }

        public bool IsInCastZone(SpellLevelRecord spellLevel, MapPoint castCell, MapPoint cell)
        {
            var range = (int)spellLevel.MaxRange;
            Set set;

            if (spellLevel.RangeCanBeBoosted)
            {
                range += Stats.Range.TotalInContext();

                if (range < spellLevel.MinRange)
                    range = (int)spellLevel.MinRange;

                range = Math.Min(range, 63);
            }

            if (spellLevel.CastInDiagonal || spellLevel.CastInLine)
            {
                set = new CrossSet(castCell, (short)range, spellLevel.MinRange)
                {
                    AllDirections = spellLevel.CastInDiagonal && spellLevel.CastInLine,
                    Diagonal = spellLevel.CastInDiagonal
                };
            }
            else
            {
                set = new LozengeSet(castCell, range, (int)spellLevel.MinRange);
            }

            return set.BelongToSet(cell);
        }

        public virtual bool HasSpell(short spellId)
        {
            return false;
        }

        public bool HasState(int stateId)
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.Id == stateId);
        }

        public Telefrag Teleport(Fighter source, CellRecord targetCell, bool register = true)
        {
            if (CantBeMoved())
            {
                return null;
            }
            Fighter otherTarget = Fight.GetFighter(targetCell.Id);

            if (otherTarget != null && otherTarget != this)
            {
                this.SwitchPosition(otherTarget, register);
                return new Telefrag(this, otherTarget);
            }

            if (!Fight.IsCellFree(targetCell))
            {
                return null;
            }

            var msg = new GameActionFightTeleportOnSameMapMessage()
            {
                actionId = (short)ActionsEnum.ACTION_CHARACTER_TELEPORT_ON_SAME_MAP,
                cellId = targetCell.Id,
                sourceId = source.Id,
                targetId = Id,
            };

            if (Stats.InvisibilityState == GameActionFightInvisibilityStateEnum.INVISIBLE)
            {
                Team.Send(msg);
            }
            else
            {
                Fight.Send(msg);
            }

            var oldCell = Cell;

            this.Cell = targetCell;

            if (register)
                MovementHistory.OnCellChanged(oldCell);

            OnMove(source, false);

            return null;
        }

        public void PushBack(Fighter source, CellRecord castCell, short delta, CellRecord targetCell)
        {
            DirectionsEnum direction = 0;

            if (targetCell.Id == Cell.Id)
            {
                bool diagonal = targetCell.Point.IsOnSameDiagonal(castCell.Point);
                direction = castCell.Point.OrientationTo(targetCell.Point, diagonal);
            }
            else
            {
                bool diagonal = Cell.Point.IsOnSameDiagonal(targetCell.Point);
                direction = targetCell.Point.OrientationTo(Cell.Point, diagonal);
            }

            Slide(source, direction, delta, true);
        }
        [WIP("useless. Put this in the Spell Effect handler.")]
        public void Advance(Fighter source, short delta, CellRecord targetCell)
        {
            bool diagonal = this.Cell.Point.IsOnSameDiagonal(targetCell.Point);
            DirectionsEnum direction = this.Cell.Point.OrientationTo(targetCell.Point, diagonal);
            source.Slide(source, direction, delta, false);
        }
        [WIP("useless. Put this in the Spell Effect handler.")]
        public void Retreat(Fighter source, short delta, CellRecord targetCell)
        {
            bool diagonal = this.Cell.Point.IsOnSameDiagonal(targetCell.Point);
            DirectionsEnum direction = targetCell.Point.OrientationTo(this.Cell.Point, diagonal);
            source.Slide(source, direction, delta, true);
        }
        public void PullForward(Fighter source, CellRecord castCell, short delta, CellRecord targetCell)
        {
            DirectionsEnum direction = 0;

            if (targetCell.Id == Cell.Id)
            {
                if (targetCell.Id == castCell.Id)
                    return;

                bool diagonal = targetCell.Point.IsOnSameDiagonal(castCell.Point);
                direction = targetCell.Point.OrientationTo(castCell.Point, diagonal);
            }
            else
            {
                if (Cell.Id == targetCell.Id)
                    return;

                bool diagonal = Cell.Point.IsOnSameDiagonal(targetCell.Point);
                direction = Cell.Point.OrientationTo(targetCell.Point, diagonal);
            }

            this.Slide(source, direction, delta, false);
        }
        private void InflictPushDamages(Fighter source, int n, bool headOn)
        {
            double num1 = headOn ? 4 : 8d;
            double num2 = ((source.Level / 2d) + (source.Stats.PushDamageBonus.TotalInContext() - this.Stats.PushDamageReduction.TotalInContext()) + 32d)
                 * (n / (double)num1);

            short delta = (short)num2;

            this.InflictDamage(new Damage(source, this, EffectSchoolEnum.Pushback, delta, delta, null));
        }
        public void UpdateLook(Fighter source)
        {
            ServerEntityLook finalLook = null;

            LookBuff lookBuff = GetBuffs<LookBuff>().LastOrDefault();

            double rescaleValue = GetBuffs<RescaleSkinBuff>().Sum(x => x.Delta);

            if (lookBuff != null)
            {
                finalLook = lookBuff.Look;
            }
            else
            {
                finalLook = BaseLook.Clone();
            }

            finalLook.Rescale(1 + rescaleValue);
            this.Look = finalLook;

            this.Fight.Send(new GameActionFightChangeLookMessage()
            {
                actionId = (short)ActionsEnum.ACTION_CHARACTER_CHANGE_LOOK,
                entityLook = Look.ToEntityLook(),
                sourceId = source.Id,
                targetId = Id,
            });
        }
        public void OnMove(Fighter source, bool isMapMovement)
        {
            if (!isMapMovement) // not a teleportation / slide / swap
            {
                this.TriggerBuffs(BuffTriggerType.OnMoved, source);
            }

            Fight.TriggerMarks(this, MarkTriggerType.OnMove);
            Moved?.Invoke(this);
        }
        public void SetSpellCooldown(Fighter source, short spellId, short value)
        {
            SpellHistory.SetSpellCooldown(spellId, value);

            Fight.Send(new GameActionFightSpellCooldownVariationMessage()
            {
                actionId = (short)ActionsEnum.ACTION_CHARACTER_ADD_SPELL_COOLDOWN,
                sourceId = source.Id,
                spellId = spellId,
                targetId = Id,
                value = value,
            });

            OnSpellCooldownChanged(source, ActionsEnum.ACTION_CHARACTER_REMOVE_SPELL_COOLDOWN, spellId, value);
        }

        public abstract Spell GetSpell(short spellId);

        public void ReduceSpellCooldown(Fighter source, short spellId, short delta)
        {
            var newCooldown = SpellHistory.ReduceSpellCooldown(spellId, delta);
            OnSpellCooldownChanged(source, ActionsEnum.ACTION_CHARACTER_REMOVE_SPELL_COOLDOWN, spellId, (short)newCooldown);
        }
        private void OnSpellCooldownChanged(Fighter source, ActionsEnum action, short spellId, short delta)
        {
            Fight.Send(new GameActionFightSpellCooldownVariationMessage()
            {
                actionId = (short)action,
                sourceId = source.Id,
                spellId = spellId,
                targetId = Id,
                value = delta,
            });
        }
        public bool HasRandDownModifier()
        {
            return Buffs.OfType<RandModifierBuff>().Any(x => !x.Up);
        }
        public bool HasRandUpModifier()
        {
            return Buffs.OfType<RandModifierBuff>().Any(x => x.Up);
        }
        public short GetApCost(SpellLevelRecord level)
        {
            if (m_spellsCosts.ContainsKey(level.SpellId))
            {
                return m_spellsCosts[level.SpellId];
            }
            else
            {
                return level.ApCost;
            }
        }
        public void ReduceApCost(SpellLevelRecord spellLevel, short delta)
        {
            int newCost = 0;

            if (this.m_spellsCosts.ContainsKey(spellLevel.SpellId))
            {
                newCost = m_spellsCosts[spellLevel.SpellId] - delta;

                if (newCost >= 0 && newCost != spellLevel.ApCost)
                {
                    m_spellsCosts[spellLevel.SpellId] = (short)newCost;
                }
                else
                {
                    m_spellsCosts.Remove(spellLevel.SpellId);
                }

            }
            else
            {
                newCost = spellLevel.ApCost - delta;

                if (newCost >= 0 && newCost != spellLevel.ApCost)
                {
                    m_spellsCosts.Add(spellLevel.SpellId, (short)newCost);
                }
            }
        }
        public void Slide(Fighter source, DirectionsEnum direction, short delta, bool isPush)
        {
            if (CantBeMoved())
            {
                return;
            }
            if (isPush && CantBePushed())
            {
                return;
            }
            MapPoint destinationPoint = Cell.Point;

            for (int i = 0; i < delta; i++)
            {
                MapPoint oldPoint = destinationPoint;
                MapPoint targetPoint = destinationPoint.GetCellInDirection(direction, 1);

                if (targetPoint != null && Fight.IsCellFree(targetPoint.CellId))
                {
                    destinationPoint = targetPoint;

                    if (Fight.ShouldTriggerOnMove(oldPoint.CellId, targetPoint.CellId))
                    {
                        break;
                    }

                }
                else
                {
                    if (isPush)
                    {
                        InflictPushDamages(source, delta - i, true);
                        MapPoint next = destinationPoint.GetNearestCellInDirection(direction);

                        if (next != null)
                        {
                            Fighter nextFighter = Fight.GetFighter(next.CellId);

                            if (nextFighter != null)
                            {
                                nextFighter.InflictPushDamages(source, delta - i, false);
                            }
                        }
                    }
                    break;
                }
            }

            if (destinationPoint.CellId == Cell.Id)
            {
                return;
            }

            using (Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE))
            {
                var msg = new GameActionFightSlideMessage()
                {
                    actionId = 6,
                    endCellId = destinationPoint.CellId,
                    sourceId = source.Id,
                    targetId = Id,
                    startCellId = this.Cell.Id,
                };

                if (Stats.InvisibilityState == GameActionFightInvisibilityStateEnum.INVISIBLE)
                {
                    Team.Send(msg);
                }
                else
                {
                    Fight.Send(msg);
                }

            }

            var oldCell = this.Cell;

            this.Cell = Fight.Map.GetCell(destinationPoint);

            OnMove(source, false);

            MovementHistory.OnCellChanged(oldCell);
        }
        [WIP("teleport triggered")]
        public void SwitchPosition(Fighter source, bool register = true)
        {
            if (CantSwitchPosition() || CantBeMoved())
                return;

            CellRecord cell = this.Cell;
            this.Cell = source.Cell;
            source.Cell = cell;

            Fight.Send(new GameActionFightExchangePositionsMessage()
            {
                actionId = 0,
                casterCellId = source.Cell.Id,
                sourceId = source.Id,
                targetCellId = this.Cell.Id,
                targetId = Id,
            });

            if (register)
            {
                source.MovementHistory.RegisterEntry(this.Cell);
                MovementHistory.RegisterEntry(cell);
            }
            else
            {
                source.MovementHistory.RegisterEntry(this.Cell);
            }

            OnMove(source, false);

        }
        public void SetInvisiblityState(GameActionFightInvisibilityStateEnum state, Fighter source)
        {
            GameActionFightInvisibilityStateEnum oldState = this.Stats.InvisibilityState;
            this.Stats.InvisibilityState = state;
            OnInvisibilityStateChanged(state, oldState, source);
        }
        private void OnInvisibilityStateChanged(GameActionFightInvisibilityStateEnum state, GameActionFightInvisibilityStateEnum oldState, Fighter source)
        {
            foreach (var fighter in Fight.GetFighters<CharacterFighter>(false))
            {
                fighter.Send(new GameActionFightInvisibilityMessage()
                {
                    actionId = (short)ActionsEnum.ACTION_CHARACTER_MAKE_INVISIBLE,
                    sourceId = source.Id,
                    targetId = Id,
                    state = (byte)GetInvisibilityStateFor(fighter),
                });
            }

            if (oldState == GameActionFightInvisibilityStateEnum.INVISIBLE && state == GameActionFightInvisibilityStateEnum.VISIBLE)
            {
                ShowFighter();
            }
        }
        public GameActionFightInvisibilityStateEnum GetInvisibilityStateFor(Fighter fighter)
        {
            if (fighter.IsFriendlyWith(this) && this.Stats.InvisibilityState != GameActionFightInvisibilityStateEnum.VISIBLE)
            {
                return GameActionFightInvisibilityStateEnum.DETECTED;
            }
            else
            {
                return Stats.InvisibilityState;
            }
        }
        public bool BlockLOS()
        {
            return Stats.InvisibilityState != GameActionFightInvisibilityStateEnum.INVISIBLE;
        }
        private void OnDetected()
        {
            this.EnemyTeam.Send(new GameActionFightInvisibleDetectedMessage()
            {
                actionId = (short)ActionsEnum.ACTION_CHARACTER_MAKE_INVISIBLE,
                cellId = Cell.Id,
                sourceId = Id,
                targetId = Id,
            });
        }

        public abstract void Kick(Fighter source);

        public abstract GameFightFighterInformations GetFightFighterInformations(CharacterFighter to);

        public abstract FightTeamMemberInformations GetFightTeamMemberInformations();

        public virtual void OnFightStarted()
        {
            this.FightStartCell = this.Cell;
        }

        public short[] GetPreviousPositions()
        {
            return MovementHistory.GetEntries(2).Select(x => (short)x.Cell.Id).ToArray();
        }

        public void DispelState(Fighter source, int stateId)
        {
            foreach (var buff in GetBuffs<StateBuff>().Where(x => x.Record.Id == stateId).ToArray())
            {
                RemoveAndDispellBuff(buff);
            }
        }
        public bool CantTackle()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.CantTackle);
        }
        public bool CantBeTackled()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.CantBeTackled);
        }
        public bool CantDealDamages()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.CantDealDamage);
        }
        public bool IsInvulnerableMelee()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.InvulnerableMelee);
        }
        public bool IsInvulnerableRange()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.InvulnerableRange);
        }
        public bool IsInvulnerable()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.Invulnerable);
        }
        public bool CantBeMoved()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.CantBeMoved);
        }
        public bool CantSwitchPosition()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.CantSwitchPosition);
        }
        public bool CantBePushed()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.CantBePushed);
        }
        public bool IsIncurable()
        {
            return GetBuffs<StateBuff>().Any(x => x.Record.Incurable);
        }
        public void Heal(Healing healing)
        {
            if (healing.Delta <= 0 || IsIncurable())
            {
                return;
            }

            int delta = healing.Delta;

            if (Stats.LifePoints + healing.Delta > Stats.MaxLifePoints)
            {
                delta = Stats.MaxLifePoints - Stats.LifePoints;
            }

            Stats.LifePoints += delta;

            Fight.Send(new GameActionFightLifePointsGainMessage()
            {
                actionId = (short)ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_WIN,
                delta = delta,
                sourceId = healing.Source.Id,
                targetId = Id,
            });


        }
        private void DispellShieldBuffs(int amount)
        {
            short num = (short)amount;

            foreach (var buff in GetBuffs<ShieldBuff>().ToArray())
            {
                buff.Delta -= num;

                if (buff.Delta <= 0)
                {
                    num = (short)(-buff.Delta);
                    RemoveBuff(buff);
                }
                else
                {
                    UpdateBuff(this, buff);
                    break;
                }
            }
        }

        private void UpdateBuff(Fighter source, Buff buff)
        {
            Fight.Send(new GameActionFightDispellableEffectMessage()
            {
                actionId = (short)ActionsEnum.ACTION_CHARACTER_UPDATE_BOOST,
                effect = buff.GetAbstractFightDispellableEffect(),
                sourceId = source.Id,
            });
        }

        [WIP("shield loss not working.")]
        public DamageResult InflictDamage(Damage damage)
        {
            if (IsInvulnerable() || damage.Source.CantDealDamages())
            {
                return DamageResult.Zero();
            }
            if (IsInvulnerableMelee() && damage.Source.IsMeleeWith(this))
            {
                return DamageResult.Zero();
            }
            if (IsInvulnerableRange() && !damage.Source.IsMeleeWith(this))
            {
                return DamageResult.Zero();
            }

            damage.Compute();

            int delta = damage.Computed.Value;

            if (delta <= 0 || (!Alive))
            {
                return DamageResult.Zero();
            }

            this.LastAttacker = damage.Source;

            TriggerBuffs(damage);

            delta = damage.Computed.Value;

            if (delta <= 0 || (!Alive))
            {
                return DamageResult.Zero();
            }

            int lifeLoss = 0;

            int shieldLoss = 0;

            int permanentDamages = CalculateErodedLife(delta);

            if (Stats.ShieldPoints > 0)
            {
                if (Stats.ShieldPoints - delta <= 0)
                {
                    int num = delta - Stats.ShieldPoints; // effective life loose
                    permanentDamages = CalculateErodedLife(num);

                    Fight.Send(new GameActionFightLifeAndShieldPointsLostMessage()
                    {
                        actionId = 0,
                        elementId = 0,
                        loss = num,
                        shieldLoss = (short)Stats.ShieldPoints,
                        permanentDamages = permanentDamages,
                        sourceId = damage.Source.Id,
                        targetId = this.Id,
                    });

                    lifeLoss = num;
                    shieldLoss = Stats.ShieldPoints;

                    Stats.SetShield(0);

                    if (Stats.LifePoints - num <= 0)
                    {
                        lifeLoss = Stats.LifePoints;
                        Stats.LifePoints = 0;
                    }
                    else
                    {
                        Stats.MaxLifePoints -= permanentDamages;
                        Stats.LifePoints -= num;

                    }

                    DispellShieldBuffs(shieldLoss);
                }
                else
                {
                    Fight.Send(new GameActionFightLifeAndShieldPointsLostMessage()
                    {
                        actionId = 0,
                        elementId = 0,
                        loss = 0,
                        permanentDamages = 0,
                        shieldLoss = (short)delta,
                        sourceId = damage.Source.Id,
                        targetId = this.Id,
                    });

                    permanentDamages = 0;

                    shieldLoss = delta;

                    Stats.RemoveShield(delta);

                    DispellShieldBuffs(shieldLoss);
                }
            }
            else
            {
                if (Stats.LifePoints - delta <= 0)
                {
                    Fight.Send(new GameActionFightLifePointsLostMessage()
                    {
                        sourceId = damage.Source.Id,
                        targetId = this.Id,
                        actionId = 0,
                        elementId = 0,
                        loss = Stats.LifePoints,
                        permanentDamages = 0,
                    });

                    lifeLoss = (short)Stats.LifePoints;
                    Stats.LifePoints = 0;


                }
                else
                {

                    Stats.MaxLifePoints -= permanentDamages;
                    Stats.LifePoints -= delta;
                    lifeLoss = delta;
                    Fight.Send(new GameActionFightLifePointsLostMessage()
                    {
                        actionId = 0,
                        elementId = 0,
                        loss = delta,
                        permanentDamages = permanentDamages,
                        sourceId = damage.Source.Id,
                        targetId = this.Id,
                    });
                }
            }

            TriggerBuffs(BuffTriggerType.AfterDamagd, damage);


            return new DamageResult(lifeLoss, permanentDamages, shieldLoss);
        }



        private void TriggerBuffs(Damage damage)
        {
            TriggerBuffs(BuffTriggerType.OnDamaged, damage);

            switch (damage.EffectSchool)
            {
                case EffectSchoolEnum.Pushback:
                    TriggerBuffs(BuffTriggerType.OnDamagedByPush, damage);
                    break;
                case EffectSchoolEnum.Neutral:
                    TriggerBuffs(BuffTriggerType.OnDamagedNeutral, damage);
                    break;
                case EffectSchoolEnum.Earth:
                    TriggerBuffs(BuffTriggerType.OnDamagedEarth, damage);
                    break;
                case EffectSchoolEnum.Water:
                    TriggerBuffs(BuffTriggerType.OnDamagedWater, damage);
                    break;
                case EffectSchoolEnum.Air:
                    TriggerBuffs(BuffTriggerType.OnDamagedAir, damage);
                    break;
                case EffectSchoolEnum.Fire:
                    TriggerBuffs(BuffTriggerType.OnDamagedFire, damage);
                    break;
            }
            if (damage.Source.IsMeleeWith(this))
            {
                TriggerBuffs(BuffTriggerType.OnDamagedInCloseRange, damage);
            }
            else
            {
                TriggerBuffs(BuffTriggerType.OnDamagedInLongRange, damage);
            }

            if (damage.IsSpellDamage())
            {
                TriggerBuffs(BuffTriggerType.OnDamagedBySpell, damage);
            }

            if (damage.Source.IsFriendlyWith(this))
            {
                TriggerBuffs(BuffTriggerType.OnDamagedByAlly, damage);
            }
            else
            {
                TriggerBuffs(BuffTriggerType.OnDamagedByEnemy, damage);
            }
        }
        private int CalculateErodedLife(int damages)
        {
            var num = Stats.Erosion;

            if (num > 50)
            {
                num = 50;
            }
            return (int)(damages * (num / 100.0d));
        }
        public bool IsMeleeWith(Fighter fighter)
        {
            return this.Cell.Point.ManhattanDistanceTo(fighter.Cell.Point) == 1;
        }
        public bool IsMeleeWith(MapPoint point)
        {
            return this.Cell.Point.ManhattanDistanceTo(point) == 1;
        }
        public IEnumerable<Fighter> GetMeleeFighters()
        {
            return Fight.GetFighters<Fighter>(x => x.IsMeleeWith(this));
        }
        public void KillAllSummons()
        {
            foreach (var summon in Fight.GetFighters<Fighter>().Where(x => x.IsSummoned() && x.GetSummoner() == this).ToArray())
            {
                summon.Die(this);
            }
        }
        public void RemoveAndDispellAllBuffs(Fighter source, FightDispellableEnum dispellable = FightDispellableEnum.REALLY_NOT_DISPELLABLE)
        {
            foreach (var buff in Buffs.Where(x => x.Cast.Source == source && x.Dispellable <= dispellable).ToArray())
            {
                RemoveAndDispellBuff(buff);
            }
        }
        public void RemoveAllCastedBuffs()
        {
            foreach (Fighter current in this.Fight.GetFighters<Fighter>())
            {
                current.RemoveAndDispellAllBuffs(this);
            }
        }
        public IEnumerable<T> GetMarks<T>() where T : Mark
        {
            return GetMarks().OfType<T>();
        }
        public IEnumerable<Glyph> GetEffectiveGlyphs()
        {
            return Fight.GetMarks().OfType<Glyph>().Where(x => x.ContainsCell(Cell.Id));
        }
        public IEnumerable<Mark> GetMarks()
        {
            var marks = Fight.GetMarks().Where(x => x.Source == this);
            return marks;
        }
        public void RemoveMarks()
        {
            foreach (var mark in GetMarks().ToArray())
            {
                Fight.RemoveMark(mark);
            }
        }

        [WIP("do we past turn here... ?")]
        public void Die(Fighter killedBy)
        {
            if (Alive)
            {
                KillAllSummons();
                RemoveAllCastedBuffs();
                this.RemoveMarks();

                this.DeathTime = DateTime.Now;

                Fight.Send(new GameActionFightDeathMessage()
                {
                    actionId = (short)ActionsEnum.ACTION_CHARACTER_DEATH,
                    sourceId = killedBy.Id,
                    targetId = this.Id,
                });

                this.Alive = false;

                TriggerBuffs(BuffTriggerType.OnDeath, killedBy);
            }
            else
            {
                Logger.Write("Cannot kill " + this + ", he is already dead!", MessageState.WARNING);
            }

        }
        public void OnDodge(Fighter source, ActionsEnum action, int delta)
        {
            Fight.Send(new GameActionFightDodgePointLossMessage()
            {
                actionId = (short)action,
                amount = (short)delta,
                sourceId = source.Id,
                targetId = Id,
            });
        }
        public virtual bool RollMPLose(Fighter from, short value)
        {
            var mpAttack = from.Stats.MPAttack.TotalInContext() > 1 ? from.Stats.MPAttack.TotalInContext() : 1;
            var mpDodge = Stats.DodgePMProbability.TotalInContext() > 1 ? Stats.DodgePMProbability.TotalInContext() : 1;
            var prob = ((Stats.MovementPoints.TotalInContext() - value) / (double)(Stats.MovementPoints.TotalInContext())) * (mpAttack / (double)mpDodge) / 2d;

            if (prob < 0.10)
                prob = 0.10;
            else if (prob > 0.90)
                prob = 0.90 - (0.10 * value);

            var rnd = new AsyncRandom().NextDouble();

            return rnd < prob;
        }
        public virtual bool RollAPLose(Fighter from, int value)
        {
            var apAttack = from.Stats.APAttack.TotalInContext() > 1 ? from.Stats.APAttack.TotalInContext() : 1;
            var apDodge = Stats.DodgePAProbability.TotalInContext() > 1 ? Stats.DodgePAProbability.TotalInContext() : 1;
            var prob = ((Stats.ActionPoints.TotalInContext() - value) / (double)(Stats.ActionPoints.TotalInContext())) * (apAttack / (double)apDodge) / 2d;

            if (prob < 0.10)
                prob = 0.10;
            else if (prob > 0.90)
                prob = 0.90;

            var rnd = new AsyncRandom().NextDouble();

            return rnd < prob;
        }
        public int CalculateArmorValue(short reduction)
        {
            return (int)(reduction * (100 + 5 * Level) / 100d);
        }

        public void OnDamageReduced(Damage damage, int dmgReduction)
        {
            Fight.Send(new GameActionFightReduceDamagesMessage()
            {
                actionId = 105,
                amount = dmgReduction,
                sourceId = damage.Source.Id,
                targetId = damage.Target.Id,
            });
        }
        public virtual bool IsSummoned()
        {
            return false;
        }
        public virtual SpellEffectHandler GetSummoningEffect()
        {
            return null;
        }
        public virtual Fighter GetSummoner()
        {
            return null;
        }
        public virtual Fighter GetController()
        {
            return null;
        }
        public virtual bool MustSkipTurn()
        {
            return (!Alive) || Buffs.OfType<SkipTurnBuff>().Any();
        }
        public override string ToString()
        {
            return $"{Id} : {Name}";
        }

        public Fighter GetSource()
        {
            return this;
        }


    }

}