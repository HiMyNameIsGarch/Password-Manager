using PassManager.Enums;

namespace PassManager.Models
{
    public class CreatePage
    {
        public CreatePage(ItemPageState pageState, int idForItem)
        {
            PageState = pageState;
            IdForItem = idForItem;
        }
        public ItemPageState PageState { get; }
        public int IdForItem { get; }
    }
}
