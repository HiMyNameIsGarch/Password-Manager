using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models.Interfaces;
using System.Threading.Tasks;
using PassManager.ViewModels.Bases;
using PassManager.Models.Items;
using PassManager.Models.Api.Processors;
using PassManager.Models.Api;
using PassManager.Views.Popups;
using PassManager.Models;
using PassManager.Enums;

namespace PassManager.ViewModels.CreateItems
{
    public class CreateWifiVM : BaseItemVM
    {
        public CreateWifiVM() : base(Enums.TypeOfItems.Wifi)
        {
            //set some default values
            IsPasswordVisible = IsSettingsPassVis = true;
            SettingsPassEntryIcon = PassEntryIcon = ImageSource.FromResource($"PassManager-UI.Images.Locked.png");
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
        public override async void GoBackButton()
        {
            if (Wifi.IsChanged(_tempWifi))
            {
                bool wantsToLeave = await PageService.DisplayAlert("Wait!", "Are you sure you want to leave?", "Yes", "No");
                if (wantsToLeave)
                    await Shell.Current.Navigation.PopToRootAsync();
            }
            else
                await Shell.Current.Navigation.PopToRootAsync(true);
        }
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
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and we couldn't get your wifi, try again!"));
            }
        }
        private protected override async Task Create()
        {
            var encryptedWifi = (Wifi)EncryptItem(Wifi);
            bool isSuccess = await WifiProcessor.CreateWifi(ApiHelper.ApiClient, encryptedWifi);
            if (isSuccess)
            {
                var latestCreatedItem = await EntireItemsProcessor.GetLatestCreated(ApiHelper.ApiClient, TypeOfItems.Wifi);
                if (latestCreatedItem is null)
                {
                    await PageService.PushPopupAsync(new ErrorView("Something went wrong and your password has not been created, try again!"));
                }
                else
                {
                    UpdateModel model = new UpdateModel(TypeOfUpdates.Create, latestCreatedItem);
                    await GoTo("Wifi", model);
                }
            }
            else
            {
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your wifi has not been created, try again!"));
            }
        }
        private protected override async Task Modify(int id)
        {
            if (!Wifi.IsChanged(_tempWifi)) await Shell.Current.Navigation.PopToRootAsync();
            var encryptedWifi = (Wifi)EncryptItem(Wifi);
            bool isSuccess = await WifiProcessor.Modify(ApiHelper.ApiClient, id, encryptedWifi);
            if (isSuccess)
            {
                if (_tempWifi.Name != Wifi.Name)
                {
                    UpdateModel model = new UpdateModel(TypeOfUpdates.Modify, new ItemPreview(Wifi.Id, Wifi.Name, TypeOfItems.Wifi.ToSampleString(), TypeOfItems.Wifi));
                    await GoTo("Wifi", model);
                }
                else
                    await GoTo("Wifi");
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your wifi has not been modified, try again!"));
        }
        private protected override async Task Delete()
        {
            bool isSuccess = await WifiProcessor.Delete(ApiHelper.ApiClient, Wifi.Id);
            if (isSuccess)
            {
                UpdateModel model = new UpdateModel(TypeOfUpdates.Delete, new ItemPreview() { Id = Wifi.Id, ItemType = TypeOfItems.Wifi });
                await GoTo("Wifi", model);
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your wifi has not been deleted, try again!"));
        }
        private protected override Models.TaskStatus IsModelValid()
        {
            string msgToDisplay = string.Empty;

            if (string.IsNullOrEmpty(Wifi.Name) || string.IsNullOrEmpty(Wifi.PasswordEncrypted))
                msgToDisplay = "You need to complete at least \"Name\" and \"Username\" in order to save!";
            else if (Wifi.Name.Length > 64)
                msgToDisplay = "Your Name must be max 64 characters long!";
            else if (Wifi.SSID.Length > 64)
                msgToDisplay = "Your SSID must be max 64 characters long!";
            else if (Wifi.ConnectionType.Length > 64)
                msgToDisplay = "Your Connection Type must be max 64 characters long!";

            if (string.IsNullOrEmpty(msgToDisplay))
                return new Models.TaskStatus(false, string.Empty);
            else 
                return new Models.TaskStatus(true, msgToDisplay);
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
