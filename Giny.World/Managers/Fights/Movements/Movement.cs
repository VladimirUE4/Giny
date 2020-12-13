﻿using Giny.World.Managers.Fights.Cast;
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
        public bool CanTrigger
        {
            get;
            private set;
        }

        public Movement(MovementType type, Fighter source, bool canTrigger)
        {
            this.Type = type;
            this.CanTrigger = canTrigger;
            this.Source = source;
        }

        public Fighter GetSource()
        {
            return Source;
        }
    }
}
