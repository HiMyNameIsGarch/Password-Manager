using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace PassManager.Models.Api
{
    public class ApiHelper
    {
        internal static string SERVER
        {
            get
            {
                return Device.RuntimePlatform == Device.Android ? "https://192.168.0.143:45455" : "https://localhost:44364";
            }
        }
        private static HttpClient _httpClient;
        internal static HttpClient ApiClient
        {
            get
            {
                _httpClient = _httpClient ?? new HttpClient
                (
                    new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                        {
                            //bypass
                            return true;
                        },
                    }, false
                );
                return _httpClient;
            }
        }
        internal static void InitializeClient()
        {
            if (_httpClient is null)
            {
                ApiClient.BaseAddress = new Uri(SERVER);
                ApiClient.DefaultRequestHeaders.Accept.Clear();
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        internal static TaskStatus ServerIsOpen(Exception ex)
        {
            string msg = ex.InnerException?.Message;
            if (msg is null) msg = ex.Message;
            return (msg.Contains("connection") && msg.Contains("server") && msg.Contains("not") && msg.Contains("established"))
                ? new TaskStatus(true, ErrorMsg.ServerError)
                : new TaskStatus(false, ErrorMsg.BasicError);
        }
        internal static void AddAuthorization(string tokenType, string token)
        {
            if (_httpClient.DefaultRequestHeaders.Authorization is null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
            }
        }
        internal static void DeleteAuthorization()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}