using Giny.Core.DesignPattern;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Criterias
{
    [Criteria("Pw")]
    public class HasGuildCriteria : Criteria
    {
        [WIP]
        public override bool Eval(WorldClient client)
        {
            return false;
        }
    }
    [Criteria("Py")]
    public class GuildLevelCriteria : Criteria
    {
        [WIP]
        public override bool Eval(WorldClient client)
        {
            return false;
        }
    }
}
