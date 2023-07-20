using System;
using System.Collections.Generic;
using EPV.Data.Conditions;
using EPV.Data.DataItems;

namespace EPV.Data.DataGetters
{
    public interface IDataLink
    {
        TDataItem Load<TDataItem>(Guid id) where TDataItem : DataItem, new();

        IList<TDataItem> LoadList<TDataItem>() where TDataItem : DataItem, new();

        void Save<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new();

        void Delete<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new();

        IList<Reference<TDataItem>> LoadReferences<TDataItem>() where TDataItem : DataItem, new();

        IList<TDataItem> LoadChildren<TDataItem>(string parentColumn, Guid id) where TDataItem : DataItem, new();
    }
}