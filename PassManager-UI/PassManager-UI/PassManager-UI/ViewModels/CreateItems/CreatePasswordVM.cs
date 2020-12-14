using System;
using System.Windows.Input;
using PassManager.Models.Interfaces;
using PassManager.Models.Items;
using Xamarin.Forms;
using PassManager.Enums;
using PassManager.Models.Api;
using System.Threading.Tasks;
using PassManager.Models;

namespace PassManager.ViewModels.CreateItems
{
    //parameters
    public class CreatePasswordVM : BaseItemVM, IBackButtonBehavior
    {
        public CreatePasswordVM()
        {
            _goBack = new Command(GoBackButton);
        }
        //variables
        private Password _password = new Password();
        private ICommand _goBack;
        //props
        public Password Password {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged(); } 
        }
        //commands
        public ICommand GoBack
        {
            get { return _goBack; }
        }
        //implementation for commands
        public async void GoBackButton()
        {
            try
            {
                await Shell.Current.GoToAsync("../..", true);
            }
            catch
            {
                await Shell.Current.GoToAsync("..", true);
            }
        }
        //functions
        private async Task GetData(int id)
        {
            Password password = await PasswordProcessor.GetPassword(ApiHelper.ApiClient,id);
            if(password != null)
            {
                Password = password;
            }
        }
        //override basic actions for password
        private protected async override Task Create()
        {
            bool isSuccess = await PasswordProcessor.CreatePassword(ApiHelper.ApiClient,Password);
            if (isSuccess)
            {
                await GoTo("Password", "?update=true");
            }
            else
            {
                //handle error(password not good or exception)
            }
        }
        private protected override Task Delete()
        {
            throw new NotImplementedException();
        }
        private protected async override Task Modify()
        {
            if(Id != string.Empty)
            {
                if (int.TryParse(Id, out int newId))
                {
                    bool isSuccess = await PasswordProcessor.Modify(ApiHelper.ApiClient, newId, Password);
                    if (isSuccess)
                    {
                        await GoTo("Password", "?update=true");
                    }
                    else
                    {
                        //handle errors
                    }
                }
                else
                {
                    //handle errors
                }
            }
            else
            {
                //handle errors
            }
        }

        private protected override void AfterSettingId()
        {
            switch (PageState)
            {
                case ItemPageState.Create:
                    PageTitle = "Create Password!";
                    ReadOnly = false;
                    break;
                case ItemPageState.View:
                    ChangeProps(ItemPageState.View, "Edit", "View Password", true);
                    break;
                case ItemPageState.Edit:
                    PageTitle = "Edit Password";
                    break;
                default:
                    PageTitle = "Your item is invalid!";
                    break;
            }
        }
        private protected override void AfterSettingPageType()
        {
            if (PageState != ItemPageState.Null)
            {
                if (int.TryParse(Id, out int newId))
                {
                    GetData(newId).Await();
                }
                else
                {
                    //handle error from id
                }
            }
            else
            {
                //handle error
                PageTitle = "Your item is invalid!";
            }
        }
    }
}
