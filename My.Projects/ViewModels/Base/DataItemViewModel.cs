﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EPV.Data.DataItems;
using EPV.Database;

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
        protected Guid Id { get; private set; }

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

        protected DataItemViewModel() : base() { }

        public DataItemViewModel(IConnector connector, TDataItem dataItem = null)
            : base(connector)
        {
            if (dataItem == null)
                Id = Guid.Empty;
            else
                Id = dataItem.Id;
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
            TDataItem item = new TDataItem { Id = this.Id};
            if (Id != Guid.Empty)
            {
                item = await loaderArguments.Connector.Load<TDataItem>(DataItem);
            }

            return item;
        }

        #endregion

        #region SetData

        protected override void SetData(object data)
        {
            DataItem = (TDataItem)data;
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
            T currentValue = (T)propertyInfo.GetValue(this);
            if (propertyInfo != null && !T.Equals(currentValue, value))
            {
                propertyInfo.SetValue(DataItem, value);
                OnPropertyChanged(propertyName);
            }
        }


        #endregion

        #endregion
    }
}