using PassManager.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using PassManager.Models;
using PassManager.Models.Items;
using Xamarin.Forms;

namespace PassManager.ViewModels
{
    public class ListItemViewModel : BaseViewModel
    {
        private CreateItem _selectedItem;
        public CreateItem SelectedPage {
            get { return _selectedItem; }
            set
            {
                if(_selectedItem != value) _selectedItem = value;
                if(_selectedItem != null)
                {
                    HandleSelectedItem().Await();
                    _selectedItem = null;
                    NotifyPropertyChanged();
                }
            } 
        }
        private async Task HandleSelectedItem()
        {
            await Shell.Current.GoToAsync($"Create{SelectedPage.Name}?pageType=Create", false);
        }
        public IEnumerable<CreateItem> ListItems
        {
            get {
                return new List<CreateItem>
                {
                    new CreateItem(TypeOfItems.Password, string.Empty),
                    new CreateItem(TypeOfItems.Wifi, string.Empty),
                };
            }
        }
    }
}
