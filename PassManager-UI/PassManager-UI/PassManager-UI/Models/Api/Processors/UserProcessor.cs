using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace PassManager.Models.Api.Processors
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
                TaskStatus status = ApiHelper.ServerIsOpen(ex);
                return status.IsError ? status : new TaskStatus(true, status.Message);
            }
            if (responseMessage.IsSuccessStatusCode)
                return await LogIn(httpClient, username, password);
            else
            {
                string errorMsg = await responseMessage.Content.ReadAsStringAsync();
                if (errorMsg.Contains("is already taken"))
                    return new TaskStatus(true, $"Email \"{username}\" is already taken, try to log in!");

                return new TaskStatus(true, ErrorMsg.BasicError);
            }
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
                TaskStatus status = ApiHelper.ServerIsOpen(ex);
                return status.IsError ? status : new TaskStatus(true, status.Message);
            }
            ResponseToken token = await responseMessage.Content.ReadAsAsync<ResponseToken>();
            if (responseMessage.IsSuccessStatusCode)
            {
                try
                {
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
