using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PassManager.Models.Interfaces;
using PassManager.Models.Items;
using Xamarin.Forms;
using PassManager.Enums;
using PassManager.Models.Api;

namespace PassManager.ViewModels.CreateItems
{
    [QueryProperty("PageType", "pageType")]
    [QueryProperty("Id","id")]
    public class CreatePasswordVM : BaseItemVM, IBackButtonBehavior
    {
        public CreatePasswordVM()
        {
            //set defaults values in case no parameter passed
            ChangeProps(ItemPageState.Null,"Create","No data provided",true);
            _goBack = new Command(GoBackButton);
        }
        //variables
        private string _id;
        private Password _password = new Password();
        private string _pageType;
        //props
        public string Id
        {
            get { return _id; }
            set 
            {
                _id = Uri.UnescapeDataString(value ?? string.Empty);
                if(PageState != ItemPageState.Null)
                {
                    //get data
                }
                else
                {
                    PageTitle = "Your item is invalid!";
                }
            }
        }
        public string PageType
        {
            get { return _pageType; }
            set 
            {
                _pageType = Uri.UnescapeDataString(value ?? string.Empty);
                Enum.TryParse(_pageType, out ItemPageState pageState);
                PageState = pageState;
                switch (PageState)
                {
                    case ItemPageState.Create:
                        PageTitle = "Create Password!";
                        ReadOnly = false;
                        break;
                    case ItemPageState.View:
                        ChangeProps(ItemPageState.View, "Edit", "View Password", true);
                        break;
                    case ItemPageState.Edit:
                        PageTitle = "Edit Password";
                        break;
                    default:
                        PageTitle = "Your item is invalid!";
                        break;
                }
            }
        }
        public Password Password {
            get { return _password; }
            set { _password = value; } 
        }
        //commands
        private ICommand _goBack;
        public ICommand GoBack
        {
            get { return _goBack; }
        }
        //implementation for commands
        public async void GoBackButton()
        {
            try
            {
                await Shell.Current.GoToAsync("../..", true);
            }
            catch
            {
                await Shell.Current.GoToAsync("..", true);
            }
        }
        //functions
        private protected override void Create()
        {
            //throw new NotImplementedException();
        }

        private protected override void Delete()
        {
            throw new NotImplementedException();
        }

        private protected override void Modify()
        {
            //throw new NotImplementedException();
        }
    }
}
