using System;

namespace PassManager.Models.Items
{
    public  class Password : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
