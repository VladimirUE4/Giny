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
    }
}
