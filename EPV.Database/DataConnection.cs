using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using EPV.Data.DataItems;
using EPV.Data.Queries;

namespace EPV.Database
{
    /// <summary>
    ///     Базовый класс для соединений с данными
    /// </summary>
    /// <typeparam name="TDbConnection">Тип соединения</typeparam>
    /// <typeparam name="TDbCommand">Тип команды</typeparam>
    public abstract class DataConnection<TDbConnection, TDbCommand> : IConnector
        where TDbConnection : DbConnection
        where TDbCommand : DbCommand, new()
    {
        #region Properties

        #region ConnectionString

        /// <summary>
        ///     Строка соединения
        /// </summary>
        public string ConnectionString { get; protected set; }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        ///     защищенный конструктор без параметров
        /// </summary>
        protected DataConnection()
        {
        }

        public DataConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Methods

        #region GetConnection

        /// <summary>
        ///     Возвращает объект для соединения с БД
        /// </summary>
        /// <returns>соедиенение с БД</returns>
        protected abstract TDbConnection GetConnection();

        #endregion

        #region GetCommand

        /// <summary>
        ///     Возвращает команду БД
        /// </summary>
        /// <returns>объект команды</returns>
        protected virtual TDbCommand GetCommand()
        {
            return new TDbCommand();
        }

        protected virtual TDbCommand GetCommand(string commandText, TDbConnection connection = null)
        {
            TDbCommand command = GetCommand();
            command.CommandText = commandText;
            command.Connection = connection;

            return command;
        }

        #endregion

        #region LoadList

        /// <summary>
        ///     Возвращает список объектов, соответствующих данным таблицы
        /// </summary>
        /// <typeparam name="TDataItem">тип объектов</typeparam>
        /// <returns>список объектов</returns>
        public virtual IList<TDataItem> LoadList<TDataItem>()
            where TDataItem : DataItem, new()
        {
            List<TDataItem> dataItems = new List<TDataItem>();

            string query = QueryBuilder.GetLoadListQuery<TDataItem>();
            using (TDbConnection connection = GetConnection())
            {
                TDbCommand command = GetCommand(query, connection);
                connection.Open();
                using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        TDataItem dataItem = new TDataItem();
                        dataItem.ReadProperties(reader);
                        dataItems.Add(dataItem);
                    }

                    reader.Close();
                }
            }

            return dataItems;
        }

        #endregion

        #region Load

        public virtual TDataItem Load<TDataItem>(TDataItem item)
            where TDataItem : DataItem, new()
        {
            string query = QueryBuilder.GetLoadQuery(item),
                idColumn = QueryBuilder.GetIdColumn<TDataItem>();

            using (TDbConnection connection = GetConnection())
            {
                TDbCommand command = GetCommand(query, connection);
                AddParameters(command, new Dictionary<string, object> {{idColumn, item.Id}});

                connection.Open();

                using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        item.ReadProperties(reader);
                        reader.Close();

                        return item;
                    }
                    else
                    {
                        reader.Close();
                        throw new ArgumentException("Not found record with the ID");
                    }

                }
            }
        }

        #endregion

        #region Save

        public virtual void Save<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem, new()
        {
            if (dataItem.Id.Equals(Guid.Empty))
                Insert(dataItem);
            else
                Update(dataItem);
        }

        #endregion

        #region Insert

        public virtual void Insert<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem
        {
            string query = QueryBuilder.GetInsertQuery<TDataItem>();

            using (TDbConnection connection = GetConnection())
            {
                TDbCommand command = GetCommand(query, connection);
                AddParameters(command, QueryBuilder.GetQueryParameters(dataItem));

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        #endregion

        #region Update

        public virtual void Update<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem
        {
            string query = QueryBuilder.GetUpdateQuery<TDataItem>();

            using (TDbConnection connection = GetConnection())
            {
                TDbCommand command = GetCommand(query, connection);
                AddParameters(command, QueryBuilder.GetQueryParameters(dataItem));

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Возвращает текст запроса для удаления объекта данных
        /// </summary>
        /// <typeparam name="TDataItem">тип данных</typeparam>
        /// <param name="item">объект данных</param>
        public virtual void Delete<TDataItem>(TDataItem item)
            where TDataItem : DataItem, new()
        {
            string query = QueryBuilder.GetDeleteQuery(item),
                idColumn = QueryBuilder.GetIdColumn<TDataItem>();

            using (TDbConnection connection = GetConnection())
            {
                TDbCommand command = GetCommand(query, connection);
                AddParameters(command, new Dictionary<string, object> { { idColumn, item.Id } });

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        #endregion

        #region AddParameters

        protected abstract void AddParameters(TDbCommand command, Dictionary<string, object> parameters);

        #endregion

        #endregion
    }
}
