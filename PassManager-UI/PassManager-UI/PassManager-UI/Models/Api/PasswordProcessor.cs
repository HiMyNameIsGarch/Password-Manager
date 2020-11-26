using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Models.Api
{
    public class PasswordProcessor
    {
        internal static async Task<bool> GetPreviews(HttpClient httpClient)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync("api/Passwords");
            }
            catch
            {
            }
            if (responseMessage.IsSuccessStatusCode)
                return true;
            else return false;
        }
        //internal static async Task GetPassword(HttpClient httpClient)
        //{
        //}
        //internal static async Task CreatePassword(HttpClient httpClient)
        //{
        //}
        //internal static async Task Delete(HttpClient httpClient)
        //{
        //}
        //internal static async Task Modify(HttpClient httpClient)
        //{
        //}
    }
}
