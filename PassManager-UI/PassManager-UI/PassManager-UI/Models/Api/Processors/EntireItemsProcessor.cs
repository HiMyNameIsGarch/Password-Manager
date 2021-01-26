using PassManager.Enums;
using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PassManager.Models.Api.Processors
{
    internal class EntireItemsProcessor
    {
        public static async Task<IEnumerable<Grouping<string, ItemPreview>>> GetPreviews(HttpClient httpClient, string searchString = "")
        {
            bool isSearch = false;
            if (searchString != "") isSearch = true; 
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync($"api/EntireItems{(isSearch ? "?searchString=" + searchString : string.Empty)}");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                var itemList = await responseMessage.Content.ReadAsAsync<IEnumerable<ItemPreview>>();
                var groupedItems = itemList.GroupBy(item => item.ItemType)
                    .Select(item => new Grouping<string, ItemPreview>(item.Key.ToSampleString(), item));
                return groupedItems;
            }
            return null;
        }
        public static async Task<ItemPreview> GetLatestCreated(HttpClient httpClient, TypeOfItems typeOfItems)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync($"api/{typeOfItems}s?lastCreated=true");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                ItemPreview item = await responseMessage.Content.ReadAsAsync<ItemPreview>();
                return item;
            }
            return null;
        }
    }
}
