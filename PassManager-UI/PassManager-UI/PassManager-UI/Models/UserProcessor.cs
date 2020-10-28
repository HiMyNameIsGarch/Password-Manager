using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

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
    }
}
