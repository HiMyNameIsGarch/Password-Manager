using System.Runtime.CompilerServices;
using System.ComponentModel;
using Xamarin.Essentials;
using PassManager.Models;
using PassManager.Views.Popups;

namespace PassManager.ViewModels.Bases
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
    }
}
