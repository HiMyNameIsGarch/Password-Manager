//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PassManager_WebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Wifi
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Notes { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastModified { get; set; }
        public string UserId { get; set; }
        public int NumOfVisits { get; set; }
        public string SSID { get; set; }
        public string SettingsPassword { get; set; }
        public string ConnectionType { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
