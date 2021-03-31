using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
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
        private UpdateLogger m_logger;

        [StartupInvoke("Database", StartupInvokePriority.SecondPass)]
        public void InitializeDatabase()
        {
            DatabaseManager.Instance.OnLoadProgress += OnLoadProgress;
            DatabaseManager.Instance.OnStartLoadTable += OnStartLoad;

            DatabaseManager.Instance.Initialize(Assembly.GetExecutingAssembly(), ConfigFile.Instance.SQLHost,
               ConfigFile.Instance.SQLDBName, ConfigFile.Instance.SQLUser, ConfigFile.Instance.SQLPassword);

            DatabaseManager.Instance.LoadTables();

        }

        private void OnStartLoad(Type type, string tableName)
        {
            Logger.Write("Loading " + tableName.FirstCharToUpper() + " ...", Channels.Log);
            m_logger = new UpdateLogger();
        }

        private void OnLoadProgress(string tableName, double ratio)
        {
            m_logger.Update((int)(ratio * 100));
        }
    }
}
