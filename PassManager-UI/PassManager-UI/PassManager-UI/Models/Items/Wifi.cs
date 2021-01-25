using System;


namespace PassManager.Models.Items
{
    public class Wifi
    {
        public Wifi()
        {
            Name = PasswordEncrypted = Notes = SSID = SettingsPassword = ConnectionType = string.Empty;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Notes { get; set; }
        public string SSID { get; set; }
        public string SettingsPassword { get; set; }
        public string ConnectionType { get; set; }
    }
}
