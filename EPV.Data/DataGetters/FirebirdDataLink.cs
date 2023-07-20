using EPV.Data.Queries;
using EPV.Database;

namespace EPV.Data.DataGetters
{
    public class FirebirdDataLink : DbDataLink<FirebirdConnection>
    {
        public override void Save<TDataItem>(TDataItem dataItem)
        {
            string query = QueryBuilder.GetUpdateOrInsertQuery(dataItem);
            DataConnection.ExecuteNonQuery(query, QueryBuilder.GetQueryParameters(dataItem));
        }
    }
}
