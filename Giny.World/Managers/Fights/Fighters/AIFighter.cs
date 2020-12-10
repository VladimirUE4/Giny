﻿using System;
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
using Giny.World.Records.Spells;

namespace Giny.World.Managers.Fights.Fighters
{
    public abstract class AIFighter : Fighter
    {
        protected MonsterBrain Brain
        {
            get;
            private set;
        }

        public AIFighter(FightTeam team, CellRecord roleplayCell) : base(team, roleplayCell)
        {

        }
        public override void Initialize()
        {
            this.Id = Fight.PopNextContextualId();
            this.Brain = new MonsterBrain(this);
            base.Initialize();
            this.IsReady = true;
        }
        public override void OnTurnBegin()
        {
            this.Brain.Play();

            if (Alive) // else has already passed turn
            {
                PassTurn();
            }
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

            IEnumerable<CellRecord> cells = path.Take(Stats.MovementPoints.TotalInContext()).Select(x => Fight.Map.GetCell(x));
            return cells.ToList();
        }

        public abstract IEnumerable<SpellRecord> GetSpells();

        public List<CellRecord> FindPath(Fighter target)
        {
            Pathfinding pathfinding = new Pathfinding(Fight.Map);
            pathfinding.PutEntities(Fight.GetFighters<Fighter>());
            var path = pathfinding.FindPath(Cell.Id, target.Cell.Id);

            if (path == null)
            {
                return new List<CellRecord>();
            }

            path.Remove(target.Cell.Id);
            IEnumerable<CellRecord> cells = path.Take(Stats.MovementPoints.TotalInContext()).Select(x => Fight.Map.GetCell(x));
            return cells.ToList();
        }

    }
}
