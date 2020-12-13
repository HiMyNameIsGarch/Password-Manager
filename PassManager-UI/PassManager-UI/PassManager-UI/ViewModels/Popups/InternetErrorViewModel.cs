using Xamarin.Forms;
using System.Windows.Input;

namespace PassManager.ViewModels.Popups
{
    internal class InternetErrorViewModel
    {
        public InternetErrorViewModel()
        {
            _quit = new Command(Quit);
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
        private void Refresh()
        {
            bool internet = Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet;
            if (internet) Models.PageService.PopPopupAsync();
        }
        private void Quit()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
