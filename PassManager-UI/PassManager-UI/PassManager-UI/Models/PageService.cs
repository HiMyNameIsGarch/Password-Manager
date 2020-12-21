using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassManager.Models
{
    internal static class PageService 
    {
        private static Page MainPage
        {
            get { return Application.Current.MainPage; }
        }
        public static void ChangeMainPage(Page page)
        {
            Application.Current.MainPage = page;
        }
        public static Task PushPopupAsync(PopupPage page, bool animate = true)
        {
            return MainPage.Navigation.PushPopupAsync(page,animate);
        }
        public static Task PopPopupAsync(bool animate = true)
        {
           return MainPage.Navigation.PopPopupAsync(animate);
        }
        //test only
        public async static Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await MainPage.DisplayAlert(title, message,accept,cancel);
        }
    }
}
