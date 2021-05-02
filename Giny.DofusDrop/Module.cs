using Giny.Core;
using Giny.Core.Extensions;
using Giny.Core.Time;
using Giny.Protocol.Custom.Enums;
using Giny.World.Api;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Results;
using Giny.World.Modules;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DofusDrop
{
    [Module("Dofus drop")]
    public class Module : IModule
    {
        private const double DofusDropPercentage = 01d;

        public void CreateHooks()
        {
            FightEventApi.OnPlayerResultApplied += OnResultApplied;
        }

        public void DestroyHooks()
        {
            FightEventApi.OnPlayerResultApplied += OnResultApplied;
        }

        private void OnResultApplied(FightPlayerResult result)
        {
            var bosses = result.Fighter.EnemyTeam.GetFighters<MonsterFighter>(false).Where(x => x.Record.IsBoss);

            if (bosses.Count() > 0)
            {
                var items = ItemRecord.GetItems().Where(x => x.TypeEnum == ItemTypeEnum.DOFUS).Where(x => x.Level <= result.Character.Level);

                if (items.Count() > 0)
                {
                    var dofusItem = items.Random();

                    AsyncRandom random = new AsyncRandom();

                    var chance = (random.Next(0, 100) + random.NextDouble());
                    var dropRate = DofusDropPercentage;

                    if (!(dropRate >= chance))
                        return;

                    result.Character.Inventory.AddItem((short)dofusItem.Id, 1);
                    result.Loot.AddItem((short)dofusItem.Id, 1);
                }
            }
        }

        public void Initialize()
        {
            Logger.Write("Dofus loot chance : " + DofusDropPercentage);
        }
    }
}
