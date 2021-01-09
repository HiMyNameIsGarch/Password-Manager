using PassManager.Models.Items;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using PassManager.Models;
using System;
using PassManager.Enums;
using System.Collections.Generic;
using System.Linq;
using PassManager.Views.Popups;

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
            _items = new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>();
        }
        //private variables
        private bool _isRefreshing;
        private ItemPreview _selectedItem;
        private ICommand _addItem;
        private string _update;
        private bool _hasItems;
        private string _noItemsText;
        private ObservableCollection<Grouping<TypeOfItems, ItemPreview>> _items;
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
                        GetDataAsync().Await(HandleException,false,true,false);
                    }
                }
            }
        }
        //props for binding
        public ObservableCollection<Grouping<TypeOfItems, ItemPreview>> Items
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
        public string NoItemsText
        {
            get { return _noItemsText; }
            private set { _noItemsText = value; NotifyPropertyChanged(); }
        }
        public bool HasItems
        {
            get { return _hasItems; }
            private set { _hasItems = value; NotifyPropertyChanged(); }
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
        public ICommand Search
        {
            get
            {
                return new Command(async () => 
                {
                    await Shell.Current.GoToAsync("SearchItem");
                });
            }
        }
        public ICommand Refresh
        {
            get { return new Command(async () => 
            {
                await PageService.PushPopupAsync(new WaitForActionView(),false);
                IsRefreshing = true;
                try
                {
                    await RefreshPageAsync();
                }
                catch(Exception ex)
                {
                    HandleException(ex);
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
        private protected abstract Task RefreshPageAsync();
        //abstract functions
        private protected abstract Task GetDataAsync();
        //methods
        private protected void DisplayMsg(string text, bool hasItems)
        {
            NoItemsText = text;
            HasItems = hasItems;
        }
        private async Task ViewSelectedItem(int id, TypeOfItems itemType)
        {
            await Shell.Current.GoToAsync($"Create{itemType}?pageType=View&id={id}");
        }
        private protected bool IsListChanged(IEnumerable<Grouping<TypeOfItems, ItemPreview>> newList)
        {
            int previewsCount = newList.Count();
            int itemsCount = Items.Count();
            bool needUpdate = false;
            if (previewsCount > 0 && itemsCount > 0)//if all the lists have items in them do:
            {
                if (itemsCount != previewsCount)//if their length aren't the same set 'needUpdate' to true
                {
                    needUpdate = true;
                }
                else if (itemsCount == previewsCount)//if their length are the same do:
                {
                    for (int i = 0; i < itemsCount && !needUpdate; i++)//loop thru all the grouping lists
                    {
                        //convert the 2 grouping lists in normal lists
                        var itemsList = Items[i].ToList();
                        var previewsList = newList.ToList()[i].ToList();
                        for (int j = 0; j < itemsList.Count(); j++)//loop thru all the items in the lists and check if their are equals
                        {
                            if (!itemsList[j].Equals(previewsList[j]))
                            {
                                needUpdate = true;
                                break;
                            }
                        }
                    }
                }
            }
            return needUpdate;
        }
        private protected ObservableCollection<Grouping<TypeOfItems, ItemPreview>> UpdateItems(IEnumerable<Grouping<TypeOfItems, ItemPreview>> newList)
        {
            return new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>(newList);
        }
    }
}
