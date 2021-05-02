using Giny.World.Managers.Entities.Characters;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Api
{
    public class InventoryEventApi
    {
        /*
         * bool = can equip ?
         */
        public delegate bool OnEquipItemDelegate(Character character, CharacterItemRecord item);

        public static event OnEquipItemDelegate CanEquipItem;

        public static bool OnItemEquipping(Character character, CharacterItemRecord item)
        {
            if (CanEquipItem != null)
            {
                return CanEquipItem.Invoke(character, item);
            }
            else
            {
                return true;
            }
        }
    }
}
