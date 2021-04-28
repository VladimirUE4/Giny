using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Criterias
{
    [Criteria("DO")]
    public class ObjectiveCriteria : Criteria
    {
        public override bool Eval(WorldClient client)
        {
            bool obj = client.Character.HasReachObjective(short.Parse(CriteriaValue));
            return ComparaisonSymbol == '=' ? obj : !obj;
        }
    }
}
