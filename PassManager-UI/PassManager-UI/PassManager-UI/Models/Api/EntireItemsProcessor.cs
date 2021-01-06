using Newtonsoft.Json;
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
        internal static async Task<IEnumerable<Grouping<TypeOfItems, ItemPreview>>> GetPreviews(HttpClient httpClient)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync("api/EntireItems");
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
