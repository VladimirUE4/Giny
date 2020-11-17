using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Api
{
    public class FightApi
    {
        public static event Action<Fight, IEnumerable<IFightResult>> OnGenerateResults;

        public static void GenerateResults(Fight fight, IEnumerable<IFightResult> results)
        {
            OnGenerateResults?.Invoke(fight, results);
        }
    }
}
