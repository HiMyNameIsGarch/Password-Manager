using PassManager.Enums;

namespace PassManager.Models.Items
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
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            ItemPreview item = (ItemPreview)obj;
            return (this.Id == item.Id) && (this.Title == item.Title) && (this.SubTitle == item.SubTitle) && (this.ItemType == item.ItemType);
        }
    }
}
