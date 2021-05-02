using Giny.Core;
using Giny.Core.Commands;
using Giny.Core.Extensions;
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
using Giny.World.Records.Guilds;
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
        [ConsoleCommand("rate")]
        public static void ExperienceRateCommand(double ratio)
        {
            ConfigFile.Instance.XpRate = ratio;

            foreach (var client in WorldServer.Instance.GetOnlineClients())
            {
                client.Character.SendServerExperienceModificator();
            }

            Logger.Write("Experience rate multiplicator is now set to : " + ConfigFile.Instance.XpRate, Channels.Info);


        }
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
                    DatabaseManager.Instance.DeleteTable<GuildRecord>();

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

        [ConsoleCommand("infos")]
        public static void InfosCommand()
        {
            Logger.Write("Connected : " + WorldServer.Instance.Clients.Count, Channels.Info);
            Logger.Write("Ips : " + WorldServer.Instance.Clients.DistinctBy(x => x.Ip).Count(), Channels.Info);
            Logger.Write("Max Connected : " + WorldServer.Instance.MaximumClients, Channels.Info);
        }
        [ConsoleCommand("save")]
        public static void SaveCommand()
        {
            WorldSaveManager.Instance.PerformSave();
            Logger.Write("Server saved.", Channels.Log);
        }

        [ConsoleCommand("npcs")]
        public static void ReloadNpcsCommand()
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
        [ConsoleCommand("items")]
        public static void ReloadItemsCommand()
        {
            DatabaseManager.Instance.Reload<ItemRecord>();
            DatabaseManager.Instance.Reload<WeaponRecord>();

            WeaponRecord.Initialize();
            ItemRecord.Initialize();

            Logger.Write("Items Reloaded.", Channels.Log);
        }


    }
}
