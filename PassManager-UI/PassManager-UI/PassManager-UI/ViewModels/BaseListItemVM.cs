using PassManager.Models.Items;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using PassManager.Models;
using System;

namespace PassManager.ViewModels
{
    public abstract class BaseListItemVM : BaseViewModel
    {
        //constructors
        public BaseListItemVM(string pageTitle) : base(pageTitle)
        {
            _addItem = new Command(SelectItemToAdd);
        }
        //private variables
        private bool _isRefreshing;
        private ItemPreview _selectedItem;
        private ICommand _addItem;
        private ObservableCollection<ItemPreview> _passwords = new ObservableCollection<ItemPreview>();
        //props for binding
        public ObservableCollection<ItemPreview> Passwords
        {
            get { return _passwords; }
            private protected set { _passwords = value; NotifyPropertyChanged(); }
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
        //methods
        private async Task ViewSelectedItem(int id, Enums.TypeOfItems itemType)
        {
            await Shell.Current.GoToAsync($"Create{itemType}?pageType=View&id={id}");
        }
        private void HandleError(Exception ex)
        {

        }
    }
}
