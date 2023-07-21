using System;
using System.Collections.Generic;
using System.Data.Common;
using EPV.Data.Conditions;
using EPV.Data.DataItems;
using EPV.Data.Queries;
using EPV.Database;

namespace EPV.Data.DataGetters
{
    public abstract class DbDataLink<TDataConnection> : IDataLink
        where TDataConnection : IDataConnection
    {
        protected TDataConnection DataConnection { get; set; }


        public virtual TDataItem Load<TDataItem>(Guid id) where TDataItem : DataItem, new()
        {
            TDataItem dataItem = new TDataItem();
            string query = QueryBuilder.GetLoadQuery(dataItem);
            CommandParameters commandParameters = new CommandParameters();
            commandParameters.Add(QueryBuilder.GetIdColumn<TDataItem>(), id);
            using (DbDataReader reader = DataConnection.ExecuteReader(query, commandParameters))
            {
                if (reader.Read())
                {
                    dataItem.ReadProperties(reader);
                    reader.Close();
                    return dataItem;
                }

                reader.Close();
                throw new ArgumentOutOfRangeException("неверный идентификатор");
            }
        }

        public virtual void Load<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new()
        {
            TDataItem source = Load<TDataItem>(dataItem.Id);
            dataItem.CopyFrom(source);
        }

        public virtual IList<TDataItem> LoadList<TDataItem>() where TDataItem : DataItem, new()
        {
            List<TDataItem> dataItems = new List<TDataItem>();
            string query = QueryBuilder.GetLoadListQuery<TDataItem>();

            using (DbDataReader reader = DataConnection.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    TDataItem dataItem = new TDataItem();
                    dataItem.ReadProperties(reader);
                    dataItems.Add(dataItem);
                }

                reader.Close();
            }

            return dataItems;
        }

        public virtual void Save<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new()
        {
            if (dataItem.Id == Guid.Empty)
                Insert(dataItem);
            else
                Update(dataItem);
        }

        protected virtual void Update<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new()
        {
            string query = QueryBuilder.GetUpdateQuery(dataItem);
            CommandParameters parameters = QueryBuilder.GetQueryParameters(dataItem);

            DataConnection.ExecuteNonQuery(query, parameters);
        }

        protected virtual void Insert<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new()
        {
            dataItem.Id = Guid.NewGuid();

            string query = QueryBuilder.GetInsertQuery(dataItem);
            CommandParameters parameters = QueryBuilder.GetQueryParameters(dataItem);

            DataConnection.ExecuteNonQuery(query, parameters);
        }

        public virtual void Delete<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new()
        {
            string query = QueryBuilder.GetDeleteQuery(dataItem);
            CommandParameters parameters 
                = new CommandParameters {{QueryBuilder.GetIdColumn<TDataItem>(), dataItem.Id}};

            DataConnection.ExecuteNonQuery(query, parameters);
        }

        public virtual IList<Reference<TDataItem>> LoadReferences<TDataItem>() where TDataItem : DataItem, new()
        {
            string query = QueryBuilder.GetReferencesList<TDataItem>();
            IList<Reference<TDataItem>> references = new List<Reference<TDataItem>>();
            using (DbDataReader reader = DataConnection.ExecuteReader(query))
            {
                while (reader.Read())
                    references.Add(new Reference<TDataItem>(reader));

                reader.Close();
            }

            return references;
        }

        public virtual IList<TDataItem> LoadChildren<TDataItem>(string parentColumn, Guid id) where TDataItem : DataItem, new()
        {
            QueryCondition condition = new QueryCondition(parentColumn, Comparison.Equal, id);
            string query = QueryBuilder.GetLoadListQuery<TDataItem>(condition);
            CommandParameters parameters = condition.GetParameters();

            IList<TDataItem> result = new List<TDataItem>();
            using (DbDataReader reader = DataConnection.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    TDataItem dataItem = new TDataItem();
                    dataItem.ReadProperties(reader);
                    result.Add(dataItem);
                }
            }

            return result;
        }


    }
}
