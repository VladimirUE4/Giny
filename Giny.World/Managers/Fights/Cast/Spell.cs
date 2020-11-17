using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Cast
{
    public class Spell
    {
        public SpellRecord Record
        {
            get;
            set;
        }
        public SpellLevelRecord Level
        {
            get;
            set;
        }

        public SpellEnum SpellEnum => (SpellEnum)Record.Id;

        public Spell(SpellRecord record,SpellLevelRecord level)
        {
            this.Record = record;
            this.Level = level;
        }
    }
}
