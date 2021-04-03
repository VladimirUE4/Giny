using Giny.Core;
using Giny.Core.Commands;
using Giny.ORM;
using Giny.Protocol.Enums;
using Giny.Protocol.IPC.Messages;
using Giny.World.Managers;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Entities.Npcs;
using Giny.World.Managers.Maps.Npcs;
using Giny.World.Network;
using Giny.World.Records;
using Giny.World.Records.Accounts;
using Giny.World.Records.Characters;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using Giny.World.Records.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World
{
    class WorldCommands
    {
        [ConsoleCommand("reset")]
        public static void ResetCommand()
        {
            Logger.Write("Reset world...", Channels.Log);

            IPCManager.Instance.SendRequest(new ResetWorldRequestMessage(),
            delegate (ResetWorldResultMessage msg)
            {
                if (msg.success)
                {
                    foreach (var character in CharacterRecord.GetCharacterRecords().ToArray())
                    {
                        CharacterManager.Instance.DeleteCharacter(character);
                    }

                    DatabaseManager.Instance.DeleteTable<WorldAccountRecord>();
                    DatabaseManager.Instance.DeleteTable<BankItemRecord>();
                    DatabaseManager.Instance.DeleteTable<MerchantItemRecord>();
                    DatabaseManager.Instance.DeleteTable<MerchantRecord>();
                    DatabaseManager.Instance.DeleteTable<BidShopItemRecord>();
                    DatabaseManager.Instance.DeleteTable<CharacterItemRecord>();
                    DatabaseManager.Instance.DeleteTable<BidShopItemRecord>();

                    Environment.Exit(0);
                }
                else
                {
                    Logger.Write("AuthServer is unable to validate world reset.", Channels.Warning);
                }
            },
           delegate ()
           {
               Logger.Write("AuthServer is unable to validate world reset. timeout", Channels.Warning);
           });
        }

        [ConsoleCommand("save")]
        public static void SaveCommand()
        {
            WorldSaveManager.Instance.PerformSave();
            Logger.Write("Server saved.", Channels.Log);
        }

        [ConsoleCommand("npcs")]
        public static void UpdateNpcsCommad()
        {
            DatabaseManager.Instance.Reload<NpcSpawnRecord>();
            DatabaseManager.Instance.Reload<NpcReplyRecord>();
            DatabaseManager.Instance.Reload<NpcActionRecord>();

            NpcSpawnRecord.Initialize();

            foreach (var map in MapRecord.GetMaps())
            {
                foreach (var npc in map.Instance.GetEntities<Npc>())
                {
                    map.Instance.RemoveEntity(npc.Id);
                }
                map.Instance.Reload();
            }

            NpcsManager.Instance.SpawnNpcs();

            Logger.Write("Npc Reloaded.", Channels.Log);
        }

        [ConsoleCommand("test")]
        public static void TestCommand()
        {
            DungeonRecord record = new DungeonRecord();

            record.Id = 1;
            record.Name = "Cour du Bouftou Royal";

            record.EntranceMapId = 120063489;
            record.ExitMapId = 120063489;

            record.Rooms = new Dictionary<long, MonsterRoom>();

            record.Rooms.Add(121373185, new MonsterRoom(10f, 134, 101, 148, 149, 148, 101, 134, 149));
            record.Rooms.Add(121374209, new MonsterRoom(10f, 134, 148, 4822, 149, 149, 4822, 148, 134));
            record.Rooms.Add(121375233, new MonsterRoom(10f, 134, 101, 4822, 149, 134, 149, 4822, 101));
            record.Rooms.Add(121373187, new MonsterRoom(10f, 134, 101, 148, 149, 101, 134, 149, 148));
            record.Rooms.Add(121374211, new MonsterRoom(10f, 147, 4822, 148, 101, 4822, 4822, 148, 101));

            record.AddInstantElement();
        }
    }
}
