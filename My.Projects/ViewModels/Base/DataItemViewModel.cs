using System;
using System.Threading.Tasks;
using EPV.Data.DataItems;
using EPV.Database;

namespace My.Projects.ViewModels.Base
{
    public abstract class DataItemViewModel<TDataItem> : DataViewModel
        where TDataItem : DataItem, new()
    {
        protected Guid Id { get; private set; }

        public TDataItem DataItem { get; private set; }

        public DataItemViewModel(IConnector connector, TDataItem dataItem = null)
            : base(connector)
        {
            if (dataItem == null)
                Id = Guid.Empty;
            else
                Id = dataItem.Id;
        }

        protected override async Task GetData()
        {
            DataItem = new TDataItem { Id = this.Id};
            if (Id != Guid.Empty)
            {
                DataItem = await Connector.Load<TDataItem>(DataItem);
            }
        }
    }
}
