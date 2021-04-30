using Giny.Protocol.Messages;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Items.Collections
{
    public class CraftItemCollection : ItemCollection<CharacterItemRecord>
    {
        private Character Character
        {
            get;
            set;
        }

        public CraftItemCollection(Character character)
        {
            this.Character = character;
        }
        public override void OnItemAdded(CharacterItemRecord item)
        {
            Character.Client.Send(new ExchangeObjectAddedMessage()
            {
                @object = item.GetObjectItem(),
                remote = false,
            });
        }
        public override void OnItemRemoved(CharacterItemRecord item)
        {
            Character.Client.Send(new ExchangeObjectRemovedMessage()
            {
                objectUID = item.UId,
                remote = false
            });
        }
        public override void OnItemQuantityChanged(CharacterItemRecord item, int quantity)
        {
            Character.Client.Send(new ExchangeObjectModifiedMessage()
            {
                @object = item.GetObjectItem(),
                remote = false,
            });
        }
        public override void OnItemsRemoved(IEnumerable<CharacterItemRecord> items)
        {
            Character.Client.Send(new ExchangeObjectsRemovedMessage()
            {
                objectUID = items.Select(x => x.UId).ToArray(),
                remote = false,
            });
        }


    }
}
