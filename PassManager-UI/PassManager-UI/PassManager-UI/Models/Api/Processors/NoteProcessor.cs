using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PassManager.Models.Api.Processors
{
    internal class NoteProcessor
    {
        public static async Task<IEnumerable<Grouping<string, ItemPreview>>> GetPreviews(HttpClient httpClient)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync("api/Notes");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                var itemList = await responseMessage.Content.ReadAsAsync<IEnumerable<ItemPreview>>();
                var groupedItems = Grouping<string, ItemPreview>.GroupList(itemList);
                return groupedItems;
            }
            return null;
        }
        public static async Task<bool> CreateNote(HttpClient httpClient, Note note)
        {
            HttpContent content = ConvertToHttpContent(note);
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PostAsync("api/Notes", content);
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
        public static async Task<Note> GetNote(HttpClient httpClient, int id)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.GetAsync($"api/Notes/{id}");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                Note note = await responseMessage.Content.ReadAsAsync<Note>();
                return note;
            }
            return null;
        }
        public static async Task<bool> Modify(HttpClient httpClient, int id, Note changedNote)
        {
            HttpContent httpContent = ConvertToHttpContent(changedNote);
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await httpClient.PutAsync($"api/Notes/{id}", httpContent);
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
                responseMessage = await httpClient.DeleteAsync($"api/Notes/{id}");
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
        private static HttpContent ConvertToHttpContent(Note note)
        {
            return new FormUrlEncodedContent(new[]
            {
                 new KeyValuePair<string, string>("Id", note.Id.ToString()),
                 new KeyValuePair<string, string>("Name", note.Name),
                 new KeyValuePair<string, string>("Notes", note.Notes)
            });
        }
    }
}
