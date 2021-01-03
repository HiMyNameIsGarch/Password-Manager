using PassManager.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassManager.ViewModels.Popups
{
    public class ErrorViewModel : BaseViewModel
    {
        public ErrorViewModel(string errorMsg, bool canQuit = false) : base(errorMsg)
        {
            _closePopup = new Command(Close);
            _quit = new Command(QuitApp);
            CanQuit = canQuit;
        }
        //private props
        private ICommand _quit;
        private ICommand _closePopup;
        private bool _canQuit;
        //command
        public ICommand ClosePopup
        {
            get { return _closePopup; }
        }
        public ICommand Quit
        {
            get { return _quit; }
        }
        public bool CanQuit
        {
            get { return _canQuit; }
            private set { _canQuit = value; NotifyPropertyChanged(); }
        }
        //functions for commands
        private async void Close()
        {
            await PageService.PopPopupAsync();
        }
        private void QuitApp()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
