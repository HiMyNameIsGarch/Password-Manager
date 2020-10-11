using PassManager_WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassManager_WebApi.Models
{
    public partial class Account
    {
        public Account()
        {

        }
        public Account(AccountVM account)
        {
            AdaptTo(account);
            this.LastModified = DateTime.Now;
            this.LastVisited = DateTime.Now;
        }
        public void AdaptTo(AccountVM account)
        {
            this.Title = account.Title;
            this.Username = account.Username;
            this.Email = account.Email;
            this.Phone = account.Phone;
            this.PasswordEncrypted = account.PasswordEncrypted;
            this.SiteRegistered = account.SiteRegistered;
            this.Notes = account.Notes;
            this.LastModified = DateTime.Now;
        }
    }
}