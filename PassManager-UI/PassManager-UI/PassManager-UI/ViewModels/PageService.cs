using PassManager.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassManager.ViewModels
{
    internal class PageService : IPageService
    {
        public static void SetMainPage(Page page)
        {
            if (page is null) return;
            if (MainPage is null) MainPage = page;
        }
        private protected static Page MainPage { get; set; }
        public async Task PushAsync(Page page)
        {
            await MainPage.Navigation.PushAsync(page);
        }
        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await MainPage.DisplayAlert(title, message, ok, cancel);
        }

    }
}
