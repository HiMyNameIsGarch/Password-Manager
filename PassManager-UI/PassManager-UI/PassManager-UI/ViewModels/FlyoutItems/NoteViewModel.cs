using PassManager.Enums;
using PassManager.Models;
using PassManager.Models.Api;
using PassManager.Models.Api.Processors;
using PassManager.Models.Items;
using PassManager.ViewModels.Bases;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PassManager.ViewModels.FlyoutItems
{
    public class NoteViewModel : BaseListItemVM
    {
        public NoteViewModel() : base(TypeOfItems.Note)
        {
            if (IsInternet())
            {
                GetDataAsync().AwaitWithPopup(HandleException, false);
            }
        }
        //functions
        private protected override async Task GetDataAsync()
        {
            IEnumerable<Grouping<string, ItemPreview>> previews = await NoteProcessor.GetPreviews(ApiHelper.ApiClient);
            DisplayItems(previews);
        }
        private protected override async Task<IEnumerable<Grouping<string, ItemPreview>>> RefreshPageAsync()
        {
            IEnumerable<Grouping<string, ItemPreview>> newList = await NoteProcessor.GetPreviews(ApiHelper.ApiClient);
            return newList;
        }
    }
}
