using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Movements
{
    public class Movement : ITriggerToken
    {
        public MovementType Type
        {
            get;
            private set;
        }
        private Fighter Source
        {
            get;
            set;
        }
        public bool TriggerMarks
        {
            get;
            set;
        }
        public Movement(MovementType type, Fighter source,bool triggerMarks = true)
        {
            this.Type = type;
            this.Source = source;
            this.TriggerMarks = triggerMarks;
        }

        public Fighter GetSource()
        {
            return Source;
        }
    }
}
