using System.Windows.Input;
using PassManager.Models.Items;
using Xamarin.Forms;
using PassManager.Models.Api;
using System.Threading.Tasks;
using PassManager.Models;
using PassManager.Views.Popups;
using PassManager.ViewModels.Bases;
using PassManager.Models.Api.Processors;
using PassManager.Enums;
using PassManager.Models.CallStatus;

namespace PassManager.ViewModels.CreateItems
{
    public class CreatePasswordVM : BaseItemVM
    {
        public CreatePasswordVM() : base(TypeOfItems.Password)
        {
            //set some defaults values
            IsPasswordVisible = true;
            PassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.Locked.png");
            //set commands
            _copyUsername = new Command(CopyUsernameToClipboard);
            _copyPassword = new Command(CopyPasswordToClipboard);
            _copyUrl = new Command(CopyUrlToClipboard);
            _changeVisOfPassword = new Command(ChangeVisOfPass);
            _password = new Password();
        }
        //variables
        private Password _password;
        private Password _tempPassword;
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
        //implementation for commands
        private async void CopyUsernameToClipboard() { await CopyToClipboard(Password.Username); }
        private async void CopyPasswordToClipboard() { await CopyToClipboard(Password.PasswordEncrypted); }
        private async void CopyUrlToClipboard() { await CopyToClipboard(Password.Url); }
        private void ChangeVisOfPass()
        {
            PassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.{(IsPasswordVisible ? "Open" : "Locked")}.png");
            IsPasswordVisible = !IsPasswordVisible;
        }
        //override functions
        private protected override bool IsItemChanged()
        {
            return Password.IsChanged(_tempPassword);
        }
        private protected async override Task GetDataAsync(int id)
        {
            Password password = await PasswordProcessor.GetPassword(ApiHelper.ApiClient, id);
            if (password != null)
            {
                var decryptedPass = (Password)DecryptItem(password);
                Password = decryptedPass;
                _tempPassword = (Password)decryptedPass.Clone();//store a temp password for future verifications
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and we couldn't get your password, try again!"));
        }
        private protected async override Task<bool> CreateAsync()
        {
            var encryptedPass = (Password)EncryptItem(Password);
            bool isSuccess = await PasswordProcessor.CreatePassword(ApiHelper.ApiClient, encryptedPass);
            return isSuccess;
        }
        private protected async override Task<DeleteCallStatus> DeleteAsync()
        {
            bool isSuccess = await PasswordProcessor.Delete(ApiHelper.ApiClient, Password.Id);
            return new DeleteCallStatus(isSuccess, Password.Id);
        }
        private protected async override Task<ModifyCallStatus> ModifyAsync(int id)
        {
            var encryptedPass = (Password)EncryptItem(Password);
            bool isSuccess = await PasswordProcessor.Modify(ApiHelper.ApiClient, id, encryptedPass);
            return new ModifyCallStatus(isSuccess, (_tempPassword.Name != Password.Name) || (_tempPassword.Username != Password.Username), new ItemPreview(Password.Id, Password.Name, Password.Username, TypeOfItems.Password));
        }
        private protected override Models.TaskStatus IsModelValid()
        {
            string msgToDisplay = string.Empty;
            if (string.IsNullOrEmpty(Password.Name) || string.IsNullOrEmpty(Password.Username) || string.IsNullOrEmpty(Password.PasswordEncrypted))
                msgToDisplay = "You need to complete at least \"name\", \"username\" and \"password\" in order to save!";
            else if (Password.Name.Length > 64)
                msgToDisplay = "Your Name must be max 64 characters long!";
            else if (Password.Username.Length > 64)
                msgToDisplay = "Your Username must be max 64 characters long!";
            else if (Password.Url?.Length > 256)
                msgToDisplay = "Your URL must be max 264 characters long!";

            return Models.TaskStatus.Status(msgToDisplay);
        }
        private protected override object EncryptItem(object obj)
        {
            var passwordToEncrypt = (Password)obj;
            passwordToEncrypt.PasswordEncrypted = VaultManager.EncryptString(passwordToEncrypt.PasswordEncrypted);
            passwordToEncrypt.Notes = VaultManager.EncryptString(passwordToEncrypt.Notes);
            return passwordToEncrypt;
        }
        private protected override object DecryptItem(object obj)
        {
            var passwordToDecrypt = (Password)obj;
            passwordToDecrypt.PasswordEncrypted = VaultManager.DecryptString(passwordToDecrypt.PasswordEncrypted);
            passwordToDecrypt.Notes = VaultManager.DecryptString(passwordToDecrypt.Notes);
            return passwordToDecrypt;
        }
    }
}
