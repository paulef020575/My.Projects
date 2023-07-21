using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using EPV.Data.DataGetters;
using EPV.Database;
using My.Projects.Data;

namespace My.Projects.ViewModels.Base
{
    /// <summary>
    ///     Модель данных для работы с данными
    /// </summary>
    public abstract class DataViewModel : ViewModel
    {
        #region Properties

        #region Connector

        /// <summary>
        ///     Коннектор для получения данных
        /// </summary>
        public IMyDataLink Connector => DataChannels.DataLink as IMyDataLink;

        #endregion

        #endregion


        #region Constructors

        public DataViewModel() { }

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
