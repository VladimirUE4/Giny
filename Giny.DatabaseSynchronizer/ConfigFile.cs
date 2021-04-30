using Giny.Core;
using Giny.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DatabaseSynchronizer
{
    public class ConfigFile
    {
        public const string ConfigPath = "config.json";

        public static ConfigFile Instance
        {
            get;
            private set;
        }
        public string ClientPath
        {
            get;
            set;
        }
        public static void LoadConfig()
        {
            if (!Initialize() || !IsValidDofusPath(Instance.ClientPath))
            {
                CreateConfig(string.Empty);
                Logger.Write("Invalid Dofus path, Please modify " + ConfigPath + ".", Channels.Warning);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        private static bool Initialize()
        {
            if (File.Exists(ConfigPath))
            {
                try
                {
                    Instance = Json.Deserialize<ConfigFile>(File.ReadAllText(ConfigPath));
                    return true;
                }
                catch
                {
                    File.Delete(ConfigPath);
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        public static void CreateConfig(string clientPath)
        {
            Instance = new ConfigFile()
            {
                ClientPath = clientPath,
            };

            Save();
            Logger.Write("Configuration file created!", Channels.Info);
        }
        public static void Save()
        {
            File.WriteAllText(ConfigPath, Json.Serialize(Instance));
        }

        private static bool IsValidDofusPath(string path)
        {
            string combined = Path.Combine(path, @"content/maps");
            return Directory.Exists(combined);
        }
    }
}
