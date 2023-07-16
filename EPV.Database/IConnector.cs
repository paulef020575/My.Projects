using System.Collections.Generic;
using System.Threading.Tasks;
using EPV.Data.DataItems;

namespace EPV.Database
{
    public interface IConnector
    {
        Task<IList<TDataItem>> LoadList<TDataItem>() where TDataItem : DataItem, new();

        Task<TDataItem> Load<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new();

        Task Save<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new();

        Task Delete<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new();
    }
}
