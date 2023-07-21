using System;
using System.Data.Common;
using System.Security.Cryptography;
using EPV.Data.DataGetters;
using EPV.Data.Queries;

namespace EPV.Data.DataItems
{
    public class Reference<TDataItem> : Reference
        where TDataItem : DataItem, new()
    {
        public Reference(DbDataReader reader)
        {
            Id = (Guid)reader["Id"];
            Description = (string)reader["Description"];
        }

        public Reference(TDataItem dataItem)
        {
            Id = dataItem.Id;
            Description = dataItem.GetDescription();
        }

        public Reference() { }

        #region Properties


        public static Type ObjectType => typeof(TDataItem);

        #endregion

        #region Methods

        #region GetObject

        public TDataItem GetObject(IDataLink link)
        {
            return link.Load<TDataItem>(Id);
        }

        public TDataItem GetObject()
        {
            return GetObject(DataChannels.DataLink);
        }

        #endregion

        #endregion
    }
}
