using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PassManager.ViewModels
{
    public class EntireItemsViewModel : BaseListItemVM
    {
        public EntireItemsViewModel(IPageService pageService) : base(pageService)
        {
            if (CheckInternet())
            {
                AddDataForAndroid();
            }
            PageTitle = "All items";
        }
        private void AddDataForAndroid()
        {
            Passwords = new ObservableCollection<ItemPreview>()
            {
                new ItemPreview()
                {
                    Id = 1,
                    Title = "Facebook",
                    SubTitle = "gabrielhanu70@yahoo.com"
                },
                new ItemPreview()
                {
                    Id = 2,
                    Title = "Instagram",
                    SubTitle = "gabihanu23@yahoo.com"
                },
                new ItemPreview()
                {
                    Id = 3,
                    Title = "Champion gg",
                    SubTitle = "gabriel70@yahoo.com"
                },
                new ItemPreview()
                {
                    Id = 4,
                    Title = "League of legends",
                    SubTitle = "hanu70@yahoo.com"
                },
                new ItemPreview()
                {
                    Id = 1,
                    Title = "Home",
                    SubTitle = "Wifi"
                },
                new ItemPreview()
                {
                    Id = 2,
                    Title = "Coffee shop",
                    SubTitle = "Wifi"
                },
                new ItemPreview()
                {
                    Id = 3,
                    Title = "My friends house",
                    SubTitle = "Wifi"
                }
            };
        }
    }
}
