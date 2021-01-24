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
            Notes = wifi.Notes;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Notes { get; set; }
        public string IsModelValid()
        {
            throw new System.NotImplementedException();
        }
    }
}