using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Giny.World
{
    public class ConfigFile
    {
        public const string CONFIG_PATH = "config.json";

        public static ConfigFile Instance
        {
            get;
            private set;
        }
        public short ServerId
        {
            get;
            set;
        }
        public string Host
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
        public string SQLHost
        {
            get;
            set;
        }
        public string SQLUser
        {
            get;
            set;
        }
        public string SQLPassword
        {
            get;
            set;
        }
        public string SQLDBName
        {
            get;
            set;
        }
        public string IPCHost
        {
            get;
            set;
        }
        public int IPCPort
        {
            get;
            set;
        }
        public long SpawnMapId
        {
            get;
            set;
        }
        public short SpawnCellId
        {
            get;
            set;
        }
        public short MaxMerchantPerMap
        {
            get;
            set;
        }
        public short ApLimit
        {
            get;
            set;
        }
        public short MpLimit
        {
            get;
            set;
        }
        public short StartLevel
        {
            get;
            set;
        }
        public short StartAp
        {
            get;
            set;
        }
        public short StartMp
        {
            get;
            set;
        }
        public string WelcomeMessage
        {
            get;
            set;
        }

        [StartupInvoke("Config", StartupInvokePriority.Initial)]
        public static void Initialize()
        {
            if (File.Exists(CONFIG_PATH))
            {
                try
                {
                    Instance = Json.Deserialize<ConfigFile>(File.ReadAllText(CONFIG_PATH));
                }
                catch
                {
                    CreateConfig();
                }

            }
            else
            {
                CreateConfig();
            }
        }
        public static void CreateConfig()
        {
            Instance = Default();
            Save();
            Logger.Write("Configuration file created!", MessageState.SUCCES);
        }
        public static void Save()
        {
            File.WriteAllText(CONFIG_PATH, Json.Serialize(Instance));
        }

        public static ConfigFile Default()
        {
            return new ConfigFile()
            {
                ServerId = 1,
                Host = "127.0.0.1",
                Port = 5555,
                SQLHost = "127.0.0.1",
                SQLDBName = "giny_world",
                SQLPassword = "",
                SQLUser = "root",
                IPCHost = "127.0.0.1",
                IPCPort = 800,
                SpawnMapId = 154010883,
                SpawnCellId = 400,
                ApLimit = 12,
                MpLimit = 6,
                StartLevel = 5,
                StartAp = 6,
                StartMp = 3,
                WelcomeMessage = "-> Pandala Alpha <-",
                MaxMerchantPerMap = 5,
            };
        }
    }
}
