using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Marks
{
    [Flags]
    public enum MarkTriggerType
    {
        Instant = 0x1,
        OnMove = 0x2,
        OnTurnStart = 0x4,
        OnTurnEnd = 0x8,
        None = 2147483647,
    }
}
