using PassManager.Models.Items;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models.Interfaces;
using PassManager.Models;

namespace PassManager.ViewModels
{
    public class BaseListItemVM : BaseViewModel
    {
        public BaseListItemVM(IPageService pageService) : base(pageService)
        {
            _addItem = new Command(SelectItemToAdd);
        }
        public BaseListItemVM(IPageService pageService, string pageTitle) : base(pageService,pageTitle)
        {
            _addItem = new Command(SelectItemToAdd);
        }
        private ItemPreview _selectedItem;
        private ICommand _addItem;
        private ObservableCollection<ItemPreview> _passwords = new ObservableCollection<ItemPreview>();
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
        public ICommand AddItem
        {
            get { return _addItem; }
        }
        private async System.Threading.Tasks.Task ViewSelectedItem(int id, Enums.TypeOfItems itemType)
        {
            await Shell.Current.GoToAsync($"Create{itemType}?pageType=View&id={id}");
        }
        private async void SelectItemToAdd()
        {
            if (Shell.Current != null)
                await Shell.Current.GoToAsync("ListItem");
        }
    }
}
