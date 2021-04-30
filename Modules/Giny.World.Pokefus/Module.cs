using Giny.World.Api;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Results;
using Giny.World.Modules;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Pokefus
{
    [Module("Pokéfus")]
    public class Module : IModule
    {
        public void Initialize()
        {
            PokefusManager.Instance.Initialize();
        }

        public void CreateHooks()
        {
            FightApi.OnPlayerResultApplied += PokefusManager.Instance.OnPlayerResultApplied; 
            CharacterApi.OnHumanOptionsCreated += PokefusManager.Instance.OnHumanOptionsCreated;
            FightApi.OnFighterJoined += PokefusManager.Instance.OnFighterJoined;
        }

        public void DestroyHooks()
        {
            FightApi.OnPlayerResultApplied -= PokefusManager.Instance.OnPlayerResultApplied;
            CharacterApi.OnHumanOptionsCreated -= PokefusManager.Instance.OnHumanOptionsCreated;
            FightApi.OnFighterJoined -= PokefusManager.Instance.OnFighterJoined;
        }

    }
}
