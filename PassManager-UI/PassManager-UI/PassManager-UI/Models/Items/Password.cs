using System;
using System.Collections.Generic;
using System.Text;

namespace PassManager.Models.Items
{
    public  class Password
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
    }
}
