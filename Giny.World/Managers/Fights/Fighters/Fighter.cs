using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Pool;
using Giny.Core.Time;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Handlers.Roleplay.Maps.Paths;
using Giny.World.Managers.Actions;
using Giny.World.Managers.Entities.Look;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Cast.History;
using Giny.World.Managers.Fights.Cast.Units;
using Giny.World.Managers.Fights.Effects.Damages;
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

        public virtual void OnMoveFailed()
        {

        }

        public void Move(List<CellRecord> path)
        {
            if (Fight.Ended || !Fight.Started)
                return;

            if (!path.Skip(1).All(x => Fight.IsCellFree(x)))
            {
                this.OnMoveFailed();
                return;
            }



            for (int i = 1; i < path.Count; i++)
            {
                if (Fight.ShouldTriggerOnMove(path[i].Id))
                {
                    if (i + 1 <= path.Count)
                    {
                        path.RemoveRange(i + 1, path.Count - i - 1);
                    }
                    break;
                }
            }

            DirectionsEnum direction = (DirectionsEnum)PathReader.GetDirection(path.Last().Id);

            short mpCost = Cell.Point.ManhattanDistanceTo(new MapPoint(path.Last().Id));

            if (mpCost <= Stats.MovementPoints.TotalInContext() && mpCost > 0)
            {
                using (Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE))
                {

                    /*   var cell = path[0];
                       short tackledMp = 0;
                       short tackledAp = 0;
                       var mp = this.Stats.MovementPoints.TotalInContext();
                       var ap = this.Stats.ActionPoints.TotalInContext();
                       List<Fighter> tacklers = new List<Fighter>();

                       foreach (var fighter in GetNearFighters<Fighter>())
                       {
                           if ((tackledMp = (short)this.GetTackledMP(mp, cell)) > 0)
                           {
                               if (tackledMp > mp)
                                   tackledMp += mp;

                               mp -= tackledMp;

                               tacklers.Add(fighter);

                           }

                           if ((tackledAp = (short)this.GetTackledAP(ap, cell)) > 0)
                           {
                               if (tackledAp > ap)
                                   tackledAp += ap;

                               ap -= tackledAp;

                               if (!tacklers.Contains(fighter))
                               {
                                   tacklers.Add(fighter);
                               }

                           }
                       }

                       if (tackledMp > 0 || tackledAp > 0)
                       {
                           this.OnTackled(tackledMp, tackledAp, tacklers.ToArray());
                       } */


                    if (Stats.MovementPoints.TotalInContext() > 0)
                    {
                        if (path.Count() > 0)
                        {
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
                            this.LooseMp(this, mpCost);

                            Fight.TriggerMarks(this, MarkTriggerType.OnMove);
                            /*   TriggerBuffs(TriggerType.AFTER_MOVE, mpCost);
                               TriggerMarks(this, MarkTriggerTypeEnum.AFTER_MOVE, mpCost); */
                        }
                    }
                }
            }
            else
            {
                this.OnMoveFailed();
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
        public void LooseMp(Fighter source, short amount)
        {
            Stats.UseMp(amount);
            Fight.PointsVariation(source.Id, Id, ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE, (short)(-amount));
        }
        public void LooseAp(Fighter source, short amount)
        {
            Stats.UseAp(amount);
            Fight.PointsVariation(source.Id, Id, ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE, (short)(-amount));
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
        public void PassTurn()
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
            return (short)Buffs.OfType<SpellBoostBuff>().Where(x => x.BoostedSpellId == spellId).Sum(x => x.Delta); // Sum, or we pick first one ?
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
                summon.DecrementBuffsDelay();
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
            return Buffs.OfType<T>().Count() > 0;
        }

        public abstract void OnTurnBegin();

        public void DecrementBuffsDelay()
        {
            foreach (var buff in Buffs.OfType<TriggerBuff>().Where(x => x.HasDelay()).ToArray())
            {
                if (buff.DecrementDelay())
                {
                    buff.Apply();

                    RemoveBuff(buff);
                }
            }
        }
        public void RemoveAndDispellBuff(Buff buff)
        {
            this.RemoveBuff(buff);
            buff.Dispell();
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
                RemoveBuff(oldBuff);
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

            foreach (var buff in Buffs.OfType<TriggerBuff>().Where(x => x.TriggerType == type).ToArray())
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
            Fight.Send(new GameActionFightDispellableEffectMessage()
            {
                actionId = buff.GetActionId(),
                effect = buff.GetAbstractFightDispellableEffect(),
                sourceId = buff.Cast.Source.Id,
            }); ;
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
            return current.Cast.SpellId == reference.Cast.SpellId &&
                 current.Effect.EffectId == reference.Effect.EffectId && current.Effect.Delay == reference.Effect.Delay
                 && current.GetTriggerType() == reference.GetTriggerType() && current.GetType().Name == reference.GetType().Name;
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
        }
        public void ShowFighter()
        {
            Fight.Send(new GameFightShowFighterMessage(GetFightFighterInformations()));
        }
        public void ShowFighter(CharacterFighter fighter)
        {
            fighter.Character.Client.Send(new GameFightShowFighterMessage(GetFightFighterInformations()));
        }

        public EntityDispositionInformations GetEntityDispositionInformations()
        {
            return new EntityDispositionInformations()
            {
                cellId = (short)Cell.Id,
                direction = (byte)Direction,
                //carryingCharacterId = 0, // todo
            };
        }

        public void BeforeTurnEnd()
        {
            using (Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END))
            {
                if (Alive)
                {
                    TriggerBuffs(BuffTriggerType.OnTurnEnd, null);
                }
            }
        }

        public IdentifiedEntityDispositionInformations GetIdentifiedEntityDispositionInformations()
        {
            return new IdentifiedEntityDispositionInformations()
            {
                cellId = Cell.Id,
                direction = (byte)Direction,
                id = Id,
            };
        }


        public void CastSpell(short spellId, short cellId)
        {
            Spell spell = GetSpell(spellId);
            CellRecord cell = Fight.Map.GetCell(cellId);
            this.CastSpell(new SpellCast(this, spell, Fight.Map.GetCell(cellId)));
        }

        public virtual bool CastSpell(SpellCast cast)
        {
            if (Fight.Ended)
                return false;

            if (!cast.Force && (!IsFighterTurn || !Alive))
                return false;

            var result = CanCastSpell(cast);

            if (result != SpellCastResult.OK)
            {
                OnSpellCastFailed(cast);
                return false;
            }

            using (Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL))
            {
                cast.Critical = RollCriticalDice(cast.Spell.Level);

                SpellCastHandler handler = SpellManager.Instance.GetSpellCastHandler(cast);

                if (!handler.Initialize())
                {
                    OnSpellCastFailed(cast);
                    return false;
                }

                if (!cast.SilentNetwork)
                    OnSpellCasting(cast);

                if (!cast.ApFree)
                    LooseAp(this, GetApCost(cast.Spell.Level));

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

        protected virtual void OnSpellCasted(SpellCastHandler handler)
        {
            this.SpellHistory.RegisterCastedSpell(handler.Cast.Spell.Level, this.Fight.GetFighter(handler.Cast.TargetCell.Id));
        }

        [WIP(WIPState.Todo, "see stump")]
        private void OnSpellCasting(SpellCast cast)
        {
            Fighter target = Fight.GetFighter(cast.TargetCell.Id);

            Fight.Send(new GameActionFightSpellCastMessage()
            {
                actionId = (short)ActionsEnum.ACTION_FIGHT_CAST_SPELL,
                critical = (byte)cast.Critical,
                destinationCellId = cast.TargetCell.Id,
                portalsIds = new short[0],
                silentCast = false,//cast.Silent,
                sourceId = this.Id,
                spellId = cast.Spell.Record.Id,
                spellLevel = cast.Spell.Level.Grade,
                targetId = target == null ? 0 : target.Id,
                verboseCast = true,
            });
        }

        public virtual FightSpellCastCriticalEnum RollCriticalDice(SpellLevelRecord spell)
        {
            var random = new AsyncRandom();

            var critical = FightSpellCastCriticalEnum.NORMAL;

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
            return Buffs.OfType<StateBuff>().Any(x => x.Record.Id == stateId);
        }

        public void Teleport(Fighter source, CellRecord targetCell)
        {
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

            source.Cell = targetCell;
        }
        public void PushBack(Fighter source, CellRecord castCell, short delta, CellRecord targetCell)
        {
            DirectionsEnum direction = 0;

            if (targetCell.Id == Cell.Id)
            {
                direction = castCell.Point.OrientationTo(targetCell.Point, false);
            }
            else
            {
                direction = targetCell.Point.OrientationTo(Cell.Point, false);
            }

            Slide(source, direction, delta, true);
        }
        [WIP("useless. Put this in the Spell Effect handler.")]
        public void Advance(Fighter source, short delta, CellRecord targetCell)
        {
            DirectionsEnum direction = this.Cell.Point.OrientationTo(targetCell.Point);
            source.Slide(source, direction, delta, false);
        }
        [WIP("useless. Put this in the Spell Effect handler.")]
        public void Retreat(Fighter source, short delta, CellRecord targetCell)
        {
            DirectionsEnum direction = targetCell.Point.OrientationTo(this.Cell.Point);
            source.Slide(source, direction, delta, true);
        }
        public void PullForward(Fighter source, CellRecord castCell, short delta, CellRecord targetCell)
        {
            if (this.Cell == castCell)
            {
                return;
            }
            DirectionsEnum direction = 0;

            if (targetCell.Id == Cell.Id)
            {
                direction = targetCell.Point.OrientationTo(castCell.Point, false);
            }
            else
            {
                direction = Cell.Point.OrientationTo(targetCell.Point, false);
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
        public void ChangeLook(ServerEntityLook look, Fighter source)
        {
            this.Look = look;

            this.Fight.Send(new GameActionFightChangeLookMessage()
            {
                actionId = (short)ActionsEnum.ACTION_CHARACTER_CHANGE_LOOK,
                entityLook = look.ToEntityLook(),
                sourceId = source.Id,
                targetId = Id,
            });
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
                MapPoint targetPoint = destinationPoint.GetCellInDirection(direction, 1);

                if (targetPoint != null && Fight.IsCellFree(targetPoint.CellId))
                {
                    destinationPoint = targetPoint;

                    if (Fight.ShouldTriggerOnMove(targetPoint.CellId))
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

            this.Cell = Fight.Map.GetCell(destinationPoint);

            Fight.TriggerMarks(this, MarkTriggerType.OnMove);

        }
        [WIP("teleport triggered")]
        public void SwitchPosition(Fighter source)
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
        private void OnDetected(Fighter source)
        {
            this.EnemyTeam.Send(new GameActionFightInvisibleDetectedMessage()
            {
                actionId = 0,
                cellId = Cell.Id,
                sourceId = source.Id,
                targetId = Id,
            });
        }

        public abstract void Kick(Fighter source);

        public abstract GameFightFighterInformations GetFightFighterInformations();

        public abstract FightTeamMemberInformations GetFightTeamMemberInformations();

        public virtual void OnFightStarted()
        {
            this.FightStartCell = this.Cell;
        }

        public void DispelState(Fighter source, int stateId)
        {
            foreach (var buff in Buffs.OfType<StateBuff>().Where(x => x.Record.Id == stateId).ToArray())
            {
                RemoveBuff(buff);
            }
        }
        public bool IsInvulnerableMelee()
        {
            return Buffs.OfType<StateBuff>().Any(x => x.Record.InvulnerableMelee);
        }
        public bool IsInvulnerableRange()
        {
            return Buffs.OfType<StateBuff>().Any(x => x.Record.InvulnerableRange);
        }
        public bool IsInvulnerable()
        {
            return Buffs.OfType<StateBuff>().Any(x => x.Record.Invulnerable);
        }
        public bool CantBeMoved()
        {
            return Buffs.OfType<StateBuff>().Any(x => x.Record.CantBeMoved);
        }
        public bool CantSwitchPosition()
        {
            return Buffs.OfType<StateBuff>().Any(x => x.Record.CantSwitchPosition);
        }
        public bool CantBePushed()
        {
            return Buffs.OfType<StateBuff>().Any(x => x.Record.CantBePushed);
        }
        public bool IsIncurable()
        {
            return Buffs.OfType<StateBuff>().Any(x => x.Record.Incurable);
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

            Fight.Send(new GameActionFightLifePointsGainMessage()
            {
                actionId = 0,
                delta = delta,
                sourceId = healing.Source.Id,
                targetId = Id,
            });

        }
        private void DispellShieldBuffs(int amount)
        {
            short num = (short)amount;

            foreach (var buff in Buffs.OfType<ShieldBuff>().ToArray())
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
            if (IsInvulnerable())
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

            if (delta <= 0)
            {
                return DamageResult.Zero();
            }

            TriggerBuffs(BuffTriggerType.BeforeDamaged, damage);

            delta = damage.Computed.Value;

            if (!Alive)
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

            this.LastAttacker = damage.Source;

            if (this.Alive)
            {
                TriggerBuffs(BuffTriggerType.OnDamaged, damage);

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
            }

            return new DamageResult(lifeLoss, permanentDamages, shieldLoss);
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
        public IEnumerable<Fighter> GetMeleeFighters()
        {
            return Fight.GetFighters<Fighter>(x => x.IsMeleeWith(this));
        }
        public void Die(Fighter killedBy, bool recusiveCall = false)
        {
            if (Alive)
            {
                /* TriggerBuffs(TriggerType.BEFORE_DEATH);
                 this.KillSummons(killedBy);
                 this.RemoveAndDispellAllBuffs();
                 this.DropCarried();
                 this.RemoveAllCastedBuffs();
                 this.RemoveMarks(); */

                this.DeathTime = DateTime.Now;

                //  BeforeDeadEvt?.Invoke(this);
                Fight.Send(new GameActionFightDeathMessage()
                {
                    actionId = (short)ActionsEnum.ACTION_CHARACTER_DEATH,
                    sourceId = killedBy.Id,
                    targetId = this.Id,
                });
                // TriggerBuffs(TriggerType.AFTER_DEATH);

                this.Alive = false;

                //  AfterDeadEvt?.Invoke(this, recusiveCall);
            }
            else
            {
                Logger.Write("Cannot kill " + this + ", he is already dead!", MessageState.WARNING);
            }

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
        protected virtual bool MustSkipTurn()
        {
            return false;
        }
        public override string ToString()
        {
            return $"{Id} : {Name}";
        }

    }
}

