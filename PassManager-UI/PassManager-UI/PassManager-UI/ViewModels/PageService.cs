using PassManager.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassManager.ViewModels
{
    public class PageService : IPageService
    {
        private readonly Page MainPage = Application.Current.MainPage;
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
