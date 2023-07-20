using System.Collections.Generic;
using System.Data.SqlClient;

namespace EPV.Database
{
    public class MsSqlConnection : DataConnection<SqlConnection, SqlCommand>
    {
        private MsSqlConnection() { }

        protected override SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected override void FillParameters(SqlCommand command, CommandParameters parameters = null)
        {
            if (parameters != null)
                foreach (string key in parameters.Keys)
                    command.Parameters.AddWithValue(key, parameters[key]);
        }

    }
}
