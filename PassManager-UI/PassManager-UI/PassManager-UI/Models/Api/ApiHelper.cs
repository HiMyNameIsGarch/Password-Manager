using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PassManager.Models.Api
{
    public class ApiHelper
    {
        internal const string SERVER = "https://localhost:44364";
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
                    } , false
                );
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "TOKEN");
                return _httpClient;
            }
        }
        internal static void InitializeClient()
        {
            ApiClient.BaseAddress = new Uri(SERVER);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
