using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability;
using NTech.Base.DataBase.Connector.DBService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Base.DataBase.Connector
{
    public class DBConnector
    {
        public static MemoryCache DBConnectorCache;
        static DBConnector()
        {
            DBConnectorCache = MemoryCache.Default;
        }
        public static IDBService Create(string dbName)
        {
            if (DBConnectorCache.Contains(dbName))
            {
                IDBService dBService = null;
                var conn = GetConnectionStringSettings(dbName);
                if (conn == null || string.IsNullOrWhiteSpace(conn.ConnectionString))
                {
                    throw new Exception($"[{dbName}] DB ConnectionString can not be found.");
                }
                switch (conn.ProviderName.ToUpper())
                {
                    case "MSSQL": dBService = new DBServiceSql(conn.ConnectionString);
                        break;
                }
                return dBService;
            }
            else
            {
                IDBService dbService = null;
                var conn = GetConnectionStringSettings(dbName);
                if (conn == null || string.IsNullOrWhiteSpace(conn.ConnectionString))
                {
                    throw new Exception($"[{dbName}] DB ConnectionString can not be found.");
                }
                switch (conn.ProviderName.ToUpper())
                {
                    case "MSSQL": dbService = new DBServiceSql(conn.ConnectionString);
                        break;
                }
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.Default;
                DBConnectorCache.Add(dbName, dbService, policy);

                return dbService;
            }
        }
        public static ConnectionStringSettings GetConnectionStringSettings(string dbName)
        {
            string conString = ConfigurationManager.ConnectionStrings["XmDb"].ToString();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
            
            string ip = builder.DataSource;
            string id = builder.UserID;
            string pwd = builder.Password;
            string providerName = "MSSQL";

            if(!string.IsNullOrWhiteSpace(ip) && !string.IsNullOrWhiteSpace(id) &&
               !string.IsNullOrWhiteSpace(pwd) && !string.IsNullOrWhiteSpace(dbName))
            {
                return new ConnectionStringSettings(dbName
                                                    , $"Server={ip};user id={id};password={pwd};initial catalog={dbName};"
                                                    , providerName);
            }
            else
            {
                return null;
            }
        }
    }
}
