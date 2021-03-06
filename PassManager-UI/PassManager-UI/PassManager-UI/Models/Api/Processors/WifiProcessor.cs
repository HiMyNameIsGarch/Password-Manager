﻿using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PassManager.Models.Api.Processors
{
    internal static class WifiProcessor
    {
        public static async Task<IEnumerable<Grouping<string, ItemPreview>>> GetPreviews(HttpClient httpClient)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync("api/Wifis");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                var itemList = await responseMessage.Content.ReadAsAsync<IEnumerable<Grouping<string, ItemPreview>>>();
                return Grouping<string, ItemPreview>.AddKeys(itemList);
            }
            return null;
        }
        public static async Task<Wifi> GetWifi(HttpClient httpClient, int id)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync($"api/Wifis/{id}");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                Wifi wifi = await responseMessage.Content.ReadAsAsync<Wifi>();
                return wifi;
            }
            return null;
        }
        public static async Task<bool> CreateWifi(HttpClient httpClient, Wifi wifi)
        {
            HttpContent content = ConvertToHttpContent(wifi);
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PostAsync("api/Wifis", content);
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
        public static async Task<bool> Modify(HttpClient httpClient, int id, Wifi changedWifi)
        {
            HttpContent httpContent = ConvertToHttpContent(changedWifi);
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PutAsync($"api/Wifis/{id}", httpContent);
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
                responseMessage = await httpClient.DeleteAsync($"api/Wifis/{id}");
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
        private static HttpContent ConvertToHttpContent(Wifi wifi)
        {
            return new FormUrlEncodedContent(new[]
            {
                 new KeyValuePair<string, string>("Id", wifi.Id.ToString()),
                 new KeyValuePair<string, string>("Name", wifi.Name),
                 new KeyValuePair<string, string>("PasswordEncrypted", wifi.PasswordEncrypted),
                 new KeyValuePair<string, string>("SSID", wifi.SSID),
                 new KeyValuePair<string, string>("SettingsPassword", wifi.SettingsPassword),
                 new KeyValuePair<string, string>("ConnectionType", wifi.ConnectionType),
                 new KeyValuePair<string, string>("Notes", wifi.Notes)
            });
        }
    }
}
