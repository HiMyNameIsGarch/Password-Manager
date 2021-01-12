using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Xamarin.Essentials;
using PassManager.Models;
using PassManager.Views.Popups;
using Rg.Plugins.Popup.Services;

namespace PassManager.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        //constructors
        public BaseViewModel(string pageTitle = "")
        {
            PageTitle = pageTitle;
        }
        //variables
        public event PropertyChangedEventHandler PropertyChanged;
        private string _pageTitle;
        //props
        public string PageTitle
        {
            get { return _pageTitle; }
            protected private set { _pageTitle = value; NotifyPropertyChanged(); }
        }
        //methods
        protected private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected private bool IsInternet()
        {
            bool internet = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if (!internet)
            {
                PageService.PushPopupAsync(new InternetErrorView());
            }
            return internet;
        }
        protected private async void HandleException(Exception ex)
        {
            Models.TaskStatus status = Models.Api.ApiHelper.ServerIsOpen(ex);
            await PopupNavigation.Instance.PopAllAsync(false);
            if (status.IsError)
            {
                await PageService.PushPopupAsync(new ErrorView("Oops... We couldn't connect to server, try again later!", true), true);
            }
            else
            {
                await PageService.PushPopupAsync(new ErrorView("Oops... Something went wrong, try again!", false), true);
            }
        }
    }
}
