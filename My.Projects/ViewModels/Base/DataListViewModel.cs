using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using EPV.Data.DataItems;
using EPV.Database;

namespace My.Projects.ViewModels.Base
{
    /// <summary>
    ///     Модель представления для списка объектов
    /// </summary>
    /// <typeparam name="TDataItem"></typeparam>
    public abstract class DataListViewModel<TDataItem> : DataViewModel
        where TDataItem : DataItem, new()
    {
        #region Properties

        #region IsLoading

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Items

        /// <summary>
        ///     Список объектов
        /// </summary>
        public ObservableCollection<TDataItem> Items { get; }

        #endregion

        #endregion

        #region Constructors

        protected DataListViewModel() : base() { }

        public DataListViewModel(IConnector connector) : base(connector)
        {
            Items = new ObservableCollection<TDataItem>();
        }

        #endregion

        #region Methods

        #region GetData

        /// <summary>
        ///     Загружает данные для работы
        /// </summary>
        /// <returns></returns>
        protected override async Task<object> GetData(LoaderArguments loaderArguments)
        {
            return await loaderArguments.Connector.LoadList<TDataItem>();
        }

        #endregion

        #region SetData

        protected override void SetData(object data)
        {
            IList<TDataItem> dataItems = (IList<TDataItem>)data;

            Items.Clear();

            if (dataItems != null)
                foreach (TDataItem item in dataItems)
                    Items.Add(item);
        }

        #endregion

        #endregion

        #region Commands

        #region RefreshCommand

        private RelayCommand _refreshCommand;

        public RelayCommand RefreshCommand
        { 
            get
            {
                if (_refreshCommand == null)
                    _refreshCommand = new RelayCommand(x => LoadData(), x => !IsLoading);

                return _refreshCommand;
            }
        }

        #endregion

        #endregion
    }
}
