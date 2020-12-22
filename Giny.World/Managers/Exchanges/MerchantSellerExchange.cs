using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Entities.Merchants;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Exchanges
{
    public class MerchantSellerExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.DISCONNECTED_VENDOR;

        private CharacterMerchant Merchant
        {
            get;
            set;
        }
        public MerchantSellerExchange(Character character, CharacterMerchant merchant) : base(character)
        {
            this.Merchant = merchant;
        }


        public override void ModifyItemPriced(int objectUID, int quantity, long price)
        {
            throw new NotImplementedException();
        }

        public override void MoveItem(int uid, int quantity)
        {
            throw new NotImplementedException();
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
            Character.Client.Send(new ExchangeStartOkHumanVendorMessage()
            {
                sellerId = Merchant.Id,
                objectsInfos = Merchant.GetItems().Select(x => x.GetObjectItemToSellInHumanVendorShop()).ToArray(),
            });

        }

        public void Buy(int uid, int quantity)
        {
            MerchantItemRecord item = Merchant.GetItem(uid);

            if (item == null || item.Quantity < quantity)
            {
                Character.OnExchangeError(ExchangeErrorEnum.BUY_ERROR);
                return;
            }

            long cost = item.Price * quantity;

            if (!Character.RemoveKamas(cost))
            {
                Character.OnExchangeError(ExchangeErrorEnum.BUY_ERROR);
                return;
            }

            this.Character.Client.Send(new ExchangeShopStockMovementRemovedMessage()
            {
                objectId = item.UId,
            });

            item.Sold = true;

            Character.OnKamasLost(cost);

            Character.Inventory.AddItem(item.ToCharacterItemRecord(Character.Id), quantity);

            Merchant.RemoveItem(item, quantity);

            Character.OnItemGained(item.GId, quantity);

            Character.Client.Send(new ExchangeBuyOkMessage());

            if (Merchant.GetItems().Count() == 0)
            {
                Merchant.Remove();
            }
        }

        public override void Ready(bool ready, short step)
        {
            throw new NotImplementedException();
        }
    }
}
