using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Maps.Elements;
using Giny.World.Managers.Monsters;
using Giny.World.Records.Bidshops;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Generic
{
    public class GenericActions
    {
        [GenericActionHandler(GenericActionEnum.AddItem, 2)]
        public static void HandleAddItem(Character character, IGenericActionParameter parameter)
        {
            short itemId = short.Parse(parameter.Param1);
            int quantity = int.Parse(parameter.Param2);
            character.Inventory.AddItem(itemId, quantity);
            character.OnItemGained(itemId, quantity);
        }
        [GenericActionHandler(GenericActionEnum.RemoveItem, 2)]
        public static void HandleRemoveItem(Character character, IGenericActionParameter parameter)
        {
            short itemId = short.Parse(parameter.Param1);
            int quantity = int.Parse(parameter.Param2);

            CharacterItemRecord item = character.Inventory.GetFirstItem(itemId, quantity);

            if (item == null)
            {
                character.ReplyWarning("Unable to remove item to character.");
                return;
            }

            character.Inventory.RemoveItem(item.UId, quantity);
            character.OnItemLost(itemId, quantity);
        }
        [GenericActionHandler(GenericActionEnum.Teleport, 1)]
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
        [GenericActionHandler(GenericActionEnum.OpenBank, 0)]
        public static void HandleOpenBank(Character character, IGenericActionParameter parameter)
        {
            character.OpenBank();
        }
        [GenericActionHandler(GenericActionEnum.Collect, 0)]
        public static void HandleCollect(Character character, IGenericActionParameter parameter)
        {
            MapStatedElement element = parameter as MapStatedElement;

            if (element == null)
            {
                character.ReplyWarning("Unable to collect. Invalid interactive element.");
            }

            element.Use(character);
        }
        [GenericActionHandler(GenericActionEnum.Bidshop, 1)]
        public static void HandleBidshop(Character character, IGenericActionParameter parameter)
        {
            BidShopRecord record = BidShopRecord.GetBidShop(int.Parse(parameter.Param1));
            character.OpenBuyExchange(record);
        }
        [GenericActionHandler(GenericActionEnum.Zaap, 0)]
        public static void HandleZaap(Character character, IGenericActionParameter parameter)
        {
            character.OpenZaap((MapElement)parameter);
        }
        [GenericActionHandler(GenericActionEnum.Zaapi, 0)]
        public static void HandleZaapi(Character character, IGenericActionParameter parameter)
        {
            character.OpenZaapi((MapElement)parameter);
        }
        [GenericActionHandler(GenericActionEnum.LearnOrnament, 1)]
        public static void HandleLearnOrnament(Character character, IGenericActionParameter parameter)
        {
            character.LearnOrnament(short.Parse(parameter.Param1), true);
        }
        [GenericActionHandler(GenericActionEnum.LearnTitle, 1)]
        public static void HandleLearnTitle(Character character, IGenericActionParameter parameter)
        {
            character.LearnTitle(short.Parse(parameter.Param1));
        }

        [GenericActionHandler(GenericActionEnum.CreateGuild, 0)]
        public static void HandleCreateGuild(Character character, IGenericActionParameter parameter)
        {
            character.OpenGuildCreationDialog();
        }
        [GenericActionHandler(GenericActionEnum.LearnSpell, 1)]
        public static void HandleLearnSpell(Character character, IGenericActionParameter parameter)
        {
            character.LearnSpell(short.Parse(parameter.Param1), true);
        }
        [GenericActionHandler(GenericActionEnum.AddKamas, 1)]
        public static void HandleAddKamas(Character character, IGenericActionParameter parameter)
        {
            long amount = long.Parse(parameter.Param1);
            character.AddKamas(amount);
            character.OnKamasGained(amount);
        }

        [GenericActionHandler(GenericActionEnum.Craft, 0)]
        public static void HandleCraft(Character character, IGenericActionParameter parameter)
        {
            MapInteractiveElement element = parameter as MapInteractiveElement;

            if (element == null)
            {
                throw new Exception("Unable to craft. Invalid generic parameter.");
            }

            character.OpenCraftExchange(element.Record.Skill.Record);
        }
        [GenericActionHandler(GenericActionEnum.Smithmagic, 0)]
        public static void HandlSmithmagic(Character character, IGenericActionParameter parameter)
        {
            MapInteractiveElement element = parameter as MapInteractiveElement;

            if (element == null)
            {
                throw new Exception("Unable to craft. Invalid generic parameter.");
            }

            character.OpenSmithmagicExchange(element.Record.Skill.Record);
        }

        [GenericActionHandler(GenericActionEnum.RuneTrade,0)]
        public static void HandleRuneTrade(Character character,IGenericActionParameter parameter)
        {
            character.OpenRuneTradeExchange();
        }

        [GenericActionHandler(GenericActionEnum.Reach, 1)]
        public static void HandleReach(Character character, IGenericActionParameter parameter)
        {

            character.ReachObjective(short.Parse(parameter.Param1));
            string message = "Status de quête mis a jour : <b>" + parameter.Param2 + "</b>";

            character.DisplayNotification(message);
            character.Reply(message);
        }
        [GenericActionHandler(GenericActionEnum.AddExperience, 1)]
        public static void HandleAddExperience(Character character, IGenericActionParameter parameter)
        {
            character.AddExperience(long.Parse(parameter.Param1), true);
        }

        [GenericActionHandler(GenericActionEnum.Fight, 2)]
        public static void HandleFight(Character character, IGenericActionParameter parameter)
        {
            short targetObjective = short.Parse(parameter.Param2);
            IEnumerable<MonsterRecord> records = parameter.Param1.Split(',').Select(x => MonsterRecord.GetMonsterRecord(short.Parse(x)));

            if (records.Count() > 0)
            {
                FightContextual fight = FightManager.Instance.CreateFightContextual(character, targetObjective);

                var cell = character.Map.RandomWalkableCell();

                foreach (var record in records)
                {
                    Monster monster = new Monster(record, cell, MonstersManager.Instance.GetAdaptativeGrade(record, character.SafeLevel));

                    fight.BlueTeam.AddFighter(monster.CreateFighter(fight.BlueTeam));
                }

                fight.RedTeam.AddFighter(character.CreateFighter(fight.RedTeam));

                fight.StartPlacement();

            }
            else
            {
                character.ReplyWarning("Unable to create contextual fight. Empty monsters list.");
            }
        }
    }
}
