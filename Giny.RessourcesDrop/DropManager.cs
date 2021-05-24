using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using Giny.Core.Time;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Results;
using Giny.World.Records.Items;
using Giny.World.Records.Jobs;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.RessourcesDrop
{
    public class DropManager : Singleton<DropManager>
    {
        private const double DropChance = 15d;

        private const double MaxQuantity = 50;

        private ConcurrentBag<ItemRecord> DroppableItems = new ConcurrentBag<ItemRecord>();

        private static ItemTypeEnum[] IgnoredTypes = new ItemTypeEnum[]
        {
                ItemTypeEnum.HAT,
                ItemTypeEnum.CLOAK,
                ItemTypeEnum.TROPHY,
                ItemTypeEnum.AMULET,
                ItemTypeEnum.RING,
                ItemTypeEnum.BOOTS,
                ItemTypeEnum.BELT,
                ItemTypeEnum.DRAGOTURKEY_CERTIFICATE,
                ItemTypeEnum.SHIELD,
                ItemTypeEnum.SHOVEL,
                ItemTypeEnum.SWORD,
                ItemTypeEnum.BOW,
                ItemTypeEnum.PET,
                ItemTypeEnum.DAGGER,
                ItemTypeEnum.HAMMER,
                ItemTypeEnum.AXE,
                ItemTypeEnum.STAFF,
                ItemTypeEnum.WAND,
                ItemTypeEnum.FAIRYWORK,
        };

        private static ItemTypeEnum[] IgnoredResultType = new ItemTypeEnum[]
        {
            ItemTypeEnum.CITY_QUESTS,
            ItemTypeEnum.MAIN_QUESTS,
            ItemTypeEnum.TEMPLE_QUESTS,
        };

        public void Initialize()
        {
            foreach (var recipe in RecipeRecord.GetRecipesRecords())
            {
                var resultRecord = ItemRecord.GetItem(recipe.ResultId);

                if (IgnoredResultType.Contains(resultRecord.TypeEnum))
                {
                    continue;
                }

                foreach (var ingredient in recipe.Ingredients)
                {
                    if (!IsDroppable(ingredient))
                    {
                        var record = ItemRecord.GetItem(ingredient);

                        if (IgnoredTypes.Contains(record.TypeEnum))
                        {
                            continue;
                        }

                        if (SkillRecord.GetSkills().Any(x => x.GatheredRessourceItem == ingredient && x.ParentBonesId != -1))
                        {
                            continue;
                        }

                        if (!DroppableItems.Contains(record))
                            DroppableItems.Add(record);
                    }
                }
            }

            Logger.Write("Ressources loot chance : " + DropChance + "%. Max Quantity : " + MaxQuantity + " (" + DroppableItems.Count + " items)");
        }
        private static bool IsDroppable(short gid)
        {
            foreach (var record in MonsterRecord.GetMonsterRecords())
            {
                foreach (var drop in record.Drops)
                {
                    if (!drop.HasCriteria && drop.ItemGId == gid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void OnPlayerResultApplied(FightPlayerResult result)
        {
            if (result.Fight.Winners != result.Fighter.Team)
            {
                return;
            }
            if (!(result.Fight is FightPvM))
            {
                return;
            }
            AsyncRandom random = new AsyncRandom();

            var chance = (random.Next(0, 100) + random.NextDouble());
            var dropRate = DropChance;

            if (!(dropRate >= chance))
                return;

            var maxLevel = result.Fighter.EnemyTeam.GetFighters<MonsterFighter>(false).Max(x => x.Level);

            if (maxLevel > 200)
            {
                maxLevel = 200;
            }

            var item = DroppableItems.Where(x => x.Level <= maxLevel).Random();

            var quantity = random.Next(1, (int)(MaxQuantity + 1));

            result.Character.Inventory.AddItem((short)item.Id, quantity);

            result.Loot.AddItem((short)item.Id, quantity);

            result.Character.Reply("Vous avez drop des ressources spéciales !");
        }
    }
}
