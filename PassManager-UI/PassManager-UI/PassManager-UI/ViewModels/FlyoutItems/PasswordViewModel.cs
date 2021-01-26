using PassManager.Models.Items;
using PassManager.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using PassManager.Models;
using PassManager.ViewModels.Bases;
using PassManager.Models.Api.Processors;

namespace PassManager.ViewModels.FlyoutItems
{
    public class PasswordViewModel : BaseListItemVM
    {
        public PasswordViewModel() : base("Passwords")
        {
            if (IsInternet())
            {
                GetDataAsync().AwaitWithPopup(HandleException, false);
            }
        }
        //functions
        private protected override async Task GetDataAsync()
        {
            IEnumerable<Grouping<string, ItemPreview>> previews = await PasswordProcessor.GetPreviews(ApiHelper.ApiClient);
            DisplayItems(previews);
        }
        private protected override async Task<IEnumerable<Grouping<string, ItemPreview>>> RefreshPageAsync()
        {
            IEnumerable<Grouping<string, ItemPreview>> newList = await PasswordProcessor.GetPreviews(ApiHelper.ApiClient);
            return newList;
        }
    }
}
