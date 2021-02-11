using PassManager.Models.Interfaces;
using PassManager.Views.Popups;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassManager.Models
{
    internal static class PageService 
    {
        public static readonly Android.Graphics.Color CeladonBlueColor = Android.Graphics.Color.Rgb(69, 123, 157);
        public static readonly Android.Graphics.Color PrussianBlueColor = Android.Graphics.Color.Rgb(29, 53, 87);
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
        public async static Task<string> DisplayActionSheet(string title, string cancel, IEnumerable<string> buttons)
        {
            var popup = new DisplayActionSheetPopup(title, cancel, buttons);
            await PopupNavigation.Instance.PushAsync(popup);
            var response = await popup.PopupClosedTask;
            return response;
        }
        public async static Task<DateTime> GetNewDateTime(DateTime datetime)
        {
            var popup = new DatePickerView(datetime);
            await PopupNavigation.Instance.PushAsync(popup);
            var response = await popup.PopupClosedTask;
            return response;
        }
        public static void ChangeNavBarColor(Android.Graphics.Color color)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var statusbar = DependencyService.Get<IStatusBarPlatformSpecific>();
                statusbar.ChangeNavigationBarColor(color);
            }
        }
        public static async void HandleException(Exception ex)
        {
            Models.TaskStatus status = Models.Api.ApiHelper.ServerIsOpen(ex);
            await PopupNavigation.Instance.PopAllAsync(false);
            if (status.IsError)
            {
                await PageService.PushPopupAsync(new ErrorView(ErrorMsg.ServerError, true), true);
            }
            else
            {
                await PageService.PushPopupAsync(new ErrorView(ErrorMsg.BasicError, false), true);
            }
        }
    }
}
