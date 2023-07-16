using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using EPV.Data.DataItems;
using EPV.Data.Queries;
using FirebirdSql.Data.FirebirdClient;

namespace EPV.Database
{
    public class FirebirdConnection : DataConnection<FbConnection, FbCommand>
    {
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

        protected override void AddParameters(FbCommand command, Dictionary<string, object> parameters)
        {
            foreach (string key in parameters.Keys)
                command.Parameters.AddWithValue(key, parameters[key]);
        }

        public override async Task Save<TDataItem>(TDataItem dataItem)
        {
            string query = QueryBuilder.GetUpdateOrInsertQuery(dataItem);

            using (FbConnection connection = GetConnection())
            {
                FbCommand command = GetCommand(query, connection);
                AddParameters(command, QueryBuilder.GetQueryParameters(dataItem));

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
        }
    }
}
