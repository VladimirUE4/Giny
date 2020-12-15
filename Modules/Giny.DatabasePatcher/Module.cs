using Giny.Core;
using Giny.DatabasePatcher.Patchs;
using Giny.DatabasePatcher.Patchs.Placements;
using Giny.World.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DatabasePatcher
{
    [Module("Database patcher")]
    public class Module : IModule
    {
        public void CreateHooks()
        {

        }

        public void DestroyHooks()
        {

        }


        public void Initialize()
        {
            ElementsSpawnManager.SynchronizeElements();
            MA3Manager.Initialize();
            MonsterKamasDropManager.Initialize();
            SpellCategoryManager.Initialize();
            MapPlacementsManager.Initialize();
        }

        
    }
}
