using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
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
        public static event Action<FightPlayerResult> OnPlayerResultApplied;
        public static event Action<Fight> OnPlacementStarted;
        public static event Action<Fighter> OnFighterJoined;

        internal static void PlayerResultApplied(FightPlayerResult fightPlayerResult)
        {
            OnPlayerResultApplied?.Invoke(fightPlayerResult);
        }

        internal static void PlacementStarted(Fight fight)
        {
            OnPlacementStarted?.Invoke(fight);
        }

        internal static void FighterJoined(Fighter fighter)
        {
            OnFighterJoined?.Invoke(fighter);
        }
    }
}
