﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using EPV.Data.DataItems;
using My.Projects.Data;

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

        public DataListViewModel() : base()
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
        protected override object GetData(LoaderArguments loaderArguments)
        {
            return loaderArguments.Connector.LoadList<TDataItem>();
        }

        #endregion

        #region SetData

        protected override void SetData(object data)
        {
            IList<TDataItem> dataItems = data as IList<TDataItem>;

            Items.Clear();

            if (dataItems != null)
                foreach (TDataItem item in dataItems)
                    Items.Add(item);
        }

        #endregion

        #region EditItem

        private void EditItem(TDataItem item)
        {
            _onSwitchToViewModel?.Invoke(this, GetDataItemViewModel(item));
        }

        #endregion

        #region GetDataItemViewModel

        protected abstract DataItemViewModel<TDataItem> GetDataItemViewModel(TDataItem dataItem = null);

        #endregion

        #region AddItem

        private void AddItem()
        {
            _onSwitchToViewModel(this, GetDataItemViewModel());
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

        #region EditItemCommand

        private RelayCommand _editItemCommand;

        public RelayCommand EditItemCommand
        {
            get
            {
                if (_editItemCommand == null)
                    _editItemCommand = new RelayCommand(x => EditItem((TDataItem)x), x => x is TDataItem);

                return _editItemCommand;
            }
        }

        #endregion

        #region AddItemCommand

        private RelayCommand _addItemCommand;

        public RelayCommand AddItemCommand
        {
            get
            {
                if (_addItemCommand == null)
                    _addItemCommand = new RelayCommand(x => AddItem());
                return _addItemCommand;
            }
        }

        #endregion

        #endregion
    }
}
