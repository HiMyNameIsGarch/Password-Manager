namespace PassManager.Models.Items
{
    public class CreateItem
    {
        public CreateItem(string item, string imageUrl)
        {
            Name = item;
            ImageUrl = imageUrl;
        }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
