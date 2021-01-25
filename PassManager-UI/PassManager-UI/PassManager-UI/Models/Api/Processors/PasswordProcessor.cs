using PassManager.Enums;
using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PassManager.Models.Api.Processors
{
    internal class PasswordProcessor
    {
        public static async Task<IEnumerable<Grouping<string, ItemPreview>>> GetPreviews(HttpClient httpClient)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync("api/Passwords");
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
        public static async Task<bool> CreatePassword(HttpClient httpClient, Password password)
        {
            HttpContent content = ConvertToHttpContent(password);
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PostAsync("api/Passwords", content);
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
        public static async Task<Password> GetPassword(HttpClient httpClient, int id)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync($"api/Passwords/{id}");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                Password password = await responseMessage.Content.ReadAsAsync<Password>();
                return password;
            }
            return null;
        }
        public static async Task<bool> Modify(HttpClient httpClient, int id, Password changedPassword)
        {
            HttpContent httpContent = ConvertToHttpContent(changedPassword);
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PutAsync($"api/Passwords/{id}", httpContent);
            }
            catch(Exception ex)
            {
                throw(ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            throw new Exception("Error from modifying a password");
        }
        public static async Task<bool> Delete(HttpClient httpClient, int id)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.DeleteAsync($"api/Passwords/{id}");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            throw new Exception("Error from deleting a password");
        }
        private static HttpContent ConvertToHttpContent(Password password)
        {
            return new FormUrlEncodedContent(new[]
            {
                 new KeyValuePair<string, string>("Id", password.Id.ToString()),
                 new KeyValuePair<string, string>("Name", password.Name),
                 new KeyValuePair<string, string>("Username", password.Username),
                 new KeyValuePair<string, string>("PasswordEncrypted", password.PasswordEncrypted),
                 new KeyValuePair<string, string>("Url", password.Url),
                 new KeyValuePair<string, string>("Notes", password.Notes)
            });
        }
    }
}
