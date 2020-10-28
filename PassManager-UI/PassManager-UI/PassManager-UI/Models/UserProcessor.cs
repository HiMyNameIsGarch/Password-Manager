using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Essentials;

namespace PassManager.Models
{
    public class UserProcessor
    {
        internal static async Task<TaskStatus> Register(HttpClient httpClient, string username, string password, string confirmPassword)
        {
            HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", username),
                    new KeyValuePair<string, string>("Password", password),
                    new KeyValuePair<string, string>("ConfirmPassword", confirmPassword),
                });// /api/Account/Register
            using (httpClient)
            {
                HttpResponseMessage responseMessage = await httpClient.PostAsync("/api/Account/Register", content);
                if (responseMessage.IsSuccessStatusCode)
                    return new TaskStatus(false);
                else 
                    return new TaskStatus(true,"The form input is invalid!");
            }
        }
        internal static async Task<TaskStatus> Login(HttpClient httpClient, string username, string password)
        {
            TaskStatus statusToken = await GetToken(httpClient, username,password);
            if (!statusToken.IsError)
            {
                try
                {
                    string authToken = await SecureStorage.GetAsync("auth_token");
                    return new TaskStatus(false);
                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                    return new TaskStatus(true, ex.Message);
                }
            }
            else return statusToken;
        }
        private static async Task<TaskStatus> GetToken(HttpClient httpClient, string username, string password)
        {
            HttpContent content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
            });
            using (httpClient)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "/token");
                requestMessage.Content = content;
                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
                ResponseToken token = await responseMessage.Content.ReadAsAsync<ResponseToken>();
                if (responseMessage.IsSuccessStatusCode)
                {
                    try
                    {
                        await SecureStorage.SetAsync("auth_token", token.token_type + token.access_token);
                    }
                    catch (Exception ex)
                    {
                        return new TaskStatus(true, ex.Message);
                    }
                    return new TaskStatus(false);
                }
                else return new TaskStatus(true, token.error_description);
            }
        }
    }
}
