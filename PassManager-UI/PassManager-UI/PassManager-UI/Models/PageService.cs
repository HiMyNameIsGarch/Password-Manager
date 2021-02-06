using PassManager.ViewModels.Popups;
using PassManager.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
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
            return PopupNavigation.Instance.PushAsync(page,animate);
        }
        public static Task PopPopupAsync(bool animate = true)
        {
            if(PopupNavigation.Instance.PopupStack.Count > 0)
            {
                return PopupNavigation.Instance.PopAsync(animate);
            }
            return Task.CompletedTask;
        }
        public static Task PopAllAsync(bool animate = true)
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                return PopupNavigation.Instance.PopAllAsync(animate);
            }
            return Task.CompletedTask;
        }
        public async static Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            var popup = new DisplayActionSheetView(title, message, accept, cancel);
            await PopupNavigation.Instance.PushAsync(popup);
            var response = await popup.PopupClosedTask;
            return response;
        }
        public async static Task<string> DisplayActionSheet(string title, string message, string cancel, IEnumerable<string> buttons)
        {
            var popup = new DisplayActionSheetPopup(title, message, cancel, buttons);
            await PopupNavigation.Instance.PushAsync(popup);
            var response = await popup.PopupClosedTask;
            return response;
        }
    }
}
