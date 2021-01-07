using PassManager.Enums;
using PassManager.Models;
using PassManager.Models.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassManager.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel() : base("Search...")
        {
            _items = new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>();
        }
        private ItemPreview _selectedItem;
        private ObservableCollection<Grouping<TypeOfItems, ItemPreview>> _items;
        public ObservableCollection<Grouping<TypeOfItems, ItemPreview>> Items
        {
            get { return _items; }
            private protected set { _items = value; NotifyPropertyChanged(); }
        }
        public bool IsUwp
        {
            get { return Device.RuntimePlatform == Device.UWP; }
        }
        public ItemPreview SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value) _selectedItem = value;
                if (_selectedItem != null)
                {
                    ViewSelectedItem(_selectedItem.Id, _selectedItem.ItemType).Await();
                    _selectedItem = null;
                    NotifyPropertyChanged();
                }
            }
        }
        private async Task ViewSelectedItem(int id, TypeOfItems itemType)
        {
            await Shell.Current.GoToAsync($"Create{itemType}?pageType=View&id={id}");
        }
    }
}
