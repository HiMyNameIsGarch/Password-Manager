using PassManager.Models.Items;
using System.Collections.ObjectModel;
using PassManager.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using PassManager.Models;
using PassManager.Enums;
using System.Linq;

namespace PassManager.ViewModels.FlyoutItems
{
    public class PasswordViewModel : BaseListItemVM
    {
        public PasswordViewModel() : base("Passwords")
        {
            if (CheckInternet())
            {
                GetData().Await(HandleException,true,true,false);
            }
        }
        //functions
        private protected override async Task GetData()
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
            //Items = new ObservableCollection<ItemPreview>()
            //{
            //    new ItemPreview()
            //    {
            //        Id = 1,
            //        Title = "Facebook",
            //        SubTitle = "gabrielhanu70@yahoo.com",
            //        ItemType = TypeOfItems.Password
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 2,
            //        Title = "Instagram",
            //        SubTitle = "gabihanu23@yahoo.com",
            //        ItemType = TypeOfItems.Password
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 3,
            //        Title = "Champion gg",
            //        SubTitle = "gabriel70@yahoo.com",
            //        ItemType = TypeOfItems.Password
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 4,
            //        Title = "League of legends",
            //        SubTitle = "hanu70@yahoo.com",
            //        ItemType = TypeOfItems.Password
            //    }
            //};
        }
        private protected override Task RefreshPage()
        {
            throw new NotImplementedException();
        }
    }
}
