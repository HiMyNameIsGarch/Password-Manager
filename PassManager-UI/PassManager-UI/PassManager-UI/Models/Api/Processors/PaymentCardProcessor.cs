using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PassManager.Models.Api.Processors
{
    internal class PaymentCardProcessor
    {
        public static async Task<IEnumerable<Grouping<string, ItemPreview>>> GetPreviews(HttpClient httpClient)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync("api/PaymentCards");
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
        public static async Task<bool> CreateCard(HttpClient httpClient, PaymentCard card)
        {
            HttpContent content = ConvertToHttpContent(card);
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PostAsync("api/PaymentCards", content);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public static async Task<PaymentCard> GetCard(HttpClient httpClient, int id)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync($"api/PaymentCards/{id}");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                PaymentCard card = await responseMessage.Content.ReadAsAsync<PaymentCard>();
                return card;
            }
            return null;
        }
        public static async Task<bool> Modify(HttpClient httpClient, int id, PaymentCard changedCard)
        {
            HttpContent httpContent = ConvertToHttpContent(changedCard);
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PutAsync($"api/PaymentCards/{id}", httpContent);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public static async Task<bool> Delete(HttpClient httpClient, int id)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.DeleteAsync($"api/PaymentCards/{id}");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        private static HttpContent ConvertToHttpContent(PaymentCard paymentCard)
        {
            return new FormUrlEncodedContent(new[]
            {
                 new KeyValuePair<string, string>("Id", paymentCard.Id.ToString()),
                 new KeyValuePair<string, string>("StartDate", paymentCard.StartDate.ToString()),
                 new KeyValuePair<string, string>("ExpirationDate", paymentCard.ExpirationDate.ToString()),
                 new KeyValuePair<string, string>("NameOnCard", paymentCard.NameOnCard),
                 new KeyValuePair<string, string>("CardType", paymentCard.CardType),
                 new KeyValuePair<string, string>("CardNumber", paymentCard.CardNumber),
                 new KeyValuePair<string, string>("SecurityCode", paymentCard.SecurityCode),
                 new KeyValuePair<string, string>("Notes", paymentCard.Notes)
            });
        }
    }
}
