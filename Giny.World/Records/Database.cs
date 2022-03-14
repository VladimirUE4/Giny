using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using Giny.Core.Logs;
using Giny.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records
{
    public class Database : Singleton<Database>
    {
        ProgressLogger ProgressLogger = new ProgressLogger();

        [StartupInvoke("Database", StartupInvokePriority.SecondPass)]
        public void InitializeDatabase()
        {
            DatabaseManager.Instance.OnLoadProgress += OnLoadProgress;

            DatabaseManager.Instance.Initialize(Assembly.GetExecutingAssembly(), ConfigFile.Instance.SQLHost,
               ConfigFile.Instance.SQLDBName, ConfigFile.Instance.SQLUser, ConfigFile.Instance.SQLPassword);

            DatabaseManager.Instance.LoadTables();

            ProgressLogger.Flush();

            DatabaseManager.Instance.OnLoadProgress -= OnLoadProgress;
        }


        private void OnLoadProgress(string tableName, int currentIndex, int length)
        {
            ProgressLogger.WriteProgressBar(currentIndex, length);
        }
    }
}
