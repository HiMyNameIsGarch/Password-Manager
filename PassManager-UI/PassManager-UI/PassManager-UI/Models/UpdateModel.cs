using PassManager.Enums;
using PassManager.Models.Items;

namespace PassManager.Models
{
    public class UpdateModel
    {
        public UpdateModel(TypeOfUpdates updateType, ItemPreview item)
        {
            UpdateType = updateType;
            ItemPreview = item;
        }
        public TypeOfUpdates UpdateType { get; set; }
        public ItemPreview ItemPreview { get; set; }
    }
}
