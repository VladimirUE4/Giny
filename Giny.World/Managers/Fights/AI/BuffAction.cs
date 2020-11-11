using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public class BuffAction : AIAction
    {
        public BuffAction(AIFighter fighter) : base(fighter)
        {

        }

        public override void Analyse()
        {
            
        }

        public override double ComputePriority()
        {
            return 0d;
        }

        public override void Execute()
        {
           
        }
    }
}
