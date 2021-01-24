using PassManager.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using PassManager.Models;
using PassManager.Models.Items;
using Xamarin.Forms;
using Newtonsoft.Json;
using PassManager.ViewModels.Bases;

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
            if (IsInternet())
            {
                //create object
                CreatePage pageToCreate = new CreatePage(ItemPageState.Create, -1);
                //serialize it
                string pageToCreateString = JsonConvert.SerializeObject(pageToCreate);
                //send it
                await Shell.Current.GoToAsync($"Create{SelectedPage.Name}?createPage={pageToCreateString}", false);
            }
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
