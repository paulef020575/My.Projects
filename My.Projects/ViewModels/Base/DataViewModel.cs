using System;
using System.Threading.Tasks;
using EPV.Database;

namespace My.Projects.ViewModels.Base
{
    /// <summary>
    ///     Модель данных для работы с данными
    /// </summary>
    public abstract class DataViewModel : ViewModel
    {
        #region MyRegion

        #region Connector

        /// <summary>
        ///     Коннектор для получения данных
        /// </summary>
        public IConnector Connector { get; private set; }

        #endregion

        #endregion


        #region Constructors

        public DataViewModel(IConnector connector)
        {
            Connector = connector;
        }

        #endregion

        #region Methods

        #region GetData

        /// <summary>
        ///     загружает данные для обработки
        /// </summary>
        /// <returns></returns>
        protected abstract Task GetData();

        #endregion

        #endregion
    }
}
