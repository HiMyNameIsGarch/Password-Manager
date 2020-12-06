namespace PassManager_WebApi.ViewModels
{
    public class PasswordVM
    {
        public PasswordVM() { }
        public PasswordVM(Models.Password password)
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
    }
}