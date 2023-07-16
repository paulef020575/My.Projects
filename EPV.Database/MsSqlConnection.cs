using System.Collections.Generic;
using System.Data.SqlClient;

namespace EPV.Database
{
    public class MsSqlConnection : DataConnection<SqlConnection, SqlCommand>
    {
        protected override SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected override void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
        {
            foreach (string key in parameters.Keys)
                command.Parameters.AddWithValue(key, parameters[key]);
        }
    }
}
