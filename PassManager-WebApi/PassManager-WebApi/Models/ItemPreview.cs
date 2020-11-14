using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassManager_WebApi.Models
{
    public class ItemPreview
    {
        public ItemPreview(int id, string title, string subTitle)
        {
            Id = id;
            Title = title;
            SubTitle = subTitle;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}