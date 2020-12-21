using System.Windows.Input;
using PassManager.Models.Interfaces;
using PassManager.Models.Items;
using Xamarin.Forms;
using PassManager.Models.Api;
using System.Threading.Tasks;
using PassManager.Models;

namespace PassManager.ViewModels.CreateItems
{
    //parameters
    public class CreatePasswordVM : BaseItemVM, IBackButtonBehavior
    {
        public CreatePasswordVM() : base(Enums.TypeOfItems.Password)
        {
            _goBack = new Command(GoBackButton);
            _password = new Password();
        }
        //variables
        private Password _password;
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
            await Shell.Current.Navigation.PopToRootAsync();
        }
        //functions
        private protected async override Task GetData(int id)
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
                await GoTo("Password", $"?update={ManageFlyoutItems.GetValue("Passwords")}");
            }
            else
            {
                //handle error(password not good or exception)
            }
        }
        private protected async override Task Delete()
        {
            bool isSuccess = await PasswordProcessor.Delete(ApiHelper.ApiClient, Password.Id);
            if (isSuccess)
            {
                await GoTo("Password", "?update=true");
            }
            else
            {
                //handle error(password not deleted)
            }
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
