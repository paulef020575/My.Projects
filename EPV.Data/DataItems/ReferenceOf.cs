using System;
using System.Data.Common;
using EPV.Data.Queries;

namespace EPV.Data.DataItems
{
    public class Reference<TDataItem> : Reference
        where TDataItem : DataItem
    {
        public Reference(DbDataReader reader)
        {
            Id = (Guid)reader["Id"];
            Description = (string) reader["Description"];
        }

        public Reference() { }

        #region Properties


        public static Type ObjectType => typeof(TDataItem);

        #endregion

 
    }
}
