using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models.Interfaces;

namespace PassManager.ViewModels.CreateItems
{
    public class CreateWifiVM :  BaseViewModel, IBackButtonBehavior
    {
        public CreateWifiVM() : base("Create Wifi")
        {
            _goBack = new Command(GoBackButton);
        }
        private ICommand _goBack;
        public ICommand GoBack {
            get { return _goBack; }
        }
        public async void GoBackButton()
        {
            await Shell.Current.Navigation.PopToRootAsync(true);
        }
    }
}
