using Giny.Core.DesignPattern;
using Giny.Core.Network.Messages;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Actions;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.History;
using Giny.World.Managers.Fights.Results;
using Giny.World.Managers.Fights.Sequences;
using Giny.World.Managers.Fights.Stats;
using Giny.World.Managers.Fights.Synchronisation;
using Giny.World.Managers.Items;
using Giny.World.Managers.Spells;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using Giny.World.Records.Spells;
using System.Collections.Generic;
using System.Linq;

namespace Giny.World.Managers.Fights.Fighters
{
    public class CharacterFighter : Fighter
    {
        public Character Character
        {
            get;
            private set;
        }

        public override short Level => Character.SafeLevel;


        private bool Disconnected
        {
            get;
            set;
        }


        private int LeftRound
        {
            get;
            set;
        }
        public override string Name => Character.Name;

        public Synchronizer PersonalSynchronizer
        {
            get;
            set;
        }
        private WeaponRecord WeaponRecord
        {
            get;
            set;
        }
        private SpellLevelRecord WeaponLevel
        {
            get;
            set;
        }
        private bool HasWeapon
        {
            get
            {
                return WeaponRecord != null;
            }
        }

        public override bool Sex => Character.Record.Sex;

        public CharacterFighter(Character character, FightTeam team, CellRecord roleplayCell) : base(team, roleplayCell)
        {
            this.Character = character;
            this.Left = false;
        }
        public override void Initialize()
        {
            this.Id = (int)Character.Id;
            this.Look = Character.Look.Clone();
            this.Stats = new FighterStats(Character);
            if (Character.Inventory.HasWeaponEquiped)
            {
                this.WeaponRecord = WeaponRecord.GetWeapon(Character.Inventory.GetWeapon().GId);
                this.WeaponLevel = WeaponManager.Instance.CreateWeaponSpellLevel(WeaponRecord, Character.Inventory.GetWeapon());
            }

            base.Initialize();
        }


        public void NoMove()
        {
            this.Send(new GameMapNoMovementMessage((short)Cell.Point.X, (short)Cell.Point.Y));

        }
        public override void OnFightStarted()
        {
            base.OnFightStarted();

            SummonedFighter summon = GetNextControlableSummon();

            if (summon != null)
            {
                summon.SwitchContext();
            }

            foreach (var item in Character.Inventory.GetSpellCastItems())
            {
                EffectDice effect = item.GetEffect<EffectDice>(Inventory.ItemCastEffect);
                SpellRecord record = SpellRecord.GetSpellRecord((short)effect.Min);
                Spell spell = new Spell(record, record.GetLevel((byte)effect.Max));
                SpellCast cast = new SpellCast(this, spell, this.Cell);
                cast.Force = true;
                this.CastSpell(cast);
            }
        }
        public override void OnJoined()
        {
            this.Fight.SendGameFightJoinMessage(this);
            this.ShowPlacementCells();
            this.Fight.ShowFighters(this);
            this.ShowReadyFighters();
            base.OnJoined();
        }
        public void ShowReadyFighters()
        {
            Fight.OnFighters((CharacterFighter fighter) =>
            {
                if (fighter.IsReady)
                {
                    Character.Client.Send(new GameFightHumanReadyStateMessage(fighter.Id, true));
                }
            });
        }
        public void ShowPlacementCells()
        {
            this.Send(new GameFightPlacementPossiblePositionsMessage(Fight.RedTeam.PlacementCells.Select(x => x.Id).ToArray(), Fight.BlueTeam.PlacementCells.Select(x => x.Id).ToArray(), (byte)Team.TeamId));
        }

        public virtual bool IsCompanion()
        {
            return false;
        }
        public override bool CastSpell(short spellId, short cellId)
        {
            if (IsFighterTurn)
            {
                return base.CastSpell(spellId, cellId);
            }
            else if (Fight.FighterPlaying.GetController() == this)
            {
                return Fight.FighterPlaying.CastSpell(spellId, cellId);
            }
            else
            {
                return false;
            }
        }
        public override bool CastSpell(SpellCast cast)
        {
            if (cast.SpellId == WeaponManager.PunchSpellId)
            {
                SpellLevelRecord level = null;

                if (HasWeapon)
                {
                    level = WeaponLevel;
                }
                else
                {
                    level = Character.GetSpell(WeaponManager.PunchSpellId).ActiveSpellRecord.Levels.Last();
                }
                return CloseCombat(cast.TargetCell, level);
            }
            else
            {
                return base.CastSpell(cast);

            }
        }

        private bool CloseCombat(CellRecord targetCell, SpellLevelRecord weaponSpellLevel)
        {
            if (Fight.Ended)
            {
                return false;
            }

            SpellCast cast = new SpellCast(this, new Spell(WeaponManager.Instance.PunchSpellRecord, weaponSpellLevel), targetCell);
            cast.Weapon = true;

            SpellCastResult canCast = CanCastSpell(cast);

            if (canCast != SpellCastResult.OK)
            {
                OnSpellCastFailed(cast);
                return false;
            }

            short weaponGenericId = (short)(HasWeapon ? WeaponRecord.Id : 0);


            using (Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_WEAPON))
            {
                cast.Critical = RollCriticalDice(cast.Spell.Level);

                SpellCastHandler handler = new DefaultSpellCastHandler(cast);

                if (!handler.Initialize())
                {
                    OnSpellCastFailed(cast);
                    return false;
                }

                Fight.Send(new GameActionFightCloseCombatMessage()
                {
                    actionId = (short)ActionsEnum.ACTION_FIGHT_CLOSE_COMBAT,
                    critical = (byte)cast.Critical,
                    silentCast = false,
                    sourceId = this.Id,
                    targetId = 0,
                    destinationCellId = targetCell.Id,
                    verboseCast = true,
                    weaponGenericId = weaponGenericId,
                });



                if (!cast.ApFree)
                    LooseAp(this, cast.Spell.Level.ApCost, ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_USE);

                if (!handler.Execute())
                {
                    Fight.Warn("Unable to cast spell : " + cast.Spell.Record.Name);
                }

                OnSpellCasted(handler);
            }
            Fight.CheckDeads();
            Fight.CheckFightEnd();

            return true;
        }

        [WIP]
        public override GameFightFighterInformations GetFightFighterInformations(CharacterFighter target)
        {
            return new GameFightCharacterInformations()
            {
                contextualId = Id,
                disposition = GetEntityDispositionInformations(),
                look = Look.ToEntityLook(),
                previousPositions = GetPreviousPositions(),
                wave = 0,
                spawnInfo = new GameContextBasicSpawnInformation()
                {
                    alive = Alive,
                    informations = new GameContextActorPositionInformations(Id, GetEntityDispositionInformations()),
                    teamId = (byte)Team.TeamId,
                },

                stats = Stats.GetFightMinimalStats(this, target),
                alignmentInfos = new ActorAlignmentInformations(),//todo
                breed = Character.Breed.Id,
                hiddenInPrefight = false,
                ladderPosition = 0,
                leagueId = 0,
                level = Level,
                name = Character.Name,
                sex = Character.Record.Sex,
                status = Character.GetPlayerStatus(),
            };
        }
        public override void OnMoveFailed(MovementFailedReason reason)
        {
            if (reason == MovementFailedReason.Obstacle)
            {
                Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 276);
            }

            this.NoMove();
        }
        public void ToggleReady(bool isReady)
        {
            this.IsReady = isReady;
            Fight.OnSetReady(this, IsReady);
        }

        public void Send(NetworkMessage message)
        {
            Character.Client.Send(message);
        }
        public override bool HasSpell(short spellId)
        {
            return Character.HasSpell(spellId);
        }


        public void Leave(bool teleportToSpawn)
        {
            if (!Fight.Started)
            {
                Team.RemoveFighter(this);

                if (!Fight.CheckFightEnd())
                {
                    Fight.CheckFightStart();
                }

                if (teleportToSpawn)
                    Character.RejoinMap(Character.Record.MapId, Fight.FightType, false, Fight.SpawnJoin);
                else
                    Character.RejoinMap(Character.Record.MapId, Fight.FightType, false, false);

            }
            else
            {

                if (!Left)
                {
                    if (Alive)
                    {
                        this.Stats.LifePoints = 0;
                        this.Fight.CheckDeads();
                    }

                    if (!Fight.Ended)
                    {
                        Synchronizer sync = new Synchronizer(this.Fight, new CharacterFighter[]
                    {
                           this
                    }, Fight.SynchronizerTimout * 1000);

                        sync.Success += delegate (Synchronizer obj)
                        {
                            this.OnPlayerReadyToLeave();
                        };
                        sync.Timeout += delegate (Synchronizer obj, CharacterFighter[] laggers)
                        {
                            this.OnPlayerReadyToLeave();
                        };
                        this.PersonalSynchronizer = sync;
                        sync.Start();
                    }

                    this.Left = true;


                }

            }
        }


        public void OnPlayerReadyToLeave()
        {
            this.PersonalSynchronizer = null;

            if (this.Fight != null && !this.Fight.CheckFightEnd())
            {
                this.Team.RemoveFighter(this);
                this.Team.AddLeaver(this);
                if (IsFighterTurn)
                    this.PassTurn();

                this.Character.RejoinMap(Character.Record.MapId, Fight.FightType, false, Fight.SpawnJoin);
            }
        }

        public void ToggleTurnReady(bool ready)
        {
            if (PersonalSynchronizer != null)
                PersonalSynchronizer.ToggleReady(this, ready);
            else if (Fight.Synchronizer != null)
                Fight.Synchronizer.ToggleReady(this, ready);
        }

        public override void OnTurnBegin()
        {
            // nothing todo
        }
        public override void OnTurnEnded()
        {
            SummonedFighter summon = GetNextControlableSummon();

            if (summon != null)
            {
                summon.SwitchContext();
            }
        }
        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberCharacterInformations()
            {
                id = Id,
                level = Level,
                name = Character.Name,
            };
        }


        [WIP("end this. (care about sending message to disconnected clients)")]
        public void OnDisconnected()
        {
            Character.Record.FightId = this.Fight.Id;

            this.EnterDisconnectedState();

            if (!Fight.CheckFightEnd() && IsFighterTurn)
                Fight.StopTurn();

            this.Team.AddLeaver(this);

            this.Fight.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 182, this.Name, Fight.TurnBeforeDisconnection);
        }

        private void EnterDisconnectedState()
        {
            Disconnected = true;
            LeftRound = Fight.RoundNumber;
        }

        public override bool MustSkipTurn()
        {
            return base.MustSkipTurn() || Disconnected;
        }
        public override IFightResult GetFightResult()
        {
            return new FightPlayerResult(this, base.GetFighterOutcome(), this.Loot);
        }

        public override void Kick(Fighter source)
        {
            if (source.Team.Leader == source && source.Team == this.Team)
            {
                Leave(false);
            }
        }
        public SummonedFighter GetNextControlableSummon()
        {
            for (int index = Fight.Timeline.Index; index < Fight.Timeline.Fighters.Count; index++)
            {
                Fighter fighter = Fight.Timeline.Fighters[index];

                if (fighter.GetController() == this && fighter.Alive)
                {
                    return (SummonedFighter)fighter;
                }
            }
            for (int index = 0; index < Fight.Timeline.Index; index++)
            {
                Fighter fighter = Fight.Timeline.Fighters[index];

                if (fighter.GetController() == this && fighter.Alive)
                {
                    return (SummonedFighter)fighter;
                }
            }

            return null;
        }
        public override void PassTurn()
        {
            if (Fight.FighterPlaying.GetController() == this)
            {
                Fight.FighterPlaying.PassTurn();
            }
            else if (IsFighterTurn)
            {
                base.PassTurn();
            }
        }
        public override void Move(List<CellRecord> path)
        {
            if (Fight.FighterPlaying.GetController() == this)
            {
                Fight.FighterPlaying.Move(path);
            }
            else if (IsFighterTurn)
            {
                base.Move(path);
            }
        }

        public override Spell GetSpell(short spellId)
        {
            CharacterSpell characterSpell = Character.GetSpell(spellId);
            SpellRecord record = characterSpell.ActiveSpellRecord;
            SpellLevelRecord level = record.GetLevel(characterSpell.GetGrade(Character));
            return new Spell(record, level);
        }
    }
}
