using System;
using System.ComponentModel;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using EPV.Data.DataItems;
using My.Projects.Data;

namespace My.Projects.ViewModels.Base
{
    /// <summary>
    ///     Модель представления для работы с объектом
    /// </summary>
    /// <typeparam name="TDataItem">тип редактируемого объекта</typeparam>
    public abstract class DataItemViewModel<TDataItem> : DataViewModel
        where TDataItem : DataItem, new()
    {
        #region Properties

        #region Id

        /// <summary>
        ///     Идентификатор объекта
        /// </summary>
        /// <remarks>
        ///     Используется для загрузки данных
        /// </remarks>
        protected Guid Id => DataItem.Id;

        #endregion

        #region DataItem

        /// <summary>
        ///     Редактируемый объект
        /// </summary>
        public TDataItem DataItem { get; private set; }

        #endregion

        #region IsModified

        private bool _isModified = false;

        public bool IsModified
        {
            get => _isModified;
            set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        protected DataItemViewModel() : base()
        {
        }

        public DataItemViewModel(IMyConnector connector, TDataItem dataItem = null)
            : base(connector)
        {
            DataItem = dataItem ?? new TDataItem();
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
            if (Id != Guid.Empty)
            { 
                return loaderArguments.Connector.Load<TDataItem>(DataItem);
            }

            return DataItem;
        }

        #endregion

        #region SetData

        protected override void SetData(object data)
        {
            DataItem = (TDataItem) data;
        }

        #endregion

        #region OnPropertyChanged

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName != nameof(IsModified))
                IsModified = true;
        }

        #endregion

        #region SetProperty

        protected virtual void SetProperty<T>(T value, [CallerMemberName] string propertyName = "")
        {
            PropertyInfo propertyInfo = typeof(TDataItem).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                try
                {
                    T currentValue = (T)propertyInfo.GetValue(DataItem);

                    if (value == null || !value.Equals(currentValue))
                    {
                        propertyInfo.SetValue(DataItem, value);
                        OnPropertyChanged(propertyName);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }


        #endregion

        #region SaveData

        protected virtual void SaveData()
        {
            startProgress(this, (string) Application.Current.Resources["DataSaving"]);
            BackgroundWorker saverWorker = new BackgroundWorker();
            saverWorker.DoWork += SaverWorker_DoWork;
            saverWorker.RunWorkerCompleted += SaverWorker_RunWorkerCompleted;
            saverWorker.RunWorkerAsync(new LoaderArguments(Connector));
        }

        private void SaverWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error is HttpRequestException)
                {
                    _onError?.Invoke(this, (string)Application.Current.Resources["ApiError"]);
                }
            }
            else
            {
                finishProgress(this, (string) Application.Current.Resources["DataSaved"]);
                _onSwitchToViewModel(this, PreviousViewModel);
            }
        }

        private void SaverWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoaderArguments loaderArguments = (LoaderArguments)e.Argument;
            loaderArguments.Connector.Save(DataItem);
        }


        #endregion

        #region CancelEdit

        private void CancelEdit()
        {
            _onSwitchToViewModel(this, PreviousViewModel);
        }

        #endregion

        #endregion

        #region Commands

        #region SaveCommand

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(x => SaveData(), x => IsModified);
                return _saveCommand;
            }
        }

        #endregion

        #region CancelCommand

        private RelayCommand _cancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(x=> CancelEdit());
                return _cancelCommand;
            }
        }

        #endregion


        #endregion


    }
}
