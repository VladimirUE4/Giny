using Giny.Core;
using Giny.Core.Network.Messages;
using Giny.Core.Pool;
using Giny.Core.Time;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Api;
using Giny.World.Managers.Actions;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Marks;
using Giny.World.Managers.Fights.Results;
using Giny.World.Managers.Fights.Sequences;
using Giny.World.Managers.Fights.Synchronisation;
using Giny.World.Managers.Fights.Timeline;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Maps.Shapes;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights
{
    public abstract class Fight
    {
        public const int TurnTime = 30;

        public const int SynchronizerTimout = 8;

        public const int TurnBeforeDisconnection = 20;

        private ReversedUniqueIdProvider m_contextualIdPopper = new ReversedUniqueIdProvider(0);

        private UniqueIdProvider m_markIdPopper = new UniqueIdProvider(0);

        public int Id
        {
            get;
            private set;
        }
        public long? TargetMapId
        {
            get;
            set;
        }
        public FightTeam BlueTeam
        {
            get;
            private set;
        }
        public abstract FightTypeEnum FightType
        {
            get;
        }
        public FightTeam RedTeam
        {
            get;
            private set;
        }
        public MapRecord Map
        {
            get;
            private set;
        }
        public bool Started
        {
            get;
            private set;
        }
        public CellRecord Cell
        {
            get;
            private set;
        }
        public DateTime CreationTime
        {
            get;
            private set;
        }
        public Synchronizer Synchronizer
        {
            get;
            private set;
        }

        public DateTime? StartTime
        {
            get;
            private set;
        }
        public abstract bool ShowBlades
        {
            get;
        }
        public abstract bool SpawnJoin
        {
            get;
        }

        public Fighter FighterPlaying
        {
            get
            {
                return this.Timeline.Current;
            }
        }

        protected ActionTimer m_placementTimer
        {
            get;
            set;
        }

        public FightTimeline Timeline
        {
            get;
            private set;
        }
        public int RoundNumber
        {
            get
            {
                return Timeline.RoundNumber;
            }
        }
        public bool Ended
        {
            get;
            private set;
        }

        public SequenceManager SequenceManager
        {
            get;
            private set;
        }
        public FightTeam Winners
        {
            get;
            private set;
        }

        private ActionTimer m_turnTimer;

        private List<Mark> Marks
        {
            get;
            set;
        }
        #region Events

        public event Action<Fight, Fighter> TurnStarted;

        #endregion


        public void Send(NetworkMessage message)
        {
            BlueTeam.Send(message);
            RedTeam.Send(message);
        }
        public void OnFighters<T>(Action<T> action, bool aliveOnly = true) where T : Fighter
        {
            BlueTeam.OnFighters(action, aliveOnly);
            RedTeam.OnFighters(action, aliveOnly);
        }

        public abstract void OnFighterJoined(Fighter fighter);

        public Fighter GetFighter(short cellId)
        {
            Fighter target = BlueTeam.GetFighter<Fighter>(x => x.Cell.Id == cellId);
            return target == null ? RedTeam.GetFighter<Fighter>(x => x.Cell.Id == cellId) : target;
        }
        public Fighter GetFighter<T>(Func<T, bool> predicate) where T : Fighter
        {
            T result = BlueTeam.GetFighter(predicate);

            if (result == null)
            {
                result = RedTeam.GetFighter(predicate);
            }
            return result;
        }
        public List<Fighter> GetFighters(bool aliveOnly = true)
        {
            List<Fighter> fighters = new List<Fighter>();
            fighters.AddRange(RedTeam.GetFighters<Fighter>(aliveOnly));
            fighters.AddRange(BlueTeam.GetFighters<Fighter>(aliveOnly));
            return fighters;
        }
        public IEnumerable<Fighter> GetFighters(IEnumerable<CellRecord> cells)
        {
            return GetFighters().Where(x => cells.Contains(x.Cell));
        }
        public IEnumerable<T> GetFighters<T>(bool aliveOnly = true)
        {
            return GetFighters(aliveOnly).OfType<T>();
        }
        public IEnumerable<T> GetFighters<T>(Func<T, bool> predicate, bool aliveOnly = true)
        {
            return GetFighters<T>(aliveOnly).Where(predicate);
        }
        public Fight(int id, MapRecord map, FightTeam blueTeam, FightTeam redTeam, CellRecord cell)
        {
            this.Id = id;
            this.Map = map;
            this.BlueTeam = blueTeam;
            this.RedTeam = redTeam;
            this.BlueTeam.Fight = this;
            this.RedTeam.Fight = this;
            this.Timeline = new FightTimeline(this);
            this.Cell = cell;
            this.Started = false;
            this.CreationTime = DateTime.Now;
            this.SequenceManager = new SequenceManager(this);
            this.Synchronizer = null;
            this.Marks = new List<Mark>();

            if (map.IsDungeonMap)
            {
                this.TargetMapId = map.DungeonMap.NextMapId;
            }
        }


        public virtual void OnSetReady(Fighter fighter, bool isReady)
        {
            this.Send(new GameFightHumanReadyStateMessage(fighter.Id, isReady));
            this.CheckFightStart();

        }
        public void UpdateTeams()
        {
            if (!Started)
            {
                BlueTeam.Update();
                RedTeam.Update();
            }
        }
        public FightTeam[] GetTeams()
        {
            return new FightTeam[] { BlueTeam, RedTeam };
        }
        public FightTeam GetTeam(TeamTypeEnum teamType)
        {
            if (BlueTeam.Type == teamType)
                return BlueTeam;
            if (RedTeam.Type == teamType)
                return RedTeam;

            throw new Exception("Unable to find team (" + teamType + ")");
        }
        public void UpdateFightersPlacementDirection()
        {
            OnFighters<Fighter>(x => x.FindPlacementDirection());
        }
        public void ShowFighters(CharacterFighter fighter)
        {
            OnFighters<Fighter>(target => target.ShowFighter(fighter));
        }
        public short GetPlacementTimeLeft()
        {
            double num = GetPlacementDelay() - (DateTime.Now - this.CreationTime).TotalSeconds;
            if (num < 0.0)
            {
                num = 0.0;
            }
            return (short)(num * 10d);
        }
        public void UpdateEntitiesPositions()
        {
            List<IdentifiedEntityDispositionInformations> positions = new List<IdentifiedEntityDispositionInformations>();

            foreach (var fighter in GetFighters())
            {
                positions.Add(fighter.GetIdentifiedEntityDispositionInformations());
            }
            this.Send(new GameEntitiesDispositionMessage(positions.ToArray()));
        }
        public virtual int GetPlacementDelay()
        {
            return 0;
        }

        public void StartPlacement()
        {
            if (GetPlacementDelay() > 0)
            {
                this.m_placementTimer = new ActionTimer(GetPlacementDelay() * 1000, StartFighting, false);
                this.m_placementTimer.Start();
            }

            if (ShowBlades)
            {
                ShowBladesOnMap();
                this.Send(new IdolFightPreparationUpdateMessage(0, new Idol[0]));
            }

            FightApi.PlacementStarted(this);
        }
        private void ShowBladesOnMap()
        {
            this.FindBladesPlacement();
            this.Map.Instance.AddFight(this);
        }
        private void FindBladesPlacement()
        {
            if (this.RedTeam.Leader.RoleplayCell.Id != this.BlueTeam.Leader.RoleplayCell.Id)
            {
                this.RedTeam.BladesCell = MapsManager.Instance.SecureRoleplayCell(this.Map, this.RedTeam.Leader.RoleplayCell);
                this.BlueTeam.BladesCell = MapsManager.Instance.SecureRoleplayCell(this.Map, this.BlueTeam.Leader.RoleplayCell);

            }
            else
            {
                this.BlueTeam.BladesCell = this.BlueTeam.Leader.RoleplayCell;

                CellRecord target = MapsManager.Instance.GetNearFreeCell(Map, this.BlueTeam.Leader.RoleplayCell);

                if (target == null)
                {
                    this.RedTeam.BladesCell = this.Map.RandomWalkableCell();
                }
                else
                {
                    this.RedTeam.BladesCell = Map.GetCell(target.Id);
                }

            }
        }
        public bool IsCellFree(CellRecord cell)
        {
            return cell.Walkable && !cell.NonWalkableDuringFight && GetFighter(cell.Id) == null;
        }
        public bool IsCellFree(short cellId)
        {
            return IsCellFree(Map.Cells[cellId]);
        }
        public int PopNextContextualId()
        {
            return m_contextualIdPopper.Pop();
        }
        public int PopNextMarkId()
        {
            return m_markIdPopper.Pop();
        }
        public virtual void StartFighting()
        {
            if (GetPlacementDelay() > 0)
            {
                this.m_placementTimer.Dispose();
                this.m_placementTimer = null;
            }

            this.StartTime = DateTime.Now;

            this.Started = true;

            UpdateEntitiesPositions();

            this.Map.Instance.RemoveBlades(this);

            this.Timeline.OrderLine();

            this.Send(GetGameFightStartMessage());

            this.Synchronize();

            this.UpdateTimeLine();

            this.OnFightStarted();

        }
        public virtual void OnFightStarted()
        {
            foreach (var fighter in GetFighters())
            {
                fighter.OnFightStarted();
            }

            StartTurn();

        }

        private void StartTurn()
        {
            if (Started && !Ended && !CheckFightEnd())
            {
                this.OnTurnStarted();
            }
        }

        private void OnTurnStarted()
        {
            SequenceManager.ResetSequences();

            if (Timeline.NewRound)
            {
                UpdateRound();
            }

            this.Send(new GameFightTurnStartMessage(this.FighterPlaying.Id, Fight.TurnTime * 10));

            FighterPlaying.TurnStartCell = FighterPlaying.Cell;

            using (SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_START))
            {

                if (!FighterPlaying.IsSummoned() && RoundNumber > 1)
                {
                    FighterPlaying.DecrementAllCastedBuffsDuration();
                    FighterPlaying.DecrementSummonsCastedBuffsDuration();
                    FighterPlaying.DecrementSummonsCastedBuffsDelays();
                    FighterPlaying.DecrementAllCastedBuffsDelay();
                }

                this.DecrementGlyphDuration(FighterPlaying);
                FighterPlaying.TriggerBuffs(BuffTriggerType.OnTurnBegin, null);
                this.TriggerMarks(FighterPlaying, MarkTriggerType.OnTurnBegin);

            }

            CheckDeads();

            Synchronize();

            if (CheckFightEnd())
            {
                return;
            }
            if (!FighterPlaying.Alive)
            {
                PassTurn();
                return;
            }

            this.m_turnTimer = new ActionTimer((int)Fight.TurnTime * 1000, StopTurn, false);
            this.m_turnTimer.Start();

            FighterPlaying.OnTurnBegin();

            TurnStarted?.Invoke(this, FighterPlaying);
        }


        public void PointsVariation(int sourceId, int targetId, ActionsEnum action, short delta)
        {
            this.Send(new GameActionFightPointsVariationMessage()
            {
                actionId = (short)action,
                delta = delta,
                sourceId = sourceId,
                targetId = targetId,
            });
        }
        public void CheckDeads()
        {
            IEnumerable<Fighter> deads = GetFighters().Where(x => x.Alive == true && x.Stats.LifePoints <= 0);

            if (deads.Count() > 0)
            {
                Fighter current = null;

                using (SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH))
                {
                    foreach (var fighter in deads)
                    {
                        fighter.Die(fighter);

                        if (fighter.IsFighterTurn)
                        {
                            current = fighter;
                        }
                    }


                }

                if (current != null)
                {
                    FighterPlaying.PassTurn();
                }

            }
        }

        public void StopTurn()
        {

            if (Ended)
                return;

            if (m_turnTimer != null)
                m_turnTimer.Dispose();

            if (Synchronizer != null)
            {
                this.Reply("Last ReadyChecker was not disposed. (Stop Turn)", Color.Red);
                Synchronizer.Cancel();
                Synchronizer = null;
            }

            if (CheckFightEnd())
                return;

            OnTurnStopped();
            Synchronizer = Synchronizer.RequestCheck(this, PassTurnAndCheck, LagAndPassTurn, SynchronizerTimout * 1000);

        }
        protected void PassTurnAndCheck()
        {

            if (Synchronizer == null)
                return;

            Synchronizer = null;

            FighterPlaying.Stats.ResetUsedPoints();
            PassTurn();
        }
        public void Warn(string message)
        {
            Reply(message, Color.Orange);
        }
        public void Reply(string message, Color color)
        {
            foreach (var fighter in GetFighters<CharacterFighter>(false))
            {
                fighter.Character.Reply(message, color);
            }
        }
        public void TextInformation(TextInformationTypeEnum type, short messageId, params object[] parameters)
        {
            foreach (var fighter in GetFighters<CharacterFighter>(false))
            {
                fighter.Character.TextInformation(type, messageId, parameters);
            }
        }

        protected void PassTurn()
        {
            if (Ended)
                return;

            if (CheckFightEnd())
                return;

            if (!Timeline.SelectNextFighter())
            {
                if (!CheckFightEnd())
                {
                    this.Reply("Something goes wrong : no more actors are available to play but the fight is not ended", Color.Red);
                }

                return;
            }

            // player left but is disconnected
            // pass turn is there are others players
            /*    if (FighterPlaying.HasLeft() && FighterPlaying is CharacterFighter)
                {
                    var leaver = (CharacterFighter)FighterPlaying;
                    if (leaver.IsDisconnected &&
                        leaver.LeftRound + FightConfiguration.TurnsBeforeDisconnection <= TimeLine.RoundNumber)
                    {
                        leaver.Die();

                        if (CheckFightEnd())
                            return;

                        var results = GenerateLeaverResults(leaver, out var leaverResult);

                        leaverResult.Apply();

                        ContextHandler.SendGameFightLeaveMessage(Clients, leaver);

                        leaver.ResetFightProperties();

                        leaver.Team.AddLeaver(leaver);
                        m_leavers.Add(leaver);
                        leaver.Team.RemoveFighter(leaver);

                        leaver.LeaveDisconnectedState(false);

                        leaver.Character.RejoinMap();
                        leaver.Character.SaveLater();

                        goto redo;
                    }

                    // <b>%1</b> vient d'être déconnecté, il quittera la partie dans <b>%2</b> tour(s) s'il ne se reconnecte pas avant.
                    BasicHandler.SendTextInformationMessage(Clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 182,
                        FighterPlaying.GetMapRunningFighterName(), leaver.LeftRound + FightConfiguration.TurnsBeforeDisconnection - TimeLine.RoundNumber);
                } */

            OnTurnPassed();

            StartTurn();
        }
        public void TriggerMarks(Fighter target, MarkTriggerType triggerType)
        {
            foreach (var mark in Marks.Where(x => x.Triggers.HasFlag(triggerType) && x.ContainsCell(target.Cell.Id)).ToArray())
            {
                using (this.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP))
                {
                    mark.Trigger(target, triggerType);
                }
            }
        }
        public IEnumerable<Mark> GetMarks()
        {
            return this.Marks;
        }
        public bool MarkExist<T>(Func<T, bool> markExist) where T : Mark
        {
            return Marks.OfType<T>().Any(markExist);
        }
        public bool ShouldTriggerOnMove(short oldCell, short cellId)
        {
            bool flag1 = Marks.OfType<Glyph>().Any(x => x.StopMovement &&
            !x.ContainsCell(oldCell) && x.ContainsCell(cellId) || x.ContainsCell(oldCell) && !x.ContainsCell(cellId)) ;
            bool flag2 = Marks.OfType<Trap>().Any(x => x.StopMovement && x.ContainsCell(cellId));

            return flag1 || flag2;
        }
        private void OnTurnPassed()
        {
            // end sequence ?
        }
        private void LagAndPassTurn(CharacterFighter[] laggers)
        {
            if (Synchronizer == null)
                return;

            // some guys are lagging !
            OnLaggersSpotted(laggers);

            PassTurnAndCheck();
        }
        private void OnLaggersSpotted(CharacterFighter[] laggers)
        {
            if (laggers.Length == 1)
            {
                OnFighters<CharacterFighter>(x => x.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 28, laggers[0].Name));
            }
            else if (laggers.Length > 1)
            {
                OnFighters<CharacterFighter>(x => x.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 29, string.Join(",", laggers.Select(entry => entry.Name))));

            }
        }
        private void OnTurnStopped()
        {
            FighterPlaying.BeforeTurnEnd();

            CheckDeads();

            if (CheckFightEnd())
                return;

            if (SequenceManager.IsSequencing)
                SequenceManager.EndAllSequences();

            Send(new GameFightTurnEndMessage(FighterPlaying.Id));
        }

        public void AddMark(Mark mark)
        {
            this.Marks.Add(mark);

            foreach (var fighter in GetFighters<CharacterFighter>())
            {
                GameActionMark gameActionMark = null;

                if (mark.IsVisibleFor(fighter))
                {
                    gameActionMark = mark.GetGameActionMark();
                }
                else
                {
                    gameActionMark = mark.GetHiddenGameActionMark();
                }
                fighter.Send(new GameActionFightMarkCellsMessage()
                {
                    actionId = 0,
                    mark = gameActionMark,
                    sourceId = mark.Source.Id
                });
            }

            mark.OnAdded();
        }
        public void RemoveMark(Mark mark)
        {
            this.Marks.Remove(mark);

            this.Send(new GameActionFightUnmarkCellsMessage()
            {
                actionId = 0,
                markId = (short)mark.Id,
                sourceId = mark.Source.Id,
            });

            mark.OnRemoved();
        }
        private void DecrementGlyphDuration(Fighter fighterPlaying)
        {
            foreach (var glyph in fighterPlaying.GetMarks<Glyph>().ToArray())
            {
                if (glyph.DecrementDuration())
                {
                    RemoveMark(glyph);
                }
            }
        }


        public void AddSummon(Fighter source, SummonedFighter fighter)
        {
            AddSummons(source, new SummonedFighter[] { fighter });
        }
        public void AddSummons(Fighter source, IEnumerable<SummonedFighter> summons)
        {
            foreach (var summon in summons)
            {
                source.Team.AddFighter(summon);
                Timeline.InsertFighter(summon, Timeline.Index + 1);
                summon.Initialize();
            }

            foreach (var target in GetFighters<CharacterFighter>())
            {
                target.Send(new GameActionFightSummonMessage()
                {
                    actionId = 0,
                    sourceId = source.Id,
                    summons = summons.Select(x => x.GetFightFighterInformations(target)).ToArray(),
                });
            }


            this.UpdateTimeLine();
        }
        public void UpdateRound()
        {
            this.Send(new GameFightNewRoundMessage(Timeline.RoundNumber));
        }
        public void Synchronize()
        {
            foreach (var fighter in GetFighters<CharacterFighter>())
            {
                fighter.Send(new GameFightSynchronizeMessage(GetFighters().Select(x => x.GetFightFighterInformations(fighter)).ToArray()));
            }
        }
        public virtual GameFightStartMessage GetGameFightStartMessage()
        {
            return new GameFightStartMessage(new Idol[0]);
        }

        public void UpdateTimeLine()
        {
            double[] ids = this.Timeline.GetIds();
            this.Send(new GameFightTurnListMessage(ids, new double[0]));
        }
        public void CheckFightStart()
        {
            if (this.RedTeam.AreAllReady() && this.BlueTeam.AreAllReady())
            {
                this.StartFighting();
            }
        }
        public virtual bool CheckFightEnd()
        {
            if (BlueTeam.Alives == 0 || RedTeam.Alives == 0 && !Ended)
            {
                Ended = true;

                if (Started)
                {

                    if (Synchronizer != null)
                        Synchronizer.Cancel();

                    if (SequenceManager.IsSequencing)
                        SequenceManager.EndAllSequences();

                    Synchronizer = Synchronizer.RequestCheck(this, EndFight, delegate (CharacterFighter[] actors)
                    {
                        EndFight();
                    }, Fight.SynchronizerTimout * 1000);

                }
                else
                    EndFight();

            }
            return Ended;
        }
        public void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            fighter.Character.Client.Send(new GameFightJoinMessage(true, !Started, false, Started, GetPlacementTimeLeft(), (byte)FightType));
        }

        public abstract FightCommonInformations GetFightCommonInformations();

        public FightTeamInformations[] GetFightTeamInformations()
        {
            return new FightTeamInformations[2] {RedTeam.GetFightTeamInformations(),
                BlueTeam.GetFightTeamInformations()};
        }
        public FightOptionsInformations[] GetFightOptionsInformations()
        {
            return new FightOptionsInformations[]
            {
                    RedTeam.Options.GetFightOptionsInformations(),
                    BlueTeam.Options.GetFightOptionsInformations()
            };
        }

        private void DeterminsWinners()
        {
            if (this.BlueTeam.Alives == 0)
            {
                this.Winners = this.RedTeam;
            }
            else if (this.RedTeam.Alives == 0)
            {
                this.Winners = this.BlueTeam;
            }
        }

        public virtual void EndFight()
        {
            if (Started)
            {
                this.DeterminsWinners();

                this.Synchronizer = null;

                IEnumerable<IFightResult> results = this.GenerateResults();

                this.ApplyResults(results);

                this.Send(new GameFightEndMessage(GetFightDuration(), 1, 0, (from entry in results
                                                                             select entry.GetFightResultListEntry()).ToArray(),
                                                                                    new NamedPartyTeamWithOutcome[0]));
            }

            long targetMapId = TargetMapId.HasValue ? TargetMapId.Value : Map.Id;

            foreach (CharacterFighter current in this.GetFighters<CharacterFighter>(false))
            {
                bool winner = current.Team == Winners ? true : false;
                current.Character.RejoinMap(targetMapId, FightType, winner, SpawnJoin);
            }

            OnFightEnded();

            Dispose();

        }
        protected abstract IEnumerable<IFightResult> GenerateResults();

        protected void ApplyResults(IEnumerable<IFightResult> results)
        {
            foreach (IFightResult current in results)
            {
                current.Apply();
            }
        }
        public int GetFightDuration()
        {
            return (!this.Started) ? 0 : ((int)(System.DateTime.Now - this.StartTime.Value).TotalMilliseconds);
        }

        public void Join(Character character, double leaderId)
        {
            FightTeam joinedTeam;

            if (BlueTeam.Leader.Id == leaderId)
                joinedTeam = BlueTeam;
            else if (RedTeam.Leader.Id == leaderId)
                joinedTeam = RedTeam;
            else
            {
                character.ReplyError("Unable to find a team to join...");
                return;
            }

            if (joinedTeam.Options.CanJoin(character))
            {
                joinedTeam.AddFighter(character.CreateFighter(joinedTeam));
            }
        }
        public bool CanBeSeen(MapPoint from, MapPoint to, bool throughEntities = false, Fighter except = null)
        {
            if (from == null || to == null) return false;
            if (from == to)
                return true;

            var occupiedCells = new short[0];
            if (!throughEntities)
                occupiedCells = GetFighters<Fighter>().Where(x => x != except && x.BlockLOS()).Select(x => x.Cell.Id).ToArray();

            var line = new LineSet(from, to);
            return !(from point in line.EnumerateValidPoints().Skip(1)
                     where to.CellId != point.CellId
                     let cell = Map.Cells[point.CellId]
                     where !cell.LineOfSight || !throughEntities && Array.IndexOf(occupiedCells, point.CellId) != -1
                     select point).Any();
        }

        public abstract void OnFightEnded();

        public void Dispose()
        {
            if (m_placementTimer != null)
            {
                m_placementTimer.Dispose();
                m_placementTimer = null;
            }

            if (m_turnTimer != null)
            {
                m_turnTimer.Dispose();
                m_turnTimer = null;
            }

            if (Synchronizer != null)
            {
                Synchronizer.Cancel();
                Synchronizer = null;
            }

            this.RedTeam = null;
            this.BlueTeam = null;
            Map.Instance.RemoveFight(this);
            FightManager.Instance.RemoveFight(this);
        }

    }
}
