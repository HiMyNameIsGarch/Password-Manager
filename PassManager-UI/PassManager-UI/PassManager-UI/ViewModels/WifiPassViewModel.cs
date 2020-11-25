using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace PassManager.ViewModels
{
    public class WifiPassViewModel : BaseListItemVM
    {
        public WifiPassViewModel(IPageService pageService) : base(pageService)
        {
            if (CheckInternet())
            {
                //add some data for page
                AddDataForAndroid();
            }
            PageTitle = "Wifi";
        }
        //function for android testing
        private void AddDataForAndroid()
        {
            Passwords = new System.Collections.ObjectModel.ObservableCollection<ItemPreview>()
            {
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
