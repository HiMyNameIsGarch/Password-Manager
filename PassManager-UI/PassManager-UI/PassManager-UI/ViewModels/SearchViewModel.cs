using PassManager.Enums;
using PassManager.Models;
using PassManager.Models.Api;
using PassManager.Models.Items;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string _searchString;
        private int _maxLength = int.MaxValue;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value; 
                NotifyPropertyChanged();
                if (!string.IsNullOrEmpty(_searchString) && _searchString.Length < _maxLength)
                {
                    DisplayItems(_searchString).Await(HandleException, false, false, false);
                    _searchString = null;
                }
                else
                {
                    _maxLength = int.MaxValue;
                    Items = null;
                }
            }
        }
        private async Task DisplayItems(string searchString)
        {
            var items = await EntireItemsProcessor.GetPreviews(ApiHelper.ApiClient, searchString.ToLower());
            if(items.Count() > 0)
            {
                Items = new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>(items);
            }
            else
            {
                _maxLength = searchString.Length;
            }
        }
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
