using PassManager_WebApi.Models.Interfaces;

namespace PassManager_WebApi.ViewModels
{
    public class WifiVM : IModelValid
    {
        public WifiVM() { }
        public WifiVM(Models.Wifi wifi)
        {
            if (wifi is null) return;
            Id = wifi.Id;
            Name = wifi.Name;
            PasswordEncrypted = wifi.PasswordEncrypted;
            SSID = wifi.SSID;
            SettingsPassword = wifi.SettingsPassword;
            ConnectionType = wifi.ConnectionType;
            Notes = wifi.Notes;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Notes { get; set; }
        public string SSID { get; set; }
        public string SettingsPassword { get; set; }
        public string ConnectionType { get; set; }

        public string IsModelValid()
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(PasswordEncrypted))
                return "You need to complete at least \"Name\" and \"Password\" in order to save!";
            if (Name.Length > 64)
                return "Your Name must be maximum 64 characters!";
            if (ConnectionType?.Length > 108)
                return "Your Connection Type must be maximum 64 characters!";
            if (SSID?.Length > 108)
                return " must be maximum 64 characters!";
            return string.Empty;
        }
    }
}