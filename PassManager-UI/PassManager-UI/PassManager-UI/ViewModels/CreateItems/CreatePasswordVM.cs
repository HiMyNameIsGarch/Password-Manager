using System.Windows.Input;
using PassManager.Models.Interfaces;
using PassManager.Models.Items;
using Xamarin.Forms;
using PassManager.Models.Api;
using System.Threading.Tasks;
using PassManager.Models;
using PassManager.Views.Popups;
using Rg.Plugins.Popup.Services;
using Newtonsoft.Json;
using PassManager.ViewModels.Bases;
using PassManager.Models.Api.Processors;

namespace PassManager.ViewModels.CreateItems
{
    //parameters
    public class CreatePasswordVM : BaseItemVM, IBackButtonBehavior
    {
        public CreatePasswordVM() : base(Enums.TypeOfItems.Password)
        {
            //set some defaults values
            IsPasswordVisible = true;
            PassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.Locked.png");
            //set commands
            _goBack = new Command(GoBackButton);
            _copyUsername = new Command(CopyUsernameToClipboard);
            _copyPassword = new Command(CopyPasswordToClipboard);
            _copyUrl = new Command(CopyUrlToClipboard);
            _changeVisOfPassword = new Command(ChangeVisOfPass);
            _password = new Password();
        }
        //variables
        private Password _password;
        private Password _tempPassword;
        private ICommand _goBack;
        private ICommand _copyUrl;
        private ICommand _copyUsername;
        private ICommand _copyPassword;
        private ICommand _changeVisOfPassword;
        private bool _isPasswordVisible;
        private ImageSource _passEntryIcon;
        //props
        public bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            private set { _isPasswordVisible = value; NotifyPropertyChanged(); }
        }
        public ImageSource PassEntryIcon
        {
            get { return _passEntryIcon; }
            private set { _passEntryIcon = value; NotifyPropertyChanged(); }
        }
        public Password Password {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged(); }
        }
        //commands
        public ICommand ChangeVisOfPassword
        {
            get { return _changeVisOfPassword; }
        }
        public ICommand CopyUsername
        {
            get { return _copyUsername; }
        }
        public ICommand CopyUrl
        {
            get { return _copyUrl; }
        }
        public ICommand CopyPassword
        {
            get { return _copyPassword; }
        }
        public ICommand GoBack
        {
            get { return _goBack; }
        }
        //implementation for commands
        public async void GoBackButton()
        {
            if (Password.IsChanged(_tempPassword))
            {
                bool wantsToLeave = await PageService.DisplayAlert("Wait!", "Are you sure you want to leave?", "Yes", "No");
                if (wantsToLeave)
                {
                    await Shell.Current.Navigation.PopToRootAsync();
                }
            }
            else
            {
                await Shell.Current.Navigation.PopToRootAsync();
            }
        }
        //functions
        private protected async override Task GetDataAsync(int id)
        {
            if (IsInternet())
            {
                Password password = await PasswordProcessor.GetPassword(ApiHelper.ApiClient, id);
                if (password != null)
                {
                    var decryptedPass = (Password)DecryptItem(password);
                    Password = decryptedPass;
                    _tempPassword = (Password)decryptedPass.Clone();//store a temp password for future verifications
                }
                else
                {
                    await PageService.PushPopupAsync(new ErrorView("Something went wrong and we couldn't get your password, try again!"));
                }
            }
        }
        private protected async override Task Create()
        {
            var encryptedPass = (Password)EncryptItem(Password);
            bool isSuccess = await PasswordProcessor.CreatePassword(ApiHelper.ApiClient, encryptedPass);
            if (isSuccess)
            {
                UpdateModel Model = new UpdateModel(Enums.TypeOfUpdates.Create);
                string stringModel = JsonConvert.SerializeObject(Model);
                await GoTo("Password", $"?update={stringModel}");
            }
            else
            {
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your password has not been created, try again!"));
            }
        }
        private protected async override Task Delete()
        {
            bool isSuccess = await PasswordProcessor.Delete(ApiHelper.ApiClient, Password.Id);
            if (isSuccess)
            {
                UpdateModel Model = new UpdateModel(Enums.TypeOfUpdates.Delete, Password.Id);
                string stringModel = JsonConvert.SerializeObject(Model);
                await GoTo("Password", $"?update={stringModel}");
            }
            else
            {
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your password has not been deleted, try again!"));
            }
        }
        private protected async override Task Modify(int id)
        {
            var encryptedPass = (Password)EncryptItem(Password);
            bool isSuccess = await PasswordProcessor.Modify(ApiHelper.ApiClient, id, Password);
            if (isSuccess)
            {
                if ((_tempPassword.Name != Password.Name) || (_tempPassword.Username != Password.Username))//if some props from itempreviews changed, then update the item
                {
                    UpdateModel Model = new UpdateModel(Enums.TypeOfUpdates.Modify);
                    string stringModel = JsonConvert.SerializeObject(Model);
                    await GoTo("Password", $"?update={stringModel}");
                }
                else
                {
                    await GoTo("Password");
                }
            }
            else
            {
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your password has not been modified, try again!"));
            }
        }
        private protected async override Task<bool> IsModelValid()
        {
            string msgToDisplay = string.Empty;
            if (string.IsNullOrEmpty(Password.Name) || string.IsNullOrEmpty(Password.Username) || string.IsNullOrEmpty(Password.PasswordEncrypted))
            {
                msgToDisplay = "You need to complete at least 'name', 'username' and 'password' in order to save!";
            }
            else if (Password.Name.Length > 64)
            {
                msgToDisplay = "Your name must be max 64 characters long!";
            }
            else if (Password.Username.Length > 64)
            {
                msgToDisplay = "Your username must be max 64 characters long!";
            }
            else if (Password.Url?.Length > 256)
            {
                msgToDisplay = "Your URL must be max 264 characters long!";
            }
            if (string.IsNullOrEmpty(msgToDisplay))
            {
                return true;
            }
            else
            {
                //pop all popups
                await PopupNavigation.Instance.PopAllAsync();
                //push main
                await PageService.PushPopupAsync(new ErrorView(msgToDisplay));
                return false;
            }
        }
        //something else
        private async void CopyUsernameToClipboard() { await CopyToClipboard(Password.Username); }
        private async void CopyPasswordToClipboard() { await CopyToClipboard(Password.PasswordEncrypted); }
        private async void CopyUrlToClipboard() { await CopyToClipboard(Password.Url); }
        private void ChangeVisOfPass()
        {
            PassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.{(IsPasswordVisible ? "Open" : "Locked")}.png");
            IsPasswordVisible = !IsPasswordVisible;
        }
        private protected override object EncryptItem(object obj)
        {
            var passwordToEncrypt = (Password)obj;
            passwordToEncrypt.PasswordEncrypted = VaultManager.EncryptString(passwordToEncrypt.PasswordEncrypted);
            if (!string.IsNullOrEmpty(passwordToEncrypt.Notes))
            {
                passwordToEncrypt.Notes = VaultManager.EncryptString(passwordToEncrypt.Notes);
            }
            return passwordToEncrypt;
        }
        private protected override object DecryptItem(object obj)
        {
            var passwordToDecrypt = (Password)obj;
            passwordToDecrypt.PasswordEncrypted = VaultManager.DecryptString(passwordToDecrypt.PasswordEncrypted);
            if (!string.IsNullOrEmpty(passwordToDecrypt.Notes))
            {
                passwordToDecrypt.Notes = VaultManager.DecryptString(passwordToDecrypt.Notes);
            }
            return passwordToDecrypt;
        }
    }
}
