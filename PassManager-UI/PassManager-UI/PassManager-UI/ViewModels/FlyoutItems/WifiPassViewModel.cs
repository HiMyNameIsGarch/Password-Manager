using System;
using PassManager.Models.Items;
using PassManager.Enums;
using System.Threading.Tasks;

namespace PassManager.ViewModels.FlyoutItems
{
    public class WifiPassViewModel : BaseListItemVM
    {
        public WifiPassViewModel() : base("Wifi")
        {
            if (CheckInternet())
            {
                //add some data for page
                AddDataForAndroid();
            }
        }

        private protected override Task RefreshPage()
        {
            throw new NotImplementedException();
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
