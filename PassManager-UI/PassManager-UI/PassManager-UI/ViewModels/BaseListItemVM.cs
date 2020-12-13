using PassManager.Models.Items;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models.Interfaces;
using PassManager.Models;

namespace PassManager.ViewModels
{
    public abstract class BaseListItemVM : BaseViewModel
    {
        //constructors
        public BaseListItemVM(string pageTitle) : base(pageTitle)
        {
            _addItem = new Command(SelectItemToAdd);
            _refresh = new Command(RefreshPage);
        }
        //private variables
        private bool _isRefreshing;
        private ItemPreview _selectedItem;
        private ICommand _addItem;
        private ICommand _refresh;
        private ObservableCollection<ItemPreview> _passwords = new ObservableCollection<ItemPreview>();
        //props for binding
        public ObservableCollection<ItemPreview> Passwords
        {
            get { return _passwords; }
            private protected set { _passwords = value; }
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
            private protected set { _isRefreshing = value; NotifyPropertyChanged(); }
        }
        //commands
        public ICommand Refresh
        {
            get { return _refresh; }
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
        private protected abstract void RefreshPage();
        //methods
        private async System.Threading.Tasks.Task ViewSelectedItem(int id, Enums.TypeOfItems itemType)
        {
            await Shell.Current.GoToAsync($"Create{itemType}?pageType=View&id={id}");
        }
    }
}
