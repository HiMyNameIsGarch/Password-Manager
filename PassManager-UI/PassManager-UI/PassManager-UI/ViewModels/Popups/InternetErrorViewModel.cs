using Xamarin.Forms;
using System.Windows.Input;
using PassManager.Models;
using Xamarin.Essentials;

namespace PassManager.ViewModels.Popups
{
    internal class InternetErrorViewModel
    {
        public InternetErrorViewModel()
        {
            _quit = new Command(async () =>
            {
                await PageService.PopPopupAsync();
            });
            _refresh = new Command(Refresh);
        }
        private ICommand _quit;
        private ICommand _refresh;
        public ICommand QuitApp
        {
            get { return _quit; }
        }
        public ICommand RefreshPage
        {
            get { return _refresh; }
        }
        private async void Refresh()
        {
            bool internet = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if (internet) await PageService.PopPopupAsync();
        }
    }
}
