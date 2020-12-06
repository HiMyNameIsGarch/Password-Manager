using PassManager.Models.Items;
using System.Collections.ObjectModel;
using PassManager.Models.Interfaces;
using PassManager.Enums;

namespace PassManager.ViewModels.FlyoutItems
{
    public class EntireItemsViewModel : BaseListItemVM
    {
        public EntireItemsViewModel(IPageService pageService) : base(pageService, "All items")
        {
            if (CheckInternet())
            {
                AddDataForAndroid();
            }
        }
        private void AddDataForAndroid()
        {
            Passwords = new ObservableCollection<ItemPreview>()
            {
                new ItemPreview()
                {
                    Id = 1,
                    Title = "Facebook",
                    SubTitle = "gabrielhanu70@yahoo.com",
                    ItemType = TypeOfItems.Password
                },
                new ItemPreview()
                {
                    Id = 2,
                    Title = "Instagram",
                    SubTitle = "gabihanu23@yahoo.com",
                    ItemType = TypeOfItems.Password
                },
                new ItemPreview()
                {
                    Id = 3,
                    Title = "Champion gg",
                    SubTitle = "gabriel70@yahoo.com",
                    ItemType = TypeOfItems.Password
                },
                new ItemPreview()
                {
                    Id = 4,
                    Title = "League of legends",
                    SubTitle = "hanu70@yahoo.com",
                    ItemType = TypeOfItems.Password
                },
                new ItemPreview()
                {
                    Id = 1,
                    Title = "Home",
                    SubTitle = "Wifi",
                    ItemType = TypeOfItems.Wifi
                },
                new ItemPreview()
                {
                    Id = 2,
                    Title = "Coffee shop",
                    SubTitle = "Wifi",
                    ItemType = TypeOfItems.Wifi
                },
                new ItemPreview()
                {
                    Id = 3,
                    Title = "My friends house",
                    SubTitle = "Wifi",
                    ItemType = TypeOfItems.Wifi
                }
            };
        }
    }
}
