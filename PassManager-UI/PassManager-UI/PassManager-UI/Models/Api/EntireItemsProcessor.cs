using PassManager.Enums;
using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PassManager.Models.Api
{
    public class EntireItemsProcessor
    {
        internal static async Task<IEnumerable<Grouping<TypeOfItems, ItemPreview>>> GetPreviews(HttpClient httpClient, string searchString = "")
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
                IEnumerable<ItemPreview> itemList = await responseMessage.Content.ReadAsAsync<IEnumerable<ItemPreview>>();
                return itemList.GroupBy(item => item.ItemType)
                                   .Select(item => new Grouping<TypeOfItems, ItemPreview>(item.Key, item));
            }
            return null;
        }
    }
}
