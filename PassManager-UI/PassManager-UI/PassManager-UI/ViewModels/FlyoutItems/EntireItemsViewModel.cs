using PassManager.Models.Items;
using System.Collections.ObjectModel;
using PassManager.Enums;
using System.Threading.Tasks;
using PassManager.Models;
using System.Collections.Generic;
using PassManager.Models.Api;
using System.Linq;
using PassManager.Views.Popups;
using Rg.Plugins.Popup.Services;

namespace PassManager.ViewModels.FlyoutItems
{
    public class EntireItemsViewModel : BaseListItemVM
    {
        public EntireItemsViewModel() : base("All items")
        {
            if (CheckInternet())
            {
                GetDataAsync().Await(HandleException, true, true, false);
            }
        }
        private protected async override Task GetDataAsync()
        {
            IEnumerable<Grouping<TypeOfItems, ItemPreview>> previews = await EntireItemsProcessor.GetPreviews(ApiHelper.ApiClient);
            if(previews != null && previews.Count() > 0)
            {
                DisplayMsg(string.Empty, false);
                Items = new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>(previews);
            }
            else if (previews.Count() == 0)
            {
                DisplayMsg("You have no items yet, click on button below to add a new one!",true);
            }
        }
        private protected override async Task RefreshPageAsync()
        {
            IEnumerable<Grouping<TypeOfItems, ItemPreview>> previews = await EntireItemsProcessor.GetPreviews(ApiHelper.ApiClient);
            await PageService.PopAllAsync(false);
            if (IsListChanged(previews))
            {
                Items = UpdateItems(previews);
            }
            else
            {
                await PageService.PushPopupAsync(new WarningView("Your items are up to date!"),false);
            }
        }
    }
}
