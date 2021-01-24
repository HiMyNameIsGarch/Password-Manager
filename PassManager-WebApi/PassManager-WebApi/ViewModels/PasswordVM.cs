using PassManager_WebApi.Models.Interfaces;
using PassManager_WebApi.Models;

namespace PassManager_WebApi.ViewModels
{
    public class PasswordVM : IModelValid
    {
        public PasswordVM() { }
        public PasswordVM(Password password)
        {
            if (password is null) return;
            Id = password.Id;
            Name = password.Name;
            Username = password.Username;
            PasswordEncrypted = password.PasswordEncrypted;
            Url = password.Url;
            Notes = password.Notes;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }

        public string IsModelValid()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(PasswordEncrypted))
                return "You need to complete at least \"'Name'\", \"Username\" and \"Password\" in order to save!";
            if (Name.Length > 64)
                return "Your Username must be maximum 64 characters!";
            if (Username.Length > 64)
                return "Your Username must be maximum 64 characters!";
            if (Url?.Length > 256)
                return "Your Url must be maximum 256 characters!";
            return string.Empty;
        }
    }
}