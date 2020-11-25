using System;
using System.Collections.Generic;
using System.Text;
using PassManager.Models.Items;
using Xamarin.Forms;

namespace PassManager.ViewModels
{
    public class ListItemViewModel
    {
        public ListItemViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }
        private IPageService _pageService;
        private CreateItem _selectedItem;
        public CreateItem SelectedPage {
            get { return _selectedItem; }
            set
            {
                if(_selectedItem != value)
                {
                    _selectedItem = value;
                }
                if(_selectedItem != null) HandleSelectedItem();
            } 
        }
        private async void HandleSelectedItem()
        {
            await Xamarin.Forms.Shell.Current.GoToAsync($"Create{SelectedPage.Name}", false);
        }
        public IEnumerable<CreateItem> ListItems
        {
            get {
                return new List<CreateItem>
                {
                    new CreateItem("Password", string.Empty),
                    new CreateItem("Wifi", string.Empty),
                };
            }
        }
    }
}
