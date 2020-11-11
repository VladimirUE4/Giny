using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public abstract class AIAction
    {
        protected AIFighter Fighter
        {
            get;
            private set;
        }

        public AIAction(AIFighter fighter)
        {
            this.Fighter = fighter;
        }

        /*
         * Execute action
         */
        public abstract void Execute();

        /*
         *  returns a priority factor between 0 and 1
         */
        public abstract double ComputePriority(); 

    }
}
