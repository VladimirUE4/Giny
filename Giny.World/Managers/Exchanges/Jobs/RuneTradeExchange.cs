using Giny.Core.Time;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Items;
using Giny.World.Managers.Items.Collections;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Exchanges.Jobs
{
    public class RuneTradeExchange : Exchange
    {
        private JobItemCollection Items
        {
            get;
            set;
        }
        public RuneTradeExchange(Character character) : base(character)
        {
            this.Items = new JobItemCollection(character);
        }

        public override ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.RUNES_TRADE;


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
            Character.Client.Send(new ExchangeStartOkRunesTradeMessage());
        }
        public override void ModifyItemPriced(int objectUID, int quantity, long price)
        {
            throw new NotImplementedException();
        }
        public override void Ready(bool ready, short step)
        {
            AsyncRandom random = new AsyncRandom();

            Dictionary<CharacterItemRecord, BasicItemCollection> results = new Dictionary<CharacterItemRecord, BasicItemCollection>();

            foreach (var item in Items.GetItems())
            {
                results.Add(item, new BasicItemCollection());

                for (int i = 0; i < item.Quantity; i++)
                {
                    results[item].AddItems(DecraftItem(random, item));
                }

            }

            OnResulted(results);

            foreach (var result in results.Values)
            {
                Character.Inventory.AddItems(result.GetItems());
            }

            foreach (var item in Items.GetItems())
            {
                Character.Inventory.RemoveItem(item.UId, item.Quantity);
            }

            Items.Clear();
        }



        private List<CharacterItemRecord> DecraftItem(AsyncRandom random, CharacterItemRecord item)
        {
            List<CharacterItemRecord> results = new List<CharacterItemRecord>();

            foreach (EffectInteger effect in item.Effects)
            {
                var record = ItemsManager.Instance.GetRuneItem(effect.EffectEnum, item.Record.Level);

                var runeValue = record.Effects.GetFirst<EffectDice>().Min;

                int effectValue = effect.Value;

                int nbIterations = effectValue / runeValue;

                for (int i = 0; i < nbIterations; i++)
                {
                    var delta = random.NextDouble();

                    if (delta <= ItemsManager.RuneDropChance)
                    {
                        CharacterItemRecord runeItem = ItemsManager.Instance.CreateCharacterItem(record, Character.Id, 3);
                        results.Add(runeItem);
                    }
                }

            }

            return results;
        }
        public override void MoveItem(int uid, int quantity)
        {
            Items.MoveItem(uid, quantity);
        }

        private void OnResulted(Dictionary<CharacterItemRecord, BasicItemCollection> results)
        {
            List<DecraftedItemStackInfo> decraftedItems = new List<DecraftedItemStackInfo>();

            foreach (var result in results)
            {
                decraftedItems.Add(new DecraftedItemStackInfo()
                {
                    objectUID = result.Key.UId,
                    bonusMax = (float)ItemsManager.RuneDropChance,
                    bonusMin = (float)ItemsManager.RuneDropChance,
                    runesId = result.Value.GetItems().Select(x => x.GId).ToArray(),
                    runesQty = result.Value.GetItems().Select(x => x.Quantity).ToArray(),
                });
            }


            Character.Client.Send(new DecraftResultMessage()
            {
                results = decraftedItems.ToArray(),
            });
        }

    }
}
