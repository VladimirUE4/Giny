using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using Giny.ORM.IO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Giny.ORM
{
    public class DatabaseManager : Singleton<DatabaseManager>
    {
        public static object SyncRoot = new object();

        private MySqlConnection ConnectionProvider
        {
            get;
            set;
        }

        public Type[] TableTypes
        {
            get;
            private set;
        }

        public event Action<Type, string> OnStartLoadTable;

        public event Action<Type, string> OnEndLoadTable;

        public event Action<string, double> OnLoadProgress;

        public void Initialize(Assembly recordsAssembly, string host, string database, string user, string password)
        {
            if (ConnectionProvider != null)
            {
                throw new Exception("There is already an instance of DatabaseManager.");
            }
            this.ConnectionProvider = new MySqlConnection(string.Format("Server={0};UserId={1};Password={2};Database={3}", host, user, password, database));
            this.TableTypes = Array.FindAll(recordsAssembly.GetTypes(), x => x.GetInterface("ITable") != null);
            TableManager.Instance.Initialize(TableTypes);
        }

        public MySqlConnection UseProvider()
        {
            return UseProvider(ConnectionProvider);
        }


        private MySqlConnection UseProvider(MySqlConnection connection)
        {
            if (!connection.Ping())
            {
                connection.Close();
                connection.Open();
            }

            return connection;
        }
        public void Reload<T>() where T : ITable
        {
            var type = typeof(T);
            TableManager.Instance.ClearContainer(type);
            DatabaseReader reader = new DatabaseReader(type);
            reader.Read(UseProvider());

            foreach (ITable element in reader.Elements.Values)
            {
                TableManager.Instance.AddToContainer(element);
            }
        }
        public void LoadTables()
        {
            var orderedTables = new Type[TableTypes.Length];

            var dontCatch = new List<Type>();

            foreach (var tableType in TableTypes)
            {
                var definition = TableManager.Instance.GetDefinition(tableType);
                var attribute = definition.TableAttribute;

                if (attribute.Load)
                {
                    if (attribute.ReadingOrder >= 0)
                        orderedTables[attribute.ReadingOrder] = tableType;
                }
                else
                    dontCatch.Add(tableType);
            }
            foreach (var table in TableTypes)
            {
                if (orderedTables.Contains(table) || dontCatch.Contains(table))
                    continue;

                for (var i = TableTypes.Length - 1; i >= 0; i--)
                {
                    if (orderedTables[i] == null)
                    {
                        orderedTables[i] = table;
                        break;
                    }
                }
            }
            foreach (var type in orderedTables)
            {
                if (type != null)
                    LoadTable(type);
            }
        }
        private void LoadTable(Type type)
        {
            var reader = new DatabaseReader(type);
            var tableName = reader.TableName;
            OnStartLoadTable?.Invoke(type, tableName);
            reader.Read(this.UseProvider());
            OnEndLoadTable?.Invoke(type, tableName);
        }
        public void LoadTable<T>() where T : ITable
        {
            LoadTable(typeof(T));
        }
        public void CloseProvider()
        {
            this.ConnectionProvider.Close();
        }

        public void DropAllTablesIfExists()
        {
            foreach (var type in TableTypes)
            {
                var definition = TableManager.Instance.GetDefinition(type);
                TableAttribute attribute = definition.TableAttribute;
                DropTableIfExists(attribute.TableName);
            }
        }
        public void DropTableIfExists(string tableName)
        {
            Query(string.Format(QueryConstants.DROP_TABLE, tableName), UseProvider());
        }
        public void DropTableIfExists(Type type)
        {
            var definition = TableManager.Instance.GetDefinition(type);
            DropTableIfExists(definition.TableAttribute.TableName);
        }
        public void DropTableIfExists<T>() where T : ITable
        {
            DropTableIfExists(TableManager.Instance.GetDefinition(typeof(T)).TableAttribute.TableName);
        }
        public void Query(string query, MySqlConnection connection)
        {
            lock (SyncRoot)
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Logger.Write("Unable to query (" + query + ")" + ex, MessageState.ERROR);
                    }
                }
            }
        }

        public void OnProgress(string tableName, double ratio)
        {
            OnLoadProgress?.Invoke(tableName, ratio);
        }

        public void DeleteTable<T>() where T : ITable
        {
            var definition = TableManager.Instance.GetDefinition(typeof(T));
            DeleteTable(definition.TableAttribute.TableName);
        }
        public void DeleteTable(string tableName)
        {
            Query(string.Format(QueryConstants.DELETE_TABLE, tableName), UseProvider());
        }
        public void CreateTableIfNotExists(Type type)
        {
            var definition = TableManager.Instance.GetDefinition(type);

            string tableName = definition.TableAttribute.TableName;

            PropertyInfo primaryProperty = definition.PrimaryProperty;

            string str = string.Empty;

            foreach (var property in definition.Properties)
            {
                string pType = QueryConstants.ConvertType(property);
                str += property.Name + " " + pType + ",";
            }

            if (primaryProperty != null)
                str += "PRIMARY KEY (" + primaryProperty.Name + ")";
            else
                str = str.Remove(str.Length - 1, 1);

            this.Query(string.Format(QueryConstants.CREATE_TABLE, tableName, str), UseProvider());

        }
        public void CreateAllTablesIfNotExists()
        {
            foreach (var type in TableTypes)
            {
                CreateTableIfNotExists(type);
            }
        }

        public void CreateTableIfNotExists<T>() where T : ITable
        {
            CreateTableIfNotExists(typeof(T));
        }

    }
}
