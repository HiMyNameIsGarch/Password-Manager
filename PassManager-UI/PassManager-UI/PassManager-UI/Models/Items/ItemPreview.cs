namespace PassManager.Models.Items
{
    public class ItemPreview
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Enums.TypeOfItems ItemType { get; set; }
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
