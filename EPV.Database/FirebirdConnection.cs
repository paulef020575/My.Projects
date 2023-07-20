using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace EPV.Database
{
    public class FirebirdConnection : DataConnection<FbConnection, FbCommand>
    {
        private FirebirdConnection() { }

        public FirebirdConnection(string connectionString) : base(connectionString) { }

        public FirebirdConnection(string server, string database, 
            string userId = "SYSDBA", string password = "masterkey")
        {
            FbConnectionStringBuilder connectionStringBuilder = new FbConnectionStringBuilder
            {
                DataSource = server,
                Database = database,
                UserID = userId,
                Password = password
            };

            ConnectionString = connectionStringBuilder.ConnectionString;
        }

        protected override FbConnection GetConnection()
        {
            return new FbConnection(ConnectionString);
        }

        protected override void FillParameters(FbCommand command, CommandParameters parameters = null)
        {
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                    command.Parameters.AddWithValue(key, parameters[key]);
            }
        }
    }
}
