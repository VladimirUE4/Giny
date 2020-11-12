using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.World.Managers.Fights.AI;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Stats;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Maps.Paths;
using Giny.World.Managers.Monsters;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;

namespace Giny.World.Managers.Fights.Fighters
{
    public abstract class AIFighter : Fighter, IMonster
    {
        public MonsterRecord Record
        {
            get;
            private set;
        }
        protected MonsterGrade Grade
        {
            get;
            private set;
        }
        protected MonsterBrain Brain
        {
            get;
            private set;
        }
        public short MonsterId => (short)Record.Id;

        public AIFighter(FightTeam team, CellRecord roleplayCell, MonsterRecord record, MonsterGrade grade) : base(team, roleplayCell)
        {
            this.Record = record;
            this.Grade = grade;
        }
        public override void Initialize()
        {
            this.Id = Fight.PopNextContextualId();
            this.Stats = new FighterStats(Grade);
            this.Brain = new MonsterBrain(this);
            base.Initialize();
            this.IsReady = true;
        }
        public override void OnTurnBegin()
        {

            this.Brain.Play();
            PassTurn();
            return;

            try
            {
                this.Brain.Play();
            }
            catch
            {
                Fight.Warn("Monster error");
            }
            finally
            {
                PassTurn();
            }
        }

        public List<CellRecord> FindPath(MapPoint target)
        {
            Pathfinding pathfinding = new Pathfinding(Fight.Map);
            pathfinding.PutEntities(Fight.GetFighters<Fighter>());
            var path = pathfinding.FindPath(Cell.Id, target.CellId);

            if (path == null)
            {
                return new List<CellRecord>();
            }

            path.Insert(0, Cell.Id);
            IEnumerable<CellRecord> cells = path.Take(Stats.MovementPoints.TotalInContext() + 1).Select(x => Fight.Map.GetCell(x));
            return cells.ToList();
        }
        public List<CellRecord> FindPath(Fighter target)
        {
            Pathfinding pathfinding = new Pathfinding(Fight.Map);
            pathfinding.PutEntities(Fight.GetFighters<Fighter>());
            var path = pathfinding.FindPath(Cell.Id, target.Cell.Id);

            if (path == null)
            {
                return new List<CellRecord>();
            }

            path.Insert(0, Cell.Id);
            path.Remove(target.Cell.Id);
            IEnumerable<CellRecord> cells = path.Take(Stats.MovementPoints.TotalInContext() + 1).Select(x => Fight.Map.GetCell(x));
            return cells.ToList();
        }
        public override bool HasSpell(short spellId)
        {
            return Record.Spells.Contains(spellId);
        }

        public override Spell GetSpell(short spellId)
        {
            var record = Record.SpellRecords[spellId];
            var level = record.GetLevel(Grade.GradeId);
            return new Spell(record, level);
        }
    }
}
