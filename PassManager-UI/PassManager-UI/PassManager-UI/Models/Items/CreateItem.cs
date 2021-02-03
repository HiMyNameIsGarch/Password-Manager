using PassManager.Enums;

namespace PassManager.Models.Items
{
    public class CreateItem
    {
        public CreateItem(TypeOfItems itemType)
        {
            Name = itemType.ToSampleString();
            IconUrl = IconHelper.GetImageUrl(itemType);
        }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
