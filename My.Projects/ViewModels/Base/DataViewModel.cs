using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        protected DataViewModel() { }

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
        protected abstract object GetData(LoaderArguments loaderArguments);

        #endregion

        #region SetData

        /// <summary>
        ///     устанавливает данные
        /// </summary>
        /// <param name="data">полученные данные</param>
        protected abstract void SetData(object data);

        #endregion

        #region LoadData

        public override void LoadData()
        {
            startProgress(this, "Загрузка данных");
            BackgroundWorker loader = new BackgroundWorker();
            loader.DoWork += Loader_DoWork;
            loader.RunWorkerCompleted += Loader_RunWorkerCompleted;
            loader.RunWorkerAsync(new LoaderArguments(Connector));
        }


        private void Loader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error is HttpRequestException)
                {
                    _onError?.Invoke(this, (string)Application.Current.Resources["ApiError"]);
                    SetData(null);
                }
            }
            else
            {
                SetData(e.Result);
            }

            finishProgress(this, "");
        }

        private void Loader_DoWork(object sender, DoWorkEventArgs e)
        {
            LoaderArguments loaderArguments = (LoaderArguments)e.Argument;

            e.Result = GetData(loaderArguments);
        }

        #endregion

        #endregion
    }
}
