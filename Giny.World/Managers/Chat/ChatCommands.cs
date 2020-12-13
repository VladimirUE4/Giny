using Giny.Core.Extensions;
using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Actions;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Npcs;
using Giny.World.Managers.Experiences;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Generic;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Maps.Npcs;
using Giny.World.Managers.Maps.Paths;
using Giny.World.Managers.Maps.Teleporters;
using Giny.World.Managers.Monsters;
using Giny.World.Network;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using Giny.World.Records.Tinsel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Giny.World.Managers.Chat
{
    class ChatCommands
    {
        [ChatCommand("help", ServerRoleEnum.PLAYER)]
        public static void HelpCommand(WorldClient client)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Available commands:" + "\n");

            foreach (var command in ChatCommandsManager.Instance.GetCommandsAttribute())
            {
                if (command.RequiredRole <= client.Account.Role)
                {
                    sb.Append("-" + command.Name + "\n");
                }
            }

            client.Character.ReplyWarning(sb.ToString());
        }
        [ChatCommand("donjon", ServerRoleEnum.ADMINISTRATOR)]
        public static void SpawnDungeonMonsterCommand(WorldClient client, string monsters, int nextMapId)
        {
            DungeonMapRecord dungeonMap = new DungeonMapRecord();
            dungeonMap.Id = client.Character.Map.Id;
            dungeonMap.Monsters = monsters.Split(',').Select(x => short.Parse(x)).ToList();
            dungeonMap.RespawnDelay = 10f;
            dungeonMap.NextMapId = nextMapId;

            dungeonMap.AddInstantElement();

            client.Character.Map.ReloadMembers();
            client.Character.Map.Instance.Reload();
        }
        [ChatCommand("monsters", ServerRoleEnum.ADMINISTRATOR)]
        public static void SpawnMonstersCommand(WorldClient client, string monsters)
        {
            IEnumerable<MonsterRecord> records = monsters.Split(',').Select(x => MonsterRecord.GetMonsterRecord(short.Parse(x)));
            MonstersManager.Instance.AddFixedMonsterGroup(client.Character.Map.Instance, client.Character.CellId, records.ToArray());
        }
        [ChatCommand("elements", ServerRoleEnum.ADMINISTRATOR)]
        public static void ElementsCommand(WorldClient client)
        {
            InteractiveElementRecord[] elements = client.Character.Map.Elements;

            if (elements.Count() == 0)
            {
                client.Character.Reply("No Elements on Map...");
                return;
            }

            var colors = CollectionsExtensions.RandomColors(elements.Count());

            client.Character.DebugClearHighlightCells();

            for (int i = 0; i < elements.Count(); i++)
            {
                var ele = elements[i];
                client.Character.DebugHighlightCells(colors[i], new CellRecord[] { client.Character.Map.GetCell(ele.CellId) });
                client.Character.Reply("Id: " + ele.Identifier + " Cell:" + ele.CellId + " Bones:" + ele.BonesId, colors[i]);
            }
        }
        [ChatCommand("rdmap", ServerRoleEnum.ADMINISTRATOR)]
        public static void TeleportToRandomMapInSubarea(WorldClient client, short subareaId)
        {
            client.Character.Teleport(MapRecord.GetMaps().Where(x => x.Subarea.Id == subareaId).Random());
        }
        [ChatCommand("map", ServerRoleEnum.ADMINISTRATOR)]
        public static void MapCommand(WorldClient client)
        {
            client.Character.Reply(client.Character.Map.Instance.ToString(), Color.CornflowerBlue);
        }
        [ChatCommand("addnpcshop", ServerRoleEnum.ADMINISTRATOR)]
        public static void AddNpcCommand(WorldClient client, short templateId, int itemType)
        {
            NpcsManager.Instance.AddNpcShop((int)client.Character.Map.Id, client.Character.CellId, client.Character.Direction, templateId, itemType);
        }
        [ChatCommand("addnpc", ServerRoleEnum.ADMINISTRATOR)]
        public static void AddNpcCommand(WorldClient client, short templateId)
        {
            NpcsManager.Instance.AddNpc((int)client.Character.Map.Id, client.Character.CellId, client.Character.Direction, templateId);
        }
        [ChatCommand("mvnpc", ServerRoleEnum.ADMINISTRATOR)]
        public static void MoveNpcCommand(WorldClient client, long spawnId)
        {
            NpcsManager.Instance.MoveNpc(spawnId, (int)client.Character.Map.Id, client.Character.CellId, client.Character.Direction);
        }
        [ChatCommand("rmnpc", ServerRoleEnum.ADMINISTRATOR)]
        public static void RemoveNpcCommand(WorldClient client, long spawnId)
        {
            NpcsManager.Instance.RemoveNpc(spawnId);
        }
        [ChatCommand("start", ServerRoleEnum.PLAYER)]
        public static void StartCommand(WorldClient client)
        {
            client.Character.Teleport(ConfigFile.Instance.SpawnMapId, ConfigFile.Instance.SpawnCellId);
        }
        [ChatCommand("itemlist", ServerRoleEnum.ADMINISTRATOR)]
        public static void ItemListCommand(WorldClient client, short id)
        {
            var items = ItemRecord.GetItems().Where(x => x.TypeId == id).OrderBy(x => x.Level);

            client.Character.Reply(string.Join(",", items.Select(x => x.Id.ToString())));
        }
        [ChatCommand("title", ServerRoleEnum.GAMEMASTER_PADAWAN)]
        public static void AddTitleCommand(WorldClient client, short id)
        {
            if (TitleRecord.Exists(id))
            {
                client.Character.LearnTitle(id);
            }
            else
            {
                client.Character.ReplyWarning("This title do not exists.");
            }
        }
        [ChatCommand("ornament", ServerRoleEnum.GAMEMASTER_PADAWAN)]
        public static void AddOrnamentCommand(WorldClient client, short id)
        {
            if (OrnamentRecord.Exists(id))
            {
                client.Character.LearnOrnament(id, true);
            }
            else
            {
                client.Character.ReplyWarning("This ornament do not exists.");
            }
        }
        [ChatCommand("addzaap", ServerRoleEnum.ADMINISTRATOR)]
        public static void AddZaapCommand(WorldClient client, int elementId, int zoneId)
        {
            TeleportersManager.Instance.AddDestination(
                TeleporterTypeEnum.TELEPORTER_ZAAP,
                InteractiveTypeEnum.ZAAP,
                GenericActionEnum.ZAAP,
                client.Character.Map,
                client.Character.Map.GetElementRecord(elementId),
                zoneId);
        }
        [ChatCommand("addzaapi", ServerRoleEnum.ADMINISTRATOR)]
        public static void AddZaapiCommand(WorldClient client, int elementId, int zoneId)
        {
            TeleportersManager.Instance.AddDestination(
                TeleporterTypeEnum.TELEPORTER_SUBWAY,
                InteractiveTypeEnum.ZAAPI,
                GenericActionEnum.ZAAPI,
                client.Character.Map,
                client.Character.Map.GetElementRecord(elementId),
                zoneId);
        }
        [ChatCommand("nocollide", ServerRoleEnum.GAMEMASTER_PADAWAN)]
        public static void NoCollideCommand(WorldClient client)
        {
            List<MapObstacle> obstacles = new List<MapObstacle>();

            for (short i = 0; i < 560; i++)
            {
                obstacles.Add(new MapObstacle(i, 1));
            }
            client.Send(new MapObstacleUpdateMessage(obstacles.ToArray()));
            client.Character.Reply("No collide.", true);
        }
        [ChatCommand("level", ServerRoleEnum.GAMEMASTER_PADAWAN)]
        public static void LevelCommand(WorldClient client, short newLevel)
        {
            if (newLevel <= client.Character.Level)
            {
                client.Character.ReplyWarning("New level must be superior to <b>" + client.Character.Level + "</b>");
                return;
            }
            client.Character.SetExperience(ExperienceManager.Instance.GetCharacterXPForLevel(newLevel));
        }
        [ChatCommand("exp", ServerRoleEnum.GAMEMASTER_PADAWAN)]
        public static void AddExpCommand(WorldClient client, long amount)
        {
            client.Character.AddExperience(amount);
        }
        [ChatCommand("go", ServerRoleEnum.GAMEMASTER_PADAWAN)]
        public static void TeleportCommand(WorldClient client, int mapId)
        {
            client.Character.Teleport(mapId);
        }
        [ChatCommand("item", ServerRoleEnum.ADMINISTRATOR)]
        public static void AddItemCommand(WorldClient client, short itemId, int quantity)
        {
            client.Character.Inventory.AddItem(itemId, quantity, true);
            client.Character.OnItemGained(itemId, quantity);
        }
        [ChatCommand("relative", ServerRoleEnum.ADMINISTRATOR)]
        public static void RelativeMapCommand(WorldClient client)
        {
            var maps = MapRecord.GetMaps(client.Character.Map.Position.Point).ToList();

            int index = maps.IndexOf(client.Character.Map);

            if (maps.Count > index + 1)
                client.Character.Teleport((int)maps[index + 1].Id);
            else
                client.Character.Teleport((int)maps[0].Id);
        }
        [ChatCommand("goto", ServerRoleEnum.GAMEMASTER_PADAWAN)]
        public static void GotoCommand(WorldClient client, string targetName)
        {
            var target = WorldServer.Instance.GetConnectedClient(targetName);

            if (target != null)
            {
                client.Character.Teleport((int)target.Character.Map.Id);
            }
            else
            {
                client.Character.ReplyWarning("<b>" + targetName + "</b> is not connected.");
            }
        }
        [ChatCommand("spell", ServerRoleEnum.ADMINISTRATOR)]
        public static void LearnSpell(WorldClient client, short spellId)
        {
            client.Character.LearnSpell(spellId, true);
        }
        [ChatCommand("kamas", ServerRoleEnum.GAMEMASTER_PADAWAN)]
        public static void AddKamas(WorldClient client, long amount)
        {
            client.Character.AddKamas(amount);
            client.Character.OnKamasGained(amount);
        }
        [ChatCommand("doom", ServerRoleEnum.ADMINISTRATOR)]
        public static void LeafCommand(WorldClient client)
        {
            if (client.Character.Fighting)
            {
                foreach (var target in client.Character.Fighter.EnemyTeam.GetFighters<Fighter>())
                {
                    target.Stats.LifePoints = 0;
                }

                client.Character.Fighter.Fight.CheckDeads();
                client.Character.Fighter.Fight.CheckFightEnd();
            }
        }
        [ChatCommand("test", ServerRoleEnum.ADMINISTRATOR)]
        public static void TestCommand(WorldClient client)
        {
            IEnumerable<MonsterRecord> records = MonsterRecord.GetMonsterRecords().Where(x => x.IsBoss == true).Shuffle().Take(5);
            MonstersManager.Instance.AddFixedMonsterGroup(client.Character.Map.Instance, client.Character.CellId, records.ToArray());

        }
    }
}
