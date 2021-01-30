using PassManager.Models.Items;

namespace PassManager.Models.CallStatus
{
    public class ModifyCallStatus
    {
        public ModifyCallStatus(bool isSuccess, bool itemChanged, ItemPreview item)
        {
            IsCallSucces = isSuccess;
            ItemPreviewChanged = itemChanged;
            ItemPreview = item;
        }
        public ItemPreview ItemPreview { get; set; }
        public bool ItemPreviewChanged { get; set; }
        public bool IsCallSucces { get; set; }
    }
}
