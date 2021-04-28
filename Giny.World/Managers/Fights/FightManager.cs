using Giny.Core.DesignPattern;
using Giny.Protocol.Enums;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Monsters;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights
{
    public class FightManager : Singleton<FightManager>
    {
        private ConcurrentDictionary<int, Fight> Fights = new ConcurrentDictionary<int, Fight>();

        public Fight GetFight(int id)
        {
            Fight result = null;

            if (Fights.TryGetValue(id, out result))
            {
                return result;
            }
            else
                return null;
        }

        public void RemoveFight(Fight fight)
        {
            Fight result = null;
            Fights.TryRemove(fight.Id, out result);
        }

        public CharacterFighter GetConnectedFighter(Character character)
        {
            if (!Fights.ContainsKey(character.Record.FightId.Value))
            {
                return null;
            }

            Fight fight = Fights[character.Record.FightId.Value];
            return fight.GetFighter<CharacterFighter>(x => x.Id == character.Id);
        }

        public int PopId()
        {
            lock (this)
            {
                return Fights.Count == 0 ? 1 : Fights.Keys.Last() + 1;
            }
        }

        public FightPvM CreateFightPvM(MonsterGroup group, MapRecord map, CellRecord cell)
        {
            FightTeam blueTeam = new FightTeam(TeamEnum.TEAM_DEFENDER, map.BlueCells, AlignmentSideEnum.ALIGNMENT_WITHOUT, TeamTypeEnum.TEAM_TYPE_MONSTER);
            FightTeam redTeam = new FightTeam(TeamEnum.TEAM_CHALLENGER, map.RedCells, AlignmentSideEnum.ALIGNMENT_WITHOUT, TeamTypeEnum.TEAM_TYPE_PLAYER);

            var fight = new FightPvM(PopId(), map, blueTeam, redTeam, cell, group);
            Fights.TryAdd(fight.Id, fight);
            return fight;
        }

        public FightDual CreateFightDual(Character source, Character target, CellRecord cell)
        {
            FightTeam blueteam = new FightTeam(TeamEnum.TEAM_DEFENDER, source.Map.BlueCells, AlignmentSideEnum.ALIGNMENT_WITHOUT, TeamTypeEnum.TEAM_TYPE_PLAYER);
            FightTeam redteam = new FightTeam(TeamEnum.TEAM_CHALLENGER, source.Map.RedCells, AlignmentSideEnum.ALIGNMENT_WITHOUT, TeamTypeEnum.TEAM_TYPE_PLAYER);

            var fight = new FightDual(PopId(), source.Map, blueteam, redteam, cell);

            Fights.TryAdd(fight.Id, fight);
            return fight;
        }

        public FightContextual CreateFightContextual(Character source, short targetObjective)
        {
            FightTeam blueTeam = new FightTeam(TeamEnum.TEAM_DEFENDER, source.Map.BlueCells, AlignmentSideEnum.ALIGNMENT_WITHOUT, TeamTypeEnum.TEAM_TYPE_MONSTER);
            FightTeam redTeam = new FightTeam(TeamEnum.TEAM_CHALLENGER, source.Map.RedCells, AlignmentSideEnum.ALIGNMENT_WITHOUT, TeamTypeEnum.TEAM_TYPE_PLAYER);

            var fight = new FightContextual(PopId(), source, targetObjective, source.Map, blueTeam, redTeam, source.GetCell());
            Fights.TryAdd(fight.Id, fight);
            return fight;
        }
    }
}
