using PassManager.Enums;
using PassManager.Models.Items;
using System;
using System.Collections.Generic;
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
                var itemList = await responseMessage.Content.ReadAsAsync<IEnumerable<Grouping<string, ItemPreview>>>();
                return Grouping<string,ItemPreview>.AddKeys(itemList);
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
