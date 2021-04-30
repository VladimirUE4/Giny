﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.Core.DesignPattern;
using Giny.Core.Time;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Monsters;
using Giny.World.Managers.Fights.Challenges;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Results;
using Giny.World.Managers.Formulas;
using Giny.World.Managers.Monsters;
using Giny.World.Records.Maps;

namespace Giny.World.Managers.Fights
{
    public class FightPvM : Fight
    {
        public override FightTypeEnum FightType => FightTypeEnum.FIGHT_TYPE_PvM;

        public override bool ShowBlades => true;

        public override bool SpawnJoin => true;

        private MonsterGroup MonsterGroup
        {
            get;
            set;
        }
        private List<Challenge> Challenges
        {
            get;
            set;
        }
        public FightPvM(int id, MapRecord map, FightTeam blueTeam, FightTeam redTeam, CellRecord cell, MonsterGroup monsterGroup)
            : base(id, map, blueTeam, redTeam, cell)
        {
            this.MonsterGroup = monsterGroup;

            if (map.IsDungeonMap && MonsterGroup is ModularMonsterGroup)
            {
                var nextMapId = map.GetNextRoomMapId();

                if (nextMapId.HasValue)
                {
                    this.TargetMapId = nextMapId;
                }
            }
        }
        public override FightCommonInformations GetFightCommonInformations()
        {
            return new FightCommonInformations((short)Id, (byte)FightType, GetFightTeamInformations(),
                new short[]
                {
                    BlueTeam.BladesCell.Id,RedTeam.BladesCell.Id
                }
                , GetFightOptionsInformations());
        }


        public override int GetPlacementDelay()
        {
            return 30;
        }
        /*
         * Before fight ended.
         */
        protected override void OnWinnersDetermined()
        {
            foreach (var challenge in Challenges)
            {
                challenge.OnWinnersDetermined();
            }
        }
        /*
         * When fight ended.
         */
        public override void OnFightEnded()
        {
            if (ShowBlades && Winners == GetTeam(TeamTypeEnum.TEAM_TYPE_MONSTER) && (this.Map.Instance.MonsterGroupCount < MonstersManager.MAX_MONSTER_GROUP_PER_MAP))
            {
                Map.Instance.AddEntity(MonsterGroup);
            }
        }
        public bool GroupExistOnMap()
        {
            return Map.Instance.MonsterGroupExists(this.MonsterGroup);
        }

        private double GetChallengesDropRatioBonus()
        {
            return Challenges.Where(x => x.Success).Sum(x => x.DropBonusRatio);
        }
        private double GetChallengesExpRatioBonus()
        {
            return Challenges.Where(x => x.Success).Sum(x => x.XpBonusRatio);
        }
        [WIP] // drop bonus , unit test.
        protected override IEnumerable<IFightResult> GenerateResults()
        {
            IEnumerable<IFightResult> results = GetFighters<Fighter>(false).Where(x => !x.IsSummoned()).Select(x => x.GetFightResult()).ToArray();

            foreach (var team in GetTeams())
            {
                double xpBonusPercent = 0d;
                double dropBonusPercent = 0d;

                if (team == GetTeamChallenged())
                {
                    xpBonusPercent += GetChallengesExpRatioBonus();
                    dropBonusPercent += GetChallengesDropRatioBonus();
                }

                IEnumerable<Fighter> droppers = team.EnemyTeam.GetFighters<Fighter>(false).Where(entry => !entry.Alive && entry.CanDrop);

                var looters = results.Where(x => x.CanLoot(team)).OrderByDescending(entry => entry.Prospecting);

                var teamPP = team.GetFighters<CharacterFighter>(false).Sum(entry => entry.Stats.Prospecting.TotalInContext());

                var kamas = Winners == team ? droppers.Sum(entry => entry.GetDroppedKamas()) * team.GetFighters<CharacterFighter>().Count() : 0;

                foreach (var looter in looters)
                {
                    if (looter is FightPlayerResult && looter.Outcome == FightOutcomeEnum.RESULT_VICTORY)
                    {
                        ((FightPlayerResult)looter).AddEarnedExperience(xpBonusPercent); // bonus ratio = challenges
                    }

                    if (team == Winners)
                    {
                        foreach (var item in droppers.SelectMany(dropper => dropper.RollLoot(looter)))
                        {
                            looter.Loot.AddItem(item);
                        }
                    }

                    looter.Loot.Kamas = teamPP > 0 ? FightFormulas.Instance.AdjustDroppedKamas(looter, teamPP, kamas) : 0;
                }
            }

            return results;
        }
        public override void OnFighterJoined(Fighter fighter)
        {
            if (!Started)
            {
                if (MonsterGroup is ModularMonsterGroup)
                {
                    FightTeam monsterTeam = this.GetTeam(TeamTypeEnum.TEAM_TYPE_MONSTER);

                    FightTeam playerTeam = monsterTeam.EnemyTeam;

                    if (fighter.Team == playerTeam)
                    {
                        var modularGroup = (ModularMonsterGroup)MonsterGroup;

                        foreach (var monsterFighter in modularGroup.CreateFighters(monsterTeam, monsterTeam.GetFightersCount(), playerTeam.GetFightersCount()))
                        {
                            monsterTeam.AddFighter(monsterFighter);
                        }
                    }
                }
            }

        }

        public override void OnFightStarted()
        {
            Challenges = ChallengesManager.Instance.CreateChallenges(GetTeamChallenged(), GetChallengeCount());
            DisplayChallenges();
        }
        private void DisplayChallenges()
        {
            FightTeam targetTeam = this.GetTeamChallenged();

            foreach (var challenge in this.Challenges)
            {
                targetTeam.Send(new ChallengeInfoMessage(challenge.Id, challenge.GetTargetId(), (int)(challenge.XpBonusRatio * 100d), (int)(challenge.DropBonusRatio * 100d)));
            }
        }
        private FightTeam GetTeamChallenged()
        {
            return RedTeam.Type == TeamTypeEnum.TEAM_TYPE_PLAYER ? RedTeam : BlueTeam;
        }
        private int GetChallengeCount()
        {
            FightTeam team = GetTeamChallenged();
            if (team.GetTeamLevel() >= team.EnemyTeam.GetTeamLevel())
                return 1;
            else
                return 2;
        }
    }
}
