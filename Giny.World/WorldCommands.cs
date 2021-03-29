﻿using Giny.Core;
using Giny.Core.Commands;
using Giny.ORM;
using Giny.Protocol.Enums;
using Giny.Protocol.IPC.Messages;
using Giny.World.Managers;
using Giny.World.Managers.Entities.Characters;
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
            Logger.Write("Reset world...", MessageState.IMPORTANT_INFO);

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
                    Logger.Write("AuthServer is unable to validate world reset.", MessageState.WARNING);
                }
            },
           delegate ()
           {
               Logger.Write("AuthServer is unable to validate world reset. timeout", MessageState.WARNING);
           });
        }

        [ConsoleCommand("save")]
        public static void SaveCommand()
        {
            WorldSaveManager.Instance.PerformSave();
            Logger.Write("Server saved.", MessageState.IMPORTANT_INFO);
        }

        [ConsoleCommand("npcs")]
        public static void UpdateNpcsCommad()
        {
            if (WorldServer.Instance.Status == ServerStatusEnum.ONLINE)
            {
                WorldServer.Instance.SetServerStatus(ServerStatusEnum.NOJOIN);

                DatabaseManager.Instance.Reload<NpcSpawnRecord>();

                NpcSpawnRecord.Initialize();

                foreach (var map in MapRecord.GetMaps())
                {
                    map.Instance.Reload();
                }

                Logger.Write("Npc Reloaded.", MessageState.IMPORTANT_INFO);

                WorldServer.Instance.SetServerStatus(ServerStatusEnum.ONLINE);
            }
            else
            {
                Logger.Write("Unable to reload npcs... server is busy.", MessageState.WARNING);
            }
        }
    }
}
