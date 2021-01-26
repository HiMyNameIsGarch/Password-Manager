using System;


namespace PassManager.Models.Items
{
    public class Wifi : ICloneable
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
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        internal bool IsChanged(Wifi wifi)
        {
            if(wifi is null)//check currect props if changed from default
            {
                return Name.Length > 0 || SSID.Length > 0 || PasswordEncrypted.Length > 0 || ConnectionType.Length > 0 || Notes.Length > 0 
                    || SettingsPassword.Length > 0;
            }
            return wifi.Name != Name || wifi.SSID != SSID || wifi.PasswordEncrypted != PasswordEncrypted || wifi.ConnectionType != ConnectionType
                || wifi.SettingsPassword != SettingsPassword || wifi.Notes != Notes;
        }
    }
}
