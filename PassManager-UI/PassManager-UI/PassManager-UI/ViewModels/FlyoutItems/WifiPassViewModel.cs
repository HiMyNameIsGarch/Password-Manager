using PassManager.Models.Items;
using PassManager.Models.Interfaces;
using PassManager.Enums;

namespace PassManager.ViewModels.FlyoutItems
{
    public class WifiPassViewModel : BaseListItemVM
    {
        public WifiPassViewModel(IPageService pageService) : base(pageService,"Wifi")
        {
            if (CheckInternet())
            {
                //add some data for page
                AddDataForAndroid();
            }
        }

        private protected override void RefreshPage()
        {
            IsRefreshing = true;
            //code here
            IsRefreshing = false;
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
