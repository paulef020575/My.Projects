using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace EPV.Database
{
    public interface IDataConnection
    {
        T ExecuteScalar<T>(string query, CommandParameters parameters = null);

        DbDataReader ExecuteReader(string query, CommandParameters parameters = null);

        void ExecuteNonQuery(string query, CommandParameters parameters = null);
    }
}
