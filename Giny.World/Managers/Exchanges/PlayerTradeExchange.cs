using Giny.Core.Network.Messages;
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
    public class PlayerTradeExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.PLAYER_TRADE;
            }
        }

        private ItemCollection<CharacterItemRecord> ExchangedItems = new ItemCollection<CharacterItemRecord>();

        private bool IsReady = false;

        private long MovedKamas = 0;

        public Character SecondCharacter
        {
            get;
            set;
        }

        public PlayerTradeExchange(Character character, Character secondCharacter)
            : base(character)
        {
            this.SecondCharacter = secondCharacter;

            ExchangedItems.OnItemAdded += ExchangedItems_OnItemAdded;
            ExchangedItems.OnItemRemoved += ExchangedItems_OnItemRemoved;
            ExchangedItems.OnItemStacked += ExchangedItems_OnItemStacked;
            ExchangedItems.OnItemUnstacked += ExchangedItems_OnItemUnstacked;
        }

        void ExchangedItems_OnItemUnstacked(CharacterItemRecord arg1, int arg2)
        {
            OnObjectModified(arg1);
        }
        void ExchangedItems_OnItemStacked(CharacterItemRecord arg1, int arg2)
        {
            OnObjectModified(arg1);
        }
        void ExchangedItems_OnItemRemoved(CharacterItemRecord obj)
        {
            Character.Client.Send(new ExchangeObjectRemovedMessage()
            {
                remote = false,
                objectUID = obj.UId
            });
            SecondCharacter.Client.Send(new ExchangeObjectRemovedMessage
            {
                remote = true,
                objectUID = obj.UId
            });
        }

        void ExchangedItems_OnItemAdded(CharacterItemRecord obj)
        {
            Character.Client.Send(new ExchangeObjectAddedMessage()
            {
                remote = false,
                @object = obj.GetObjectItem()
            });
            SecondCharacter.Client.Send(new ExchangeObjectAddedMessage()
            {
                remote = true,
                @object = obj.GetObjectItem()
            });
        }
        private void OnObjectModified(CharacterItemRecord obj)
        {
            Character.Client.Send(new ExchangeObjectModifiedMessage()
            {
                remote = false,
                @object = obj.GetObjectItem(),
            });
            SecondCharacter.Client.Send(new ExchangeObjectModifiedMessage()
            {
                remote = true,
                @object = obj.GetObjectItem(),
            });
        }


        public override void Open()
        {
            this.Send(new ExchangeStartedWithPodsMessage()
            {
                exchangeType = (byte)ExchangeType,
                firstCharacterId = Character.Id,
                firstCharacterCurrentWeight = Character.Inventory.CurrentWeight,
                firstCharacterMaxWeight = Character.Inventory.TotalWeight,
                secondCharacterId = SecondCharacter.Id,
                secondCharacterCurrentWeight = SecondCharacter.Inventory.CurrentWeight,
                secondCharacterMaxWeight = SecondCharacter.Inventory.TotalWeight,
            });
        }

        public void Send(NetworkMessage message)
        {
            Character.Client.Send(message);
            SecondCharacter.Client.Send(message);
        }
        public override void Close()
        {
            SecondCharacter.Client.Send(new ExchangeLeaveMessage()
            {
                dialogType = (byte)DialogType,
                success = Succes
            });

            SecondCharacter.Dialog = null;

            Character.Client.Send(new ExchangeLeaveMessage()
            {
                dialogType = (byte)DialogType,
                success = Succes
            });

            Character.Dialog = null;
        }
        private bool CanMoveItem(CharacterItemRecord item, int quantity)
        {
            if (item.PositionEnum != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED || !item.CanBeExchanged())
                return false;

            CharacterItemRecord exchanged = null;

            exchanged = ExchangedItems.GetItem(item.GId, item.Effects);

            if (exchanged != null && exchanged.UId != item.UId)
                return false;

            exchanged = ExchangedItems.GetItem(item.UId);

            if (exchanged == null)
            {
                return true;
            }

            if (exchanged.Quantity + quantity > item.Quantity)
                return false;
            else
                return true;
        }
        public override void MoveItem(int uid, int quantity)
        {
            if (!IsReady)
            {
                CharacterItemRecord item = Character.Inventory.GetItem(uid);

                if (item != null && CanMoveItem(item, quantity))
                {
                    if (SecondCharacter.GetDialog<PlayerTradeExchange>().IsReady)
                    {
                        SecondCharacter.GetDialog<PlayerTradeExchange>().Ready(false, 0);
                    }
                    if (quantity > 0)
                    {
                        if (item.Quantity >= quantity)
                        {
                            ExchangedItems.AddItem(item, quantity);
                        }
                    }
                    else
                    {
                        ExchangedItems.RemoveItem(item.UId, Math.Abs(quantity));
                    }
                }
            }
        }

        public override void Ready(bool ready, short step)
        {
            this.IsReady = ready;

            Send(new ExchangeIsReadyMessage(Character.Id, this.IsReady));

            if (this.IsReady && SecondCharacter.GetDialog<PlayerTradeExchange>().IsReady)
            {
                foreach (var item in ExchangedItems.GetItems())
                {
                    item.CharacterId = SecondCharacter.Id;
                    SecondCharacter.Inventory.AddItem((CharacterItemRecord)item.CloneWithoutUID());
                    Character.Inventory.RemoveItem(item.UId, item.Quantity);
                }

                foreach (var item in SecondCharacter.GetDialog<PlayerTradeExchange>().ExchangedItems.GetItems())
                {
                    item.CharacterId = Character.Id;
                    Character.Inventory.AddItem((CharacterItemRecord)item.CloneWithoutUID());
                    SecondCharacter.Inventory.RemoveItem(item.UId, item.Quantity);
                }

                SecondCharacter.AddKamas(MovedKamas);
                Character.RemoveKamas(MovedKamas);

                Character.AddKamas(SecondCharacter.GetDialog<PlayerTradeExchange>().MovedKamas);
                SecondCharacter.RemoveKamas(SecondCharacter.GetDialog<PlayerTradeExchange>().MovedKamas);

                this.Succes = true;
                this.Close();
            }
        }

        public override void MoveKamas(long quantity)
        {
            if (quantity <= Character.Record.Kamas)
            {
                if (IsReady)
                {
                    Ready(false, 0);
                }
                if (SecondCharacter.GetDialog<PlayerTradeExchange>().IsReady)
                {
                    SecondCharacter.GetDialog<PlayerTradeExchange>().Ready(false, 0);
                }

                Character.Client.Send(new ExchangeKamaModifiedMessage()
                {
                    remote = false,
                    quantity = quantity
                });
                SecondCharacter.Client.Send(new ExchangeKamaModifiedMessage()
                {
                    remote = true,
                    quantity = quantity
                });

                MovedKamas = quantity;
            }


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