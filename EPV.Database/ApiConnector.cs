using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using EPV.Data.DataItems;

namespace EPV.Database
{
    /// <summary>
    ///     API-коннектор для работы с данными
    /// </summary>
    public class ApiConnector : IConnector
    {
        #region Properties

        #region HttpClient

        /// <summary>
        ///     Клиент для получения данных через протокол HTTP
        /// </summary>
        private HttpClient HttpClient { get; }

        #endregion

        #endregion

        #region Constructors

        private ApiConnector() { }

        public ApiConnector(string baseAddress)
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(baseAddress);

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        #region Methods

        #region LoadList

        /// <summary>
        ///     Возвращает список объектов из таблицы с сайта
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <returns>список объектов</returns>
        public virtual IList<TDataItem> LoadList<TDataItem>()
            where TDataItem : DataItem, new()
        {
            TDataItem[] itemList = new TDataItem[0];

            HttpResponseMessage response = HttpClient.GetAsync($"API/{typeof(TDataItem).Name}").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                itemList = response.Content.ReadAsAsync<TDataItem[]>().GetAwaiter().GetResult();
            }

            return itemList?.ToList();
        }


        #endregion

        #region Load

        /// <summary>
        ///     Возвращает объект, загруженный с сайта по идентификатору
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="id">Идентификатор записи</param>
        /// <returns>объект с сайта</returns>
        public TDataItem Load<TDataItem>(Guid id)
            where TDataItem : DataItem, new()
        {
            TDataItem item = null;

            HttpResponseMessage response
                = HttpClient.GetAsync($"API/{typeof(TDataItem).Name}/{id}").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
                item = response.Content.ReadAsAsync<TDataItem>().GetAwaiter().GetResult();

            return item;
        }

        /// <summary>
        ///     Возвращает копию объекта, загруженную с сайта
        /// </summary>
        /// <typeparam name="TDataItem">Тип объекта данных</typeparam>
        /// <param name="dataItem">объект данных</param>
        /// <returns>копия объекта</returns>
        public TDataItem Load<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem, new()
        {
            return Load<TDataItem>(dataItem.Id);
        }

        #endregion

        #region Save

        /// <summary>
        ///     Передает объект на сайт для сохранения
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта данных</typeparam>
        /// <param name="dataItem">объект для сохранения</param>
        /// <returns></returns>
        public void Save<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem, new()
        {
            HttpResponseMessage response
                = HttpClient.PostAsJsonAsync($"API/{typeof(TDataItem).Name}", dataItem).GetAwaiter().GetResult();
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Отправляет команду удаления объекта с заданным идентификатором
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="id">идентификатор объекта</param>
        /// <returns></returns>
        public void Delete<TDataItem>(Guid id)
            where TDataItem : DataItem, new()
        {
            HttpResponseMessage response
                = HttpClient.DeleteAsync($"API/{typeof(TDataItem).Name}/{id}").GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Отправляет команду удаления объекта 
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="dataItem">объект данных</param>
        /// <returns></returns>
        public void Delete<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem, new()
        {
            Delete<TDataItem>(dataItem.Id);
        }

        #endregion

        #region LoadReferences

        public IList<Reference<TDataItem>> LoadReferences<TDataItem>()
            where TDataItem : DataItem, new()
        {
            Reference<TDataItem>[] itemList = new Reference<TDataItem>[0];

            HttpResponseMessage response = HttpClient.GetAsync($"API/{typeof(TDataItem).Name}/LoadReferences").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                itemList = response.Content.ReadAsAsync<Reference<TDataItem>[]>().GetAwaiter().GetResult();
            }

            return itemList?.ToList();
        }

        #endregion

        #region LoadReferences

        public IList<TDataItem> LoadChildren<TDataItem>(string idParent, Guid id)
            where TDataItem : DataItem, new()
        {
            TDataItem[] itemList = new TDataItem[0];

            HttpResponseMessage response 
                = HttpClient.GetAsync($"API/{typeof(TDataItem).Name}/LoadChildren?idParent={idParent}&id={id}").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                itemList = response.Content.ReadAsAsync<TDataItem[]>().GetAwaiter().GetResult();
            }

            return itemList?.ToList();
        }

        #endregion

        #endregion
    }
}
