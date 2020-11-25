using PassManager.Models.Api;
using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PassManager.ViewModels
{
    public class PasswordViewModel : BaseListItemVM
    {
        public PasswordViewModel(IPageService pageService) : base(pageService)
        {
            if (CheckInternet())
            {
                //add data for page
                AddDataForAndroid();
            }
            PageTitle = "Passwords";
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
                }
            };
        }
    }
}
