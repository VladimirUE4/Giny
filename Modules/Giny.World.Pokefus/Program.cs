using Giny.World.Api;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Results;
using Giny.World.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Pokefus
{
    [Module("Pokéfus")]
    public class Program : IModule
    {
        public void CreateHooks()
        {
            FightApi.OnGenerateResults += OnGenerateResults;
        }

        private void OnGenerateResults(Fight arg1, IEnumerable<IFightResult> arg2)
        {
            foreach (var result in arg2.OfType<FightPlayerResult>())
            {
                result.Loot.AddItem(2469, 1);
            }
        }

        public void DestroyHooks()
        {

        }

        public void Initialize()
        {

        }

    }
}
