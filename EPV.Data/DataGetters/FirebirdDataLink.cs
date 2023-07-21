using EPV.Data.Queries;
using EPV.Database;

namespace EPV.Data.DataGetters
{
    public class FirebirdDataLink : DbDataLink<FirebirdConnection>
    {
        public FirebirdDataLink(string connectionString)
        {
            DataConnection = new FirebirdConnection(connectionString);
        }

        public FirebirdDataLink(string server, string database, string userId = "SYSDBA", string password = "masterkey") 
        {
            DataConnection = new FirebirdConnection(server, database, userId, password);
        }


        public override void Save<TDataItem>(TDataItem dataItem)
        {
            string query = QueryBuilder.GetUpdateOrInsertQuery(dataItem);
            DataConnection.ExecuteNonQuery(query, QueryBuilder.GetQueryParameters(dataItem));
        }
    }
}
