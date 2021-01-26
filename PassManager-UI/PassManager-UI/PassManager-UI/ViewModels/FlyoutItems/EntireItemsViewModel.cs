using PassManager.Models.Items;
using System.Threading.Tasks;
using PassManager.Models;
using System.Collections.Generic;
using PassManager.Models.Api;
using PassManager.ViewModels.Bases;
using PassManager.Models.Api.Processors;

namespace PassManager.ViewModels.FlyoutItems
{
    public class EntireItemsViewModel : BaseListItemVM
    {
        public EntireItemsViewModel() : base("All items")
        {
            if (IsInternet())
            {
                GetDataAsync().AwaitWithPopup(HandleException, false);
            }
        }
        private protected async override Task GetDataAsync()
        {
            IEnumerable<Grouping<string, ItemPreview>> previews = await EntireItemsProcessor.GetPreviews(ApiHelper.ApiClient);
            DisplayItems(previews);
        }
        private protected override async Task<IEnumerable<Grouping<string, ItemPreview>>> RefreshPageAsync()
        {
            var previews = await EntireItemsProcessor.GetPreviews(ApiHelper.ApiClient);
            return previews;
        }
    }
}
