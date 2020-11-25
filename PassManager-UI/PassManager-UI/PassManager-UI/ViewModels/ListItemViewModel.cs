using System;
using System.Collections.Generic;
using System.Text;
using PassManager.Models.Items;

namespace PassManager.ViewModels
{
    public class ListItemViewModel
    {
        private CreateItem _selectedItem;
        public CreateItem SelectedPage {
            get { return _selectedItem; }
            set
            {
                if(_selectedItem != value) _selectedItem = value;
                if(_selectedItem != null)
                {
                    HandleSelectedItem();
                    _selectedItem = null;
                }
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
