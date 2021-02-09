using PassManager.Models.Items;
using PassManager.Models.Api.Processors;
using System.Threading.Tasks;
using PassManager.ViewModels.Bases;
using PassManager.Models;
using PassManager.Models.Api;
using System.Collections.Generic;
using PassManager.Enums;

namespace PassManager.ViewModels.FlyoutItems
{
    public class WifiPassViewModel : BaseListItemVM
    {
        public WifiPassViewModel() : base(TypeOfItems.Wifi)
        {
            if (IsInternet())
            {
                GetDataAsync().AwaitWithPopup(PageService.HandleException, false);
            }
        }

        private protected override async Task GetDataAsync()
        {
            var previews = await WifiProcessor.GetPreviews(ApiHelper.ApiClient);
            DisplayItems(previews);
        }

        private protected override async Task<IEnumerable<Grouping<string, ItemPreview>>> RefreshPageAsync()
        {
            var newList = await WifiProcessor.GetPreviews(ApiHelper.ApiClient);
            return newList;
        }
    }
}
