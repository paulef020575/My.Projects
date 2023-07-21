using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using EPV.Data.DataItems;

namespace EPV.Data.DataGetters
{
    public class ApiLink : IDataLink
    {
        private HttpClient Client { get; }

        public ApiLink(string baseAddress)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(baseAddress);

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public virtual TDataItem Load<TDataItem>(Guid id) where TDataItem : DataItem, new()
        {
            TDataItem item = null;

            HttpResponseMessage response 
                = Client.GetAsync($"API/{typeof(TDataItem).Name}/{id}").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                item = response.Content.ReadAsAsync<TDataItem>().GetAwaiter().GetResult();
            }

            return item;
        }
        public virtual void Load<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new()
        {
            TDataItem source = Load<TDataItem>(dataItem.Id);
            dataItem.CopyFrom(source);
        }

        public virtual IList<TDataItem> LoadList<TDataItem>() where TDataItem : DataItem, new()
        {
            TDataItem[] items = new TDataItem[0];

            HttpResponseMessage response
                = Client.GetAsync($"API/{typeof(TDataItem).Name}").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                items = response.Content.ReadAsAsync<TDataItem[]>().GetAwaiter().GetResult();
            }

            return items?.ToList();
        }

        public virtual void Save<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new()
        {
            HttpResponseMessage response = Client.PostAsJsonAsync($"API/{typeof(TDataItem).Name}", dataItem)
                .GetAwaiter().GetResult();
        }

        public virtual void Delete<TDataItem>(TDataItem dataItem) where TDataItem : DataItem, new()
        {
            HttpResponseMessage response = Client.DeleteAsync($"API/{typeof(TDataItem).Name}/{dataItem.Id}")
                .GetAwaiter().GetResult();
        }

        public virtual IList<Reference<TDataItem>> LoadReferences<TDataItem>() where TDataItem : DataItem, new()
        {
            Reference<TDataItem>[] itemList = new Reference<TDataItem>[0];

            HttpResponseMessage response = Client.GetAsync($"API/{typeof(TDataItem).Name}/LoadReferences").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                itemList = response.Content.ReadAsAsync<Reference<TDataItem>[]>().GetAwaiter().GetResult();
            }

            return itemList?.ToList();
        }

        public virtual IList<TDataItem> LoadChildren<TDataItem>(string parentColumn, Guid id) where TDataItem : DataItem, new()
        {
            TDataItem[] itemList = new TDataItem[0];
            string uri = $"API/{typeof(TDataItem)}/LoadChildren?idParent={parentColumn}&id={id}";

            HttpResponseMessage response = Client.GetAsync(uri).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                itemList = response.Content.ReadAsAsync<TDataItem[]>().GetAwaiter().GetResult();
            }

            return itemList?.ToList();
        }
    }
}
