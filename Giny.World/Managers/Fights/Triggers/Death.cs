using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Triggers
{
    public class Death : ITriggerToken
    {
        private Fighter Source
        {
            get;
            set;
        }
        public bool CanTrigger
        {
            get;
            private set;
        }
        public Death(Fighter source,bool canTrigger)
        {
            this.Source = source;
            this.CanTrigger = canTrigger;
        }
       

        public Fighter GetSource()
        {
            return Source;
        }
    }
}
