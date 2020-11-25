using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models.Interfaces;

namespace PassManager.ViewModels.CreateItems
{
    public class CreateWifiVM :  BaseViewModel, IBackButtonBehavior
    {
        public CreateWifiVM()
        {
            _goBack = new Command(GoBackButton);
            PageTitle = "Create Wifi!";
        }
        private ICommand _goBack;
        public ICommand GoBack {
            get { return _goBack; }
        }
        public async void GoBackButton()
        {
            await Shell.Current.GoToAsync("../..", true);
        }
    }
}
