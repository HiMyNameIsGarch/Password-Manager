using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PassManager.Models.Items
{
    public class CreateItem
    {
        public CreateItem(string name, string imageUrl)
        {
            Name = name;
            ImageUrl = imageUrl;
        }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
