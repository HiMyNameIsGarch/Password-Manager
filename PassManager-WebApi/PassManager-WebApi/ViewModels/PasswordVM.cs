using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassManager_WebApi.ViewModels
{
    public class PasswordVM
    {
        public PasswordVM() { }
        public PasswordVM(Models.Password password)
        {
            Id = password.Id;
            Name = password.Name;
            Username = password.Username;
            PasswordEncrypted = password.PasswordEncrypted;
            Url = password.Url;
            Notes = password.Notes;
            CreateDate = DateTime.Now;
            LastVisited = DateTime.Now;
            LastModified = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastVisited { get; set; }
        public DateTime LastModified { get; set; }
    }
}