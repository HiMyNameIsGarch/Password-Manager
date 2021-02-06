using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using PassManager.ViewModels.Bases;
using PassManager.Models.Items;
using PassManager.Models.Api.Processors;
using PassManager.Models.Api;
using PassManager.Views.Popups;
using PassManager.Models;
using PassManager.Enums;
using PassManager.Models.CallStatus;

namespace PassManager.ViewModels.CreateItems
{
    public class CreateWifiVM : BaseItemVM
    {
        public CreateWifiVM() : base(TypeOfItems.Wifi)
        {
            //set some default values
            IsPasswordVisible = IsSettingsPassVis = true;
            SettingsPassEntryIcon = PassEntryIcon = ImageSource.FromResource(IconHelper.GetImageUrl("Locked"));
            _wifi = new Wifi();
            //set commands
            _copyPassword = new Command(CopyPasswordToClipboard);
            _copySSID = new Command(CopySSIDToClipboard);
            _copySettingsPassword = new Command(CopySettingsPasswordToClipboard);
            _changeVisOfPassword = new Command(ChangeVisOfPass);
            _changeVisOfSettingsPassword = new Command(ChangeVisOfSettingsPass);
        }
        //private fields
        private Wifi _wifi;
        private Wifi _tempWifi;
        private ICommand _copyPassword;
        private ICommand _copySSID;
        private ICommand _copySettingsPassword;
        private ICommand _changeVisOfSettingsPassword;
        private ICommand _changeVisOfPassword;
        private bool _isSettingsPassVis;
        private bool _isPasswordVisible;
        private ImageSource _passEntryIcon;
        private ImageSource _settingsPassEntryIcon;
        //props
        public Wifi Wifi
        {
            get { return _wifi; }
            private set { _wifi = value; NotifyPropertyChanged(); }
        }
        public ImageSource SettingsPassEntryIcon
        {
            get { return _settingsPassEntryIcon; }
            private set { _settingsPassEntryIcon = value; NotifyPropertyChanged(); }
        }
        public ImageSource PassEntryIcon
        {
            get { return _passEntryIcon; }
            private set { _passEntryIcon = value; NotifyPropertyChanged(); }
        }
        public bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            private set { _isPasswordVisible = value; NotifyPropertyChanged(); }
        }
        public bool IsSettingsPassVis
        {
            get { return _isSettingsPassVis; }
            private set { _isSettingsPassVis = value; NotifyPropertyChanged(); }
        }
        //commands
        public ICommand CopyPassword
        {
            get { return _copyPassword; }
        }
        public ICommand CopySSID
        {
            get { return _copySSID; }
        }
        public ICommand CopySettingsPassword
        {
            get { return _copySettingsPassword; }
        }
        public ICommand ChangeVisOfPassword
        {
            get { return _changeVisOfPassword; }
        }
        public ICommand ChangeVisOfSettingsPassword
        {
            get { return _changeVisOfSettingsPassword; }
        }
        //implementaions for commands
        private async void CopyPasswordToClipboard() { await CopyToClipboard(Wifi.PasswordEncrypted); }
        private async void CopySSIDToClipboard() { await CopyToClipboard(Wifi.SSID); }
        private async void CopySettingsPasswordToClipboard() { await CopyToClipboard(Wifi.SettingsPassword); }
        private void ChangeVisOfSettingsPass()
        {
            SettingsPassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.{(IsSettingsPassVis ? "Open" : "Locked")}.png");
            IsSettingsPassVis = !IsSettingsPassVis;
        }
        private void ChangeVisOfPass()
        {
            PassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.{(IsPasswordVisible ? "Open" : "Locked")}.png");
            IsPasswordVisible = !IsPasswordVisible;
        }
        //override functions
        private protected override bool IsItemChanged()
        {
            return Wifi.IsChanged(_tempWifi);
        }
        private protected override async Task GetDataAsync(int id)
        {
            Wifi wifi = await WifiProcessor.GetWifi(ApiHelper.ApiClient, id);
            if (wifi != null)
            {
                var decryptedWifi = (Wifi)DecryptItem(wifi);
                Wifi = decryptedWifi;
                _tempWifi = (Wifi)decryptedWifi.Clone();//store a temp wifi for later verifications
            }
            else
            {
                await PageService.PushPopupAsync(new ErrorView(ErrorMsg.CouldNotGetItem(ItemType)));
            }
        }
        private protected override async Task<bool> CreateAsync()
        {
            var encryptedWifi = (Wifi)EncryptItem(Wifi);
            bool isSuccess = await WifiProcessor.CreateWifi(ApiHelper.ApiClient, encryptedWifi);
            return isSuccess;
        }
        private protected override async Task<ModifyCallStatus> ModifyAsync(int id)
        {
            var encryptedWifi = (Wifi)EncryptItem(Wifi);
            bool isSuccess = await WifiProcessor.Modify(ApiHelper.ApiClient, id, encryptedWifi);
            return new ModifyCallStatus(isSuccess, _tempWifi.Name != Wifi.Name, new ItemPreview(Wifi.Id, Wifi.Name, TypeOfItems.Wifi.ToSampleString(), TypeOfItems.Wifi));
        }
        private protected override async Task<DeleteCallStatus> DeleteAsync()
        {
            bool isSuccess = await WifiProcessor.Delete(ApiHelper.ApiClient, Wifi.Id);
            return new DeleteCallStatus(isSuccess, Wifi.Id);
        }
        private protected override Models.TaskStatus IsModelValid()
        {
            string msgToDisplay = string.Empty;

            if (string.IsNullOrEmpty(Wifi.Name) || string.IsNullOrEmpty(Wifi.PasswordEncrypted))
                msgToDisplay = ErrorMsg.CompleteFields("Name", "Password");
            else if (Wifi.Name.Length > 64)
                msgToDisplay = ErrorMsg.FieldMaxCharLong("Name", 64);
            else if (Wifi.SSID.Length > 64)
                msgToDisplay = ErrorMsg.FieldMaxCharLong("SSID", 64);
            else if (Wifi.ConnectionType.Length > 64)
                msgToDisplay = ErrorMsg.FieldMaxCharLong("Connection Type", 64);
            return Models.TaskStatus.Status(msgToDisplay);
        }
        private protected override object EncryptItem(object obj)
        {
            var wifiToEncrypt = (Wifi)obj;
            wifiToEncrypt.PasswordEncrypted = VaultManager.EncryptString(wifiToEncrypt.PasswordEncrypted);
            wifiToEncrypt.SSID = VaultManager.EncryptString(wifiToEncrypt.SSID);
            wifiToEncrypt.SettingsPassword = VaultManager.EncryptString(wifiToEncrypt.SettingsPassword);
            wifiToEncrypt.ConnectionType = VaultManager.EncryptString(wifiToEncrypt.ConnectionType);
            wifiToEncrypt.Notes = VaultManager.EncryptString(wifiToEncrypt.Notes);
            return wifiToEncrypt;
        }
        private protected override object DecryptItem(object obj)
        {
            var wifiToDecrypt = (Wifi)obj;
            wifiToDecrypt.PasswordEncrypted = VaultManager.DecryptString(wifiToDecrypt.PasswordEncrypted);
            wifiToDecrypt.SSID = VaultManager.DecryptString(wifiToDecrypt.SSID);
            wifiToDecrypt.SettingsPassword = VaultManager.DecryptString(wifiToDecrypt.SettingsPassword);
            wifiToDecrypt.ConnectionType = VaultManager.DecryptString(wifiToDecrypt.ConnectionType);
            wifiToDecrypt.Notes = VaultManager.DecryptString(wifiToDecrypt.Notes);
            return wifiToDecrypt;
        }
    }
}
