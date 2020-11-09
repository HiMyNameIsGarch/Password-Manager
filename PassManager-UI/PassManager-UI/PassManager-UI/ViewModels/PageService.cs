using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassManager.ViewModels
{
    internal class PageService : IPageService
    {
        public static void SetMainPage()
        {
            if (Application.Current.MainPage is null) return;
            if (MainPage is null) MainPage = Application.Current.MainPage;
        }
        private protected static Page MainPage { get; set; }
        public async Task PushAsync(Page page)
        {
            await MainPage.Navigation.PushModalAsync(page);
        }
        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await MainPage.DisplayAlert(title, message, ok, cancel);
        }
        public void ChangeMainPage(Page page)
        {
            Application.Current.MainPage = page;
        }

    }
}
