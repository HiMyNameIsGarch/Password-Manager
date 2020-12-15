﻿using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models.Interfaces;
using System.Threading.Tasks;

namespace PassManager.ViewModels.CreateItems
{
    public class CreateWifiVM : BaseItemVM, IBackButtonBehavior
    {
        public CreateWifiVM()
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

        private protected override Task Create()
        {
            throw new System.NotImplementedException();
        }

        private protected override Task Delete()
        {
            throw new System.NotImplementedException();
        }

        private protected override Task Modify()
        {
            throw new System.NotImplementedException();
        }

        private protected override void AfterSettingId()
        {
            throw new System.NotImplementedException();
        }

        private protected override void AfterSettingPageType()
        {
            throw new System.NotImplementedException();
        }
    }
}
