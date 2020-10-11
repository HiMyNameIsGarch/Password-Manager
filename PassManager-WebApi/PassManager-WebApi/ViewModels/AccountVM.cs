using PassManager_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassManager_WebApi.ViewModels
{
    public class AccountVM : AccountPreviewVM
    {
        public AccountVM ()
        {
        }
        public AccountVM(Account account) : base(account)
        {
            this.Username = account.Username;
            this.Email = account.Email;
            this.Phone = account.Phone;
            this.PasswordEncrypted = account.PasswordEncrypted;
            this.Notes = account.Notes;
        }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Notes { get; set; }
    }
}