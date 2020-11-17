using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PassManager.Models.Api
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
            });
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PostAsync("/api/Account/Register", content);
            }
            catch (Exception ex)
            {
                return ApiHelper.ServerIsOpen(ex);
            }
            if (responseMessage.IsSuccessStatusCode)
                return await LogIn(httpClient, username, password);
            else
                return new TaskStatus(true, "The form input is invalid!");
        }
        internal static async Task<TaskStatus> LogIn(HttpClient httpClient, string username, string password)
        {
            HttpContent content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
            });
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "/token");
            requestMessage.Content = content;
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.SendAsync(requestMessage);
            }
            catch(Exception ex)
            {
                return ApiHelper.ServerIsOpen(ex);
            }
            ResponseToken token = await responseMessage.Content.ReadAsAsync<ResponseToken>();
            if (responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    await SecureStorage.SetAsync("token_auth", token.access_token);
                    await SecureStorage.SetAsync("token_type", token.token_type);
                    ApiHelper.AddAuthorization(token.token_type, token.access_token);
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
