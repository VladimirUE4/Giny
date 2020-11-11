using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Maps.Elements;
using Giny.World.Records.Bidshops;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Generic
{
    public class GenericActions
    {
        [GenericActionHandler(GenericActionEnum.TELEPORT)]
        public static void HandleTeleportAction(Character character, IGenericActionParameter parameter)
        {
            short cellId = -1;
            if (short.TryParse(parameter.Param2, out cellId))
            {
                character.Teleport(int.Parse(parameter.Param1), cellId);
            }
            else
            {
                character.Teleport(int.Parse(parameter.Param1));
            }
        }
        [GenericActionHandler(GenericActionEnum.OPEN_BANK)]
        public static void HandleOpenBank(Character character, IGenericActionParameter parameter)
        {
            character.OpenBank();
        }
        [GenericActionHandler(GenericActionEnum.COLLECT)]
        public static void HandleCollect(Character character, IGenericActionParameter parameter)
        {
            MapStatedElement element = parameter as MapStatedElement;
            
            if (element == null)
            {
                throw new Exception("Unable to collect. Invalid interactive element.");
            }

            lock (element)
            {
                element.Use(character);
            }
        }
        [GenericActionHandler(GenericActionEnum.BIDSHOP)]
        public static void HandleBidshop(Character character,IGenericActionParameter parameter)
        {
            BidShopRecord record = BidShopRecord.GetBidShop(int.Parse(parameter.Param1));
            character.OpenBuyExchange(record);
        }
        [GenericActionHandler(GenericActionEnum.ZAAP)]
        public static void HandleZaap(Character character,IGenericActionParameter parameter)
        {
            character.OpenZaap((MapElement)parameter);
        }
        [GenericActionHandler(GenericActionEnum.ZAAPI)]
        public static void HandleZaapi(Character character, IGenericActionParameter parameter)
        {
            character.OpenZaapi((MapElement)parameter);
        }
    }
}
