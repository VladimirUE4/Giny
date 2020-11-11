using Giny.Core.DesignPattern;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Criterias
{
    public class CriteriaManager : Singleton<CriteriaManager>
    {
        public bool EvaluateCriterias(WorldClient client, string criteria)
        {
            return true;
        }
    }
}
