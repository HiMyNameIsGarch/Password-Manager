using System;

namespace PassManager.Models.Items
{
    public class Password : ICloneable
    {
        public Password()
        {
            Name = Username = PasswordEncrypted = Url = Notes = string.Empty;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        internal bool IsChanged(Password password)
        {
            if(password is null)
            {
                return Name.Length > 0 || Username.Length > 0 || PasswordEncrypted.Length > 0 || Url.Length > 0 
                    || Notes.Length > 0;
            }
            return password.Name != Name || password.Username != Username || password.PasswordEncrypted != PasswordEncrypted
                || password.Username != Username || password.Url != Url || password.Notes != Notes;
        }
    }
}
