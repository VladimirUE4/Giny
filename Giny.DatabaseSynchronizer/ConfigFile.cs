﻿using Giny.Core;
using Giny.Core.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Giny.DatabaseSynchronizer
{
    public class ConfigFile
    {
        public const string CONFIG_PATH = "config.json";

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
            if (!Initialize())
            {
                MessageBox.Show("Please select Dofus.exe file.", "Hello", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "EXE files (*.exe) | *.exe";

                if (openFileDialog.ShowDialog() == true)
                {
                    string path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                    if (!ConfigFile.IsValidDofusPath(path))
                    {
                        MessageBox.Show("This is not a correct dofus directory.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        LoadConfig();
                    }
                    else
                    {
                        CreateConfig(path);
                    }
                }
            }
        }

        private static bool Initialize()
        {
            if (File.Exists(CONFIG_PATH))
            {
                try
                {
                    Instance = Json.Deserialize<ConfigFile>(File.ReadAllText(CONFIG_PATH));
                    return true;
                }
                catch
                {
                    File.Delete(CONFIG_PATH);
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
            Logger.Write("Configuration file created!", MessageState.SUCCES);
        }
        public static void Save()
        {
            File.WriteAllText(CONFIG_PATH, Json.Serialize(Instance));
        }

        private static bool IsValidDofusPath(string path)
        {
            string combined = Path.Combine(path, @"content/maps");
            return Directory.Exists(combined);
        }
    }
}
