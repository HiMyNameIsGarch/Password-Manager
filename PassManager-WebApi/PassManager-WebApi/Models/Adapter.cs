using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassManager_WebApi.Models
{
    public partial class Password
    {
        public Password() { }
        public Password(ViewModels.PasswordVM passwordVM)
        {
            ModifyTo(passwordVM);
            CreateDate = DateTime.Now;
            LastVisited = DateTime.Now;
        }
        public void ModifyTo(ViewModels.PasswordVM passwordVM)
        {
            Name = passwordVM.Name;
            Username = passwordVM.Username;
            PasswordEncrypted = passwordVM.PasswordEncrypted;
            Url = passwordVM.Url;
            Notes = passwordVM.Notes;
            LastModified = DateTime.Now;
        }
    }
    public partial class Wifi
    {
        public Wifi() { }
        public Wifi(ViewModels.WifiVM wifiVM)
        {
            Name = wifiVM.Name;
            PasswordEncrypted = wifiVM.PasswordEncrypted;
            Notes = wifiVM.Notes;
            CreateDate = DateTime.Now;
            LastVisited = DateTime.Now;
            LastModified = DateTime.Now;
        }
    }
}