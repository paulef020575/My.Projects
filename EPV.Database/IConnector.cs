using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPV.Data.DataItems;

namespace EPV.Database
{
    public interface IConnector
    {
        IList<TDataItem> LoadList<TDataItem>() where TDataItem : DataItem, new();

        TDataItem Load<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new();

        void Save<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new();

        void Delete<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new();

        IList<Reference<TDataItem>> LoadReferences<TDataItem>() where TDataItem : DataItem, new();

        IList<TDataItem> LoadChildren<TDataItem>(string idParent, Guid id) where TDataItem : DataItem, new();
    }
}
