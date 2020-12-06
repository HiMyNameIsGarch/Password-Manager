using PassManager.Models.Items;
using PassManager.Models.Interfaces;
using System.Collections.ObjectModel;
using PassManager.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using PassManager.Models;
using PassManager.Enums;

namespace PassManager.ViewModels.FlyoutItems
{
    public class PasswordViewModel : BaseListItemVM
    {
        public PasswordViewModel(IPageService pageService) : base(pageService, "Passwords")
        {
            if (CheckInternet())
            {
                GetData().Await(ErrorCallBack);
                //add data for page
                //AddDataForAndroid();
            }
        }
        private void ErrorCallBack(Exception ex)
        {

        }
        private async Task GetData()
        {
            IEnumerable<ItemPreview> previews = await PasswordProcessor.GetPreviews(ApiHelper.ApiClient);
            foreach (var item in previews)
            {
                Passwords.Add(item);
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
    }
}
