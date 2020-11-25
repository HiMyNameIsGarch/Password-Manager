using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PassManager.Models.Interfaces;
using Xamarin.Forms;

namespace PassManager.ViewModels.CreateItems
{
    public class CreatePasswordVM : BaseViewModel, IBackButtonBehavior
    {
        public CreatePasswordVM()
        {
            _goBack = new Command(GoBackButton);
            PageTitle = "Create password";
        }
        private ICommand _goBack;
        public ICommand GoBack
        {
            get { return _goBack; }
        }
        public async void GoBackButton()
        {
            await Shell.Current.GoToAsync("../..", true);
        }
    }
}
