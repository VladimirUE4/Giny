using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Items.Collections;
using Giny.World.Records.Items;
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

        public CraftExchange(Character character, SkillRecord skillRecord) : base(character)
        {
            this.Skill = skillRecord;
            this.Items = new CraftItemCollection(character);
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

        public override void Ready(bool ready, short step)
        {
            throw new NotImplementedException();
        }
    }
}
