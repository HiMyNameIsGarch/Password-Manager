using PassManager.Models.Items;
using System.Collections.ObjectModel;
using PassManager.Enums;
using System;
using System.Threading.Tasks;
using PassManager.Models;
using System.Collections.Generic;
using PassManager.Models.Api;
using System.Linq;

namespace PassManager.ViewModels.FlyoutItems
{
    public class EntireItemsViewModel : BaseListItemVM
    {
        public EntireItemsViewModel() : base("All items")
        {
            if (CheckInternet())
            {
                GetData().Await(HandleException, true, true, false);
            }
        }

        private protected async override Task GetData()
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

        private protected override Task RefreshPage()
        {
            throw new NotImplementedException();
        }

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
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 1,
            //        Title = "Home",
            //        SubTitle = "Wifi",
            //        ItemType = TypeOfItems.Wifi
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 2,
            //        Title = "Coffee shop",
            //        SubTitle = "Wifi",
            //        ItemType = TypeOfItems.Wifi
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 3,
            //        Title = "My friends house",
            //        SubTitle = "Wifi",
            //        ItemType = TypeOfItems.Wifi
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 4,
            //        Title = "League of legends",
            //        SubTitle = "hanu70@yahoo.com",
            //        ItemType = TypeOfItems.Password
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 1,
            //        Title = "Home",
            //        SubTitle = "Wifi",
            //        ItemType = TypeOfItems.Wifi
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 2,
            //        Title = "Coffee shop",
            //        SubTitle = "Wifi",
            //        ItemType = TypeOfItems.Wifi
            //    },
            //    new ItemPreview()
            //    {
            //        Id = 3,
            //        Title = "My friends house",
            //        SubTitle = "Wifi",
            //        ItemType = TypeOfItems.Wifi
            //    }
            //};
        }
    }
}
