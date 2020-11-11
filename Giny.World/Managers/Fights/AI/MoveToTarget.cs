using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public class MoveToTarget : AIAction
    {
        public MoveToTarget(AIFighter fighter) : base(fighter)
        {

        }

        public override double ComputePriority()
        {
            return 0.5d;
        }

        public override void Execute()
        {
            var target = Fighter.EnemyTeam.CloserFighter(Fighter);
            var path = Fighter.FindPath(target);
            Fighter.Move(path);
        }
    }
}
