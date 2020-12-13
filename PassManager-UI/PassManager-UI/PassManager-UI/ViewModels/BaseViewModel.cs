using PassManager.Models.Interfaces;
using PassManager_UI;
using System.ComponentModel;
using Xamarin.Essentials;

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
        protected private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected private bool CheckInternet()
        {
            bool internet = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if(!internet)
            {
                Models.PageService.PushPopupAsync(new Views.Popups.InternetErrorView());
            }
            return internet;
        }
    }
}
