using PassManager.Models.Items;
using System.Collections.ObjectModel;
using PassManager.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using PassManager.Models;
using PassManager.Enums;
using System.Linq;
using PassManager.Views.Popups;

namespace PassManager.ViewModels.FlyoutItems
{
    public class PasswordViewModel : BaseListItemVM
    {
        public PasswordViewModel() : base("Passwords")
        {
            if (CheckInternet())
            {
                GetDataAsync().Await(HandleException,true,true,false);
            }
        }
        //functions
        private protected override async Task GetDataAsync()
        {
            IEnumerable<Grouping<TypeOfItems, ItemPreview>> previews = await PasswordProcessor.GetPreviews(ApiHelper.ApiClient);
            if(previews != null && previews.Count() > 0)
            {
                DisplayMsg(string.Empty, false);
                Items = new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>(previews);
            }
            else if(previews.Count() == 0)
            {
                DisplayMsg("You have no passwords yet, click on button below to add a new one!", true);
            }
        }
        //this function is for android testing
        private void AddDataForAndroid()
        {
        }
        private protected override async Task RefreshPageAsync()
        {
            IEnumerable<Grouping<TypeOfItems, ItemPreview>> newList = await PasswordProcessor.GetPreviews(ApiHelper.ApiClient);
            await PageService.PopAllAsync(false);
            if (IsListChanged(newList))
            {
                Items = UpdateItems(newList);
            }
            else
            {
                await PageService.PushPopupAsync(new WarningView("Your passwords are up to date!"));
            }
        }
    }
}
