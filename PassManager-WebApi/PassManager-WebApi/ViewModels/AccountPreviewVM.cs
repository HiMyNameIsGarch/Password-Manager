using PassManager_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassManager_WebApi.ViewModels
{
    public class AccountPreviewVM
    {
        public AccountPreviewVM(){}
        public AccountPreviewVM(Account account)
        {
            Id = account.Id;
            Title = account.Title;
            SiteRegistered = account.SiteRegistered;
            LastVisited = account.LastVisited;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string SiteRegistered { get; set; }
        public DateTime LastVisited { get; set; }
    }
}