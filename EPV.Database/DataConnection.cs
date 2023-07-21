using System.Data;
using System.Data.Common;

namespace EPV.Database
{
    /// <summary>
    ///     Базовый класс для соединений с данными
    /// </summary>
    /// <typeparam name="TDbConnection">Тип соединения</typeparam>
    /// <typeparam name="TDbCommand">Тип команды</typeparam>
    public abstract class DataConnection<TDbConnection, TDbCommand> : IDataConnection
        where TDbConnection : DbConnection
        where TDbCommand : DbCommand, new()
    {
        public string ConnectionString { get; protected set; }

        protected DataConnection() { }

        public DataConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #region GetConnection

        protected abstract TDbConnection GetConnection();

        #endregion

        #region FillParameters

        protected abstract void FillParameters(TDbCommand command, CommandParameters parameters = null);

        #endregion

        public T ExecuteScalar<T>(string query, CommandParameters parameters = null`) 
        {
            using (TDbConnection connection = GetConnection())
            {
                TDbCommand command = new TDbCommand { CommandText = query, Connection = connection};
                FillParameters(command, parameters);

                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();

                return (T)result;
            }
        }

        public DbDataReader ExecuteReader(string query, CommandParameters parameters = null)
        {
            TDbConnection connection = GetConnection();
            TDbCommand command = new TDbCommand { CommandText = query, Connection = connection};
            FillParameters(command, parameters);

            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public void ExecuteNonQuery(string query, CommandParameters parameters = null)
        {
            using (TDbConnection connection = GetConnection())
            {
                TDbCommand command = new TDbCommand { CommandText = query, Connection = connection};
                FillParameters(command, parameters);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
