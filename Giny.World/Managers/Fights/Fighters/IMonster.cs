﻿using Giny.IO.D2OClasses;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Fighters
{
    public interface IMonster
    {
        public short MonsterId
        {
            get;
        }
    }
}
