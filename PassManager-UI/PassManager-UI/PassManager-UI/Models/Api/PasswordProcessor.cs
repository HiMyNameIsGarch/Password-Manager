using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PassManager.Models.Api
{
    public class PasswordProcessor
    {
        internal static async Task<IEnumerable<ItemPreview>> GetPreviews(HttpClient httpClient)
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
                IEnumerable<ItemPreview> itemList = await responseMessage.Content.ReadAsAsync<IEnumerable<ItemPreview>>();
                return itemList;
            }
            return null;
        }
        internal static async Task<bool> CreatePassword(HttpClient httpClient, Password password)
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
        internal static async Task<Password> GetPassword(HttpClient httpClient, int id)
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
        internal static async Task<bool> Modify(HttpClient httpClient, int id, Password changedPassword)
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
            return false;
        }
        internal static async Task<bool> Delete(HttpClient httpClient, int id)
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
            return false;
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
