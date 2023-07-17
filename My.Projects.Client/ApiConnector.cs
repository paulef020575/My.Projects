using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using EPV.Data.DataItems;
using EPV.Database;

namespace My.Projects.Client
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
        public virtual async Task<IList<TDataItem>> LoadList<TDataItem>()
            where TDataItem : DataItem, new()
        {
                TDataItem[] itemList = new TDataItem[0];

                HttpResponseMessage response = await HttpClient.GetAsync($"API/{typeof(TDataItem).Name}");

                if (response.IsSuccessStatusCode)
                {
                    itemList = await response.Content.ReadAsAsync<TDataItem[]>();
                }

                Thread.Sleep(5000);
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
        public async Task<TDataItem> Load<TDataItem>(Guid id)
            where TDataItem : DataItem, new()
        {
            TDataItem item = null;

            HttpResponseMessage response 
                = await HttpClient.GetAsync($"API/{typeof(TDataItem).Name}/{id}");

            if (response.IsSuccessStatusCode)
                item = await response.Content.ReadAsAsync<TDataItem>();

            return item;
        }

        /// <summary>
        ///     Возвращает копию объекта, загруженную с сайта
        /// </summary>
        /// <typeparam name="TDataItem">Тип объекта данных</typeparam>
        /// <param name="dataItem">объект данных</param>
        /// <returns>копия объекта</returns>
        public async Task<TDataItem> Load<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem, new()
        {
            return await Load<TDataItem>(dataItem.Id);
        }

        #endregion

        #region Save
        
        /// <summary>
        ///     Передает объект на сайт для сохранения
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта данных</typeparam>
        /// <param name="dataItem">объект для сохранения</param>
        /// <returns></returns>
        public async Task Save<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem, new()
        {
            JsonContent content = JsonContent.Create(dataItem);
            HttpResponseMessage response 
                = await HttpClient.PostAsJsonAsync($"API/{typeof(TDataItem).Name}", dataItem);
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Отправляет команду удаления объекта с заданным идентификатором
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="id">идентификатор объекта</param>
        /// <returns></returns>
        public async Task Delete<TDataItem>(Guid id)
            where TDataItem : DataItem, new()
        {
            HttpResponseMessage response
                = await HttpClient.DeleteAsync($"API/{typeof(TDataItem).Name}/{id}");
        }

        /// <summary>
        ///     Отправляет команду удаления объекта 
        /// </summary>
        /// <typeparam name="TDataItem">тип объекта</typeparam>
        /// <param name="dataItem">объект данных</param>
        /// <returns></returns>
        public async Task Delete<TDataItem>(TDataItem dataItem)
            where TDataItem : DataItem, new()
        {
            await Delete<TDataItem>(dataItem.Id);
        }

        #endregion

        #endregion
    }
}
