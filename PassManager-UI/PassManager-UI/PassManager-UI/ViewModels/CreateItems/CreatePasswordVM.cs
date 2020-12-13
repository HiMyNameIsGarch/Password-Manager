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
    [QueryProperty("PageType", "pageType")]
    [QueryProperty("Id","id")]
    public class CreatePasswordVM : BaseItemVM, IBackButtonBehavior
    {
        public CreatePasswordVM()
        {
            //set defaults values in case no parameter passed
            ChangeProps(ItemPageState.Null,"Create","No data provided",true);
            _goBack = new Command(GoBackButton);
        }
        //variables
        private string _id;
        private Password _password = new Password();
        private string _pageType;
        private ICommand _goBack;
        //props
        public string Id
        {
            get { return _id; }
            set 
            {
                _id = Uri.UnescapeDataString(value ?? string.Empty);
                if(PageState != ItemPageState.Null)
                {
                    if(int.TryParse(_id, out int newId))
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
        public string PageType
        {
            get { return _pageType; }
            set 
            {
                _pageType = Uri.UnescapeDataString(value ?? string.Empty);
                Enum.TryParse(_pageType, out ItemPageState pageState);
                PageState = pageState;
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
        }
        public Password Password {
            get { return _password; }
            set { _password = value; } 
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
                NotifyPropertyChanged("Password");
            }
        }
        //override basic actions for password
        private protected async override Task Create()
        {
            bool isSuccess = await PasswordProcessor.CreatePassword(ApiHelper.ApiClient,Password);
            if (true)
            {
                await GoTo("Password");
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
    }
}
