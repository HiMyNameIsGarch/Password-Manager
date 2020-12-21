using PassManager.Models.Items;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using PassManager.Models;
using System;

namespace PassManager.ViewModels
{
    [QueryProperty("Update", "update")]
    public abstract class BaseListItemVM : BaseViewModel
    {
        //constructors
        public BaseListItemVM(string pageTitle) : base(pageTitle)
        {
            ManageFlyoutItems.Add(pageTitle, true);
            _addItem = new Command(SelectItemToAdd);
            _items = new ObservableCollection<ItemPreview>();
        }
        //private variables
        private bool _isRefreshing;
        private ItemPreview _selectedItem;
        private ICommand _addItem;
        private string _update;
        private ObservableCollection<ItemPreview> _items;
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
                        GetData().Await(HandleError,false,true,false);
                    }
                }
            }
        }
        //props for binding
        public ObservableCollection<ItemPreview> Items
        {
            get { return _items; }
            private protected set { _items = value; NotifyPropertyChanged(); }
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
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            private set { _isRefreshing = value; NotifyPropertyChanged(); }
        }
        public bool IsUwp
        {
            get { return Device.RuntimePlatform == Device.UWP; }
        }
        //commands
        public ICommand Refresh
        {
            get { return new Command(async () => 
            {
                IsRefreshing = true;
                try
                {
                    await RefreshPage();
                }
                catch(Exception ex)
                {
                    HandleError(ex);
                }
                IsRefreshing = false;
            }); }
        }
        public ICommand AddItem
        {
            get { return _addItem; }
        }
        //actions for commands
        private async void SelectItemToAdd()
        {
            if (Shell.Current != null)
                await Shell.Current.GoToAsync("ListItem");
        }
        private protected abstract Task RefreshPage();
        //abstract functions
        private protected abstract Task GetData();
        //methods
        private async Task ViewSelectedItem(int id, Enums.TypeOfItems itemType)
        {
            await Shell.Current.GoToAsync($"Create{itemType}?pageType=View&id={id}");
        }
        private protected void HandleError(Exception ex)
        {

        }
    }
}
