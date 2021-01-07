using PassManager.Enums;

namespace PassManager.Models.Items
{
    public class CreateItem
    {
        public CreateItem(TypeOfItems item, string imageUrl)
        {
            Name = item;
            ImageUrl = imageUrl;
        }
        public TypeOfItems Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
