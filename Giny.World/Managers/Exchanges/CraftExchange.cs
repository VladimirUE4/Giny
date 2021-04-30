using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Formulas;
using Giny.World.Managers.Items;
using Giny.World.Managers.Items.Collections;
using Giny.World.Records.Characters;
using Giny.World.Records.Items;
using Giny.World.Records.Jobs;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Exchanges
{
    public class CraftExchange : Exchange
    {
        private const int CountDefault = 1;

        public const int CountLimit = 5000;

        public override ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.CRAFT;

        private SkillRecord Skill
        {
            get;
            set;
        }
        private CraftItemCollection Items
        {
            get;
            set;
        }
        private CharacterJob CharacterJob
        {
            get;
            set;
        }
        private int Count
        {
            get;
            set;
        }
        public CraftExchange(Character character, SkillRecord skillRecord) : base(character)
        {
            this.Skill = skillRecord;
            this.Items = new CraftItemCollection(character);
            this.CharacterJob = character.GetJob(skillRecord.ParentJobId);
            this.Count = CountDefault;
        }

        public override void ModifyItemPriced(int objectUID, int quantity, long price)
        {
            throw new NotImplementedException();
        }

        public override void MoveItem(int uid, int quantity)
        {
            CharacterItemRecord item = null;

            if (quantity > 0)
            {
                item = Character.Inventory.GetItem(uid);

                if (item != null && CanAddItem(item, quantity))
                {
                    Items.AddItem(item, quantity);
                }
            }
            else if (quantity < 0)
            {
                item = Items.GetItem(uid);

                if (item != null)
                {
                    Items.RemoveItem(item, Math.Abs(quantity));
                }
            }
            else
            {
                return;
            }
        }

        public void SetRecipe(short gid)
        {
            RecipeRecord recipeRecord = RecipeRecord.GetRecipeRecord(gid);

            foreach (var ingredient in recipeRecord.IngredientsQuantities)
            {
                CharacterItemRecord item = Character.Inventory.GetFirstItem(ingredient.Key, ingredient.Value);

                if (item != null)
                {
                    Items.AddItem(item, ingredient.Value);
                }
            }
        }

        private bool CanAddItem(CharacterItemRecord item, int quantity)
        {
            if (item.PositionEnum != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return false;

            CharacterItemRecord exchanged = Items.GetItem(item.GId, item.Effects);

            if (exchanged != null && exchanged.UId != item.UId)
                return false;

            exchanged = Items.GetItem(item.UId);

            if (exchanged == null)
            {
                return true;
            }

            if (exchanged.Quantity + quantity > item.Quantity)
                return false;
            else
                return true;
        }

        public void SetCount(int count)
        {
            RecipeRecord recipe = GetCurrentRecipe();

            if (recipe == null)
            {
                return;
            }

            int countMax = recipe.GetMaximumCount(Character.Inventory);

            if (count > CountLimit)
            {
                count = CountLimit;
            }

            if (count <= countMax)
            {
                this.Count = count;
                Character.Client.Send(new ExchangeCraftCountModifiedMessage(count));
            }
        }

        public override void MoveItemPriced(int objectUID, int quantity, long price)
        {
            throw new NotImplementedException();
        }

        public override void MoveKamas(long quantity)
        {
            throw new NotImplementedException();
        }

        public override void OnNpcGenericAction(NpcActionsEnum action)
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            Character.Client.Send(new ExchangeStartOkCraftWithInformationMessage()
            {
                skillId = (short)Skill.Id,
            });
        }
        private RecipeRecord GetCurrentRecipe()
        {
            short[] gids = Items.GetItems().Select(x => x.GId).ToArray();
            int[] quantities = Items.GetItems().Select(x => x.Quantity).ToArray();

            RecipeRecord recipeRecord = RecipeRecord.GetRecipeRecord(gids, quantities);
            return recipeRecord;
        }
        public override void Ready(bool ready, short step)
        {
            if (ready)
            {
                RecipeRecord recipeRecord = GetCurrentRecipe();

                if (recipeRecord != null && recipeRecord.JobId == CharacterJob.JobId && recipeRecord.SkillId == Skill.Id)
                {
                    ItemRecord resultRecord = ItemRecord.GetItem((int)recipeRecord.ResultId);

                    if (resultRecord.Level <= CharacterJob.Level || resultRecord == null)
                    {
                        PerformCraft(recipeRecord);
                    }
                    else
                    {
                        OnCraftResulted(CraftResultEnum.CRAFT_FAILED);
                    }

                }
                else
                {
                    OnCraftResulted(CraftResultEnum.CRAFT_FAILED);
                }
            }
        }
        private void PerformCraft(RecipeRecord recipe)
        {
            List<CharacterItemRecord> results = new List<CharacterItemRecord>();
            Dictionary<int, int> removed = new Dictionary<int, int>();

            for (int i = 0; i < Count; i++)
            {
                foreach (var ingredient in Items.GetItems())
                {
                    if (!removed.ContainsKey(ingredient.UId))
                        removed.Add(ingredient.UId, ingredient.Quantity);
                    else
                        removed[ingredient.UId] += ingredient.Quantity;

                }
                results.Add(ItemManager.Instance.CreateCharacterItem(recipe.ResultRecord, Character.Id, 1));
            }

            Character.Inventory.RemoveItems(removed);
            Character.Inventory.AddItems(results);

            if (results.Count == 1)
            {
                OnCraftResulted(CraftResultEnum.CRAFT_SUCCESS, results.Last().GetObjectItemNotInContainer());
            }
            else
            {
                OnCraftResulted(CraftResultEnum.CRAFT_SUCCESS, new ObjectItemNotInContainer()
                {
                    effects = recipe.ResultRecord.Effects.GetObjectEffects(),
                    objectGID = (short)recipe.ResultId,
                    objectUID = 0,
                    quantity = results.Count,
                });
            }

            int craftXpRatio = recipe.ResultRecord.CraftXpRatio;
            int exp = JobFormulas.Instance.GetCraftExperience(recipe.ResultRecord.Level, CharacterJob.Level, craftXpRatio);
            Character.AddJobExp(CharacterJob.JobId, exp * Count);
            Items.Clear();
            SetCount(1);
        }
        private void OnCraftResulted(CraftResultEnum result)
        {
            Character.Client.Send(new ExchangeCraftResultMessage((byte)result));
        }
        private void OnCraftResulted(CraftResultEnum result, ObjectItemNotInContainer item)
        {
            Character.Client.Send(new ExchangeCraftResultWithObjectDescMessage()
            {
                craftResult = (byte)result,
                objectInfo = item,
            });
        }
    }
}
