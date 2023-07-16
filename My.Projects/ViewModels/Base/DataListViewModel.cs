using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml.Schema;
using EPV.Data.DataItems;
using EPV.Database;

namespace My.Projects.ViewModels.Base
{
    public abstract class DataListViewModel<TDataItem> : DataViewModel
        where TDataItem : DataItem, new()
    {
        public ObservableCollection<TDataItem> Items { get; }

        public DataListViewModel(IConnector connector) : base(connector)
        {
            Items = new ObservableCollection<TDataItem>();
        }

        protected override async Task GetData()
        {
            IList<TDataItem> items = await Connector.LoadList<TDataItem>();
            foreach (TDataItem dataItem in items)
                Items.Add(dataItem);
        }
    }
}
