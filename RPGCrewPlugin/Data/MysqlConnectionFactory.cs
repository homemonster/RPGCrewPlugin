using System;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using NLog;
using RPGCrewPlugin.Data.Schema;

namespace RPGCrewPlugin.Data
{
    public class MysqlConnectionFactory : IConnectionFactory
    {
        private static readonly Logger Log = LogManager.GetLogger("Economy.MysqlConnectionFactory");
        
        public void Setup()
        {
            using (var connection = Open())
            {
                try
                {
                    var schemaGenerator = new MySQLSchemaGenerator();
                    var sql = schemaGenerator.Generate();
                    connection.Execute(sql);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public IDbConnection Open()
        {
            var connection = new MySqlConnection(RPGCrewPlugin.Instance.Config.MySQLConnectionString);
            connection.Open();
            
            return connection;
        }
    }
}
