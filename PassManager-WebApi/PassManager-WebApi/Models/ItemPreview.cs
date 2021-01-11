using PassManager_WebApi.Enums;

namespace PassManager_WebApi.Models
{
    public class ItemPreview
    {
        public ItemPreview()
        {

        }
        public ItemPreview(int id, string title, string subTitle, TypeOfItems itemType)
        {
            Id = id;
            Title = title;
            SubTitle = subTitle;
            ItemType = itemType;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public TypeOfItems ItemType { get; set; }
    }
}