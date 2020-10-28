using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PassManager.Models
{
    public class ResponseToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
