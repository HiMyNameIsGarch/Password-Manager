using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassManager_WebApi.ViewModels
{
    public class WifiVM
    {
        public WifiVM() { }
        public WifiVM(Models.Wifi wifi)
        {
            Id = wifi.Id;
            Name = wifi.Name;
            PasswordEncrypted = wifi.PasswordEncrypted;
            Notes = wifi.Notes;
            CreateDate = DateTime.Now;
            LastVisited = DateTime.Now;
            LastModified = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Notes { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastVisited { get; set; }
        public DateTime LastModified { get; set; }
    }
}