using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Pokefus
{
    public class Main : IModule
    {
        public void CreateHooks()
        {
            // ItemManager, CharacterManager, SpellManager, NpcManager .. etc .. etc ..

            /* Should we build an API instead of using event ? */
            FightManager.Instance.OnFightStarted += OnFightStart; 
        }

        public void DestroyHooks()
        {
            FightManager.Instance.OnFightStarted -= OnFightStart; 
        }

        public void Initialize()
        {
           
        }

        private void OnFightStart(Fight fight)
        {
            foreach (var monster in fight.GetFighters<MonsterFighter>())
            {
                Console.WriteLine("Module found monster " + monster.Name + "!");
            }
        }
    }
}
