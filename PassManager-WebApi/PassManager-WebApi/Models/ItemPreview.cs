using PassManager_WebApi.Enums;

namespace PassManager_WebApi.Models
{
    public class ItemPreview
    {
        public ItemPreview()
        {

        }
        public ItemPreview(int id, string title, string subTitle, TypeOfItems itemType, string iconUrl)
        {
            Id = id;
            Title = title;
            SubTitle = subTitle;
            ItemType = itemType;
            IconUrl = iconUrl;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public TypeOfItems ItemType { get; set; }
        public string IconUrl { get; set; }
    }
}