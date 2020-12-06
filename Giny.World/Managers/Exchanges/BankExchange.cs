using Giny.Core.DesignPattern;
using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Items;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Exchanges
{
    public class BankExchange : Exchange
    {
        public const int STORAGE_MAX_SLOTS = 300;

        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.BANK;
            }
        }
        private ItemCollection<BankItemRecord> m_items; /* put this in Character. Create another ItemCollection */

        public BankExchange(Character character, ItemCollection<BankItemRecord> bankItems)
            : base(character)
        {
            m_items = bankItems;
            m_items.OnItemAdded += m_items_OnItemAdded;
            m_items.OnItemRemoved += m_items_OnItemRemoved;
            m_items.OnItemStacked += m_items_OnItemStacked;
            m_items.OnItemUnstacked += m_items_OnItemUnstacked;
        }

        void m_items_OnItemUnstacked(BankItemRecord arg1, int arg2)
        {
            arg1.UpdateElement();
            Character.Client.Send(new StorageObjectUpdateMessage(arg1.GetObjectItem()));
        }

        void m_items_OnItemStacked(BankItemRecord arg1, int arg2)
        {
            arg1.UpdateElement();
            Character.Client.Send(new StorageObjectUpdateMessage(arg1.GetObjectItem()));
        }

        void m_items_OnItemRemoved(BankItemRecord obj)
        {
            obj.RemoveElement();
            Character.Client.Send(new StorageObjectRemoveMessage(obj.UId));
        }

        void m_items_OnItemAdded(BankItemRecord obj)
        {
            obj.AddElement();
            Character.Client.Send(new StorageObjectUpdateMessage(obj.GetObjectItem()));
        }
        public override void Open()
        {
            Character.Client.Send(new ExchangeStartedWithStorageMessage()
            {
                exchangeType = (byte)ExchangeType,
                storageMaxSlot = STORAGE_MAX_SLOTS,

            });
            Character.Client.Send(new StorageInventoryContentMessage()
            {
                kamas = Character.Client.WorldAccount.BankKamas,
                objects = m_items.GetObjectsItems(),
            });
        }
        public override void MoveItem(int uid, int quantity)
        {
            if (quantity > 0)
            {
                CharacterItemRecord item = Character.Inventory.GetItem(uid);

                if (item != null && item.Quantity >= quantity && item.CanBeExchanged())
                {
                    var bankItem = item.ToBankItemRecord(Character.Client.Account.Id);
                    bankItem.Quantity = quantity;
                    Character.Inventory.RemoveItem(item.UId, quantity);
                    m_items.AddItem(bankItem);
                }
            }
            else
            {
                BankItemRecord item = m_items.GetItem(uid);
                int removedQuantity = Math.Abs(quantity);

                if (item != null && item.Quantity >= removedQuantity)
                {
                    var characterItemRecord = item.ToCharacterItemRecord(Character.Id);
                    characterItemRecord.Quantity = removedQuantity;
                    m_items.RemoveItem(uid, removedQuantity);
                    Character.Inventory.AddItem(characterItemRecord);

                }
            }
        }
        public override void MoveKamas(long quantity)
        {
            if (quantity < 0)
            {
                if (Character.Client.WorldAccount.BankKamas >= Math.Abs(quantity))
                    Character.AddKamas(Math.Abs(quantity));
                else
                    return;
            }
            else
            {
                if (!Character.RemoveKamas(quantity))
                    return;
            }

            Character.Client.WorldAccount.BankKamas += quantity;
            Character.Client.WorldAccount.UpdateElement();
            Character.Client.Send(new StorageKamasUpdateMessage((int)Character.Client.WorldAccount.BankKamas));
        }

        public override void Ready(bool ready, short step)
        {
            throw new NotImplementedException();
        }

        public override void OnNpcGenericAction(NpcActionsEnum action)
        {
            throw new NotImplementedException();
        }

        public override void MoveItemPriced(int objectUID, int quantity, long price)
        {
            throw new NotImplementedException();
        }

        public override void ModifyItemPriced(int objectUID, int quantity, long price)
        {
            throw new NotImplementedException();
        }
    }
}
