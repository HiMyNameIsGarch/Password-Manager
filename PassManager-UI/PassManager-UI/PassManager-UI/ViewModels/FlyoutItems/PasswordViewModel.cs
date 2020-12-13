using PassManager.Models.Items;
using System.Collections.ObjectModel;
using PassManager.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using PassManager.Models;
using PassManager.Enums;
using System.Linq;
using Xamarin.Forms;

namespace PassManager.ViewModels.FlyoutItems
{
    [QueryProperty("Update","update")]
    public class PasswordViewModel : BaseListItemVM
    {
        public PasswordViewModel() : base("Passwords")
        {
            if (CheckInternet())
            {
                GetData().Await(ErrorCallBack);
                //add data for page
                //AddDataForAndroid();
            }
        }
        //private variables
        private string _update;
        //parameters
        public string Update
        {
            get { return _update; }
            set
            {
                _update = Uri.UnescapeDataString(value ?? string.Empty);
                if (Boolean.TryParse(_update, out bool refresh))
                {
                    if (refresh)
                    {
                        GetData().Await(ErrorCallBack);
                    }
                }
            }
        }
        //functions
        private void ErrorCallBack(Exception ex)
        {

        }
        private async Task GetData()
        {
            IEnumerable<ItemPreview> previews = await PasswordProcessor.GetPreviews(ApiHelper.ApiClient);
            if(previews != null && previews.Count() > 0)
            {
                Passwords = new ObservableCollection<ItemPreview>(previews);
            }
        }
        //this function is for android testing
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
                }
            };
        }
        private protected override void RefreshPage()
        {
            IsRefreshing = true;
            GetData().Await(ErrorCallBack);
            IsRefreshing = false;
        }
    }
}
