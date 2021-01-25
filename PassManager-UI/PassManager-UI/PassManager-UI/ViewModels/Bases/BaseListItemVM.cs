﻿using PassManager.Models.Items;
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
using Newtonsoft.Json;
using PassManager.Models.Api;
using PassManager.Models.Api.Processors;

namespace PassManager.ViewModels.Bases
{
    [QueryProperty("Update", "update")]
    public abstract class BaseListItemVM : BaseViewModel
    {
        //constructors
        public BaseListItemVM(string pageTitle) : base(pageTitle)
        {
            _addItem = new Command(SelectItemToAdd);
            _items = new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>();
            _refresh = new Command(RefreshCommand);
        }
        //private variables
        private bool _isRefreshing;
        private ItemPreview _selectedItem;
        private ICommand _addItem;
        private ICommand _refresh;
        private string _update;
        private bool _hasItems;
        private string _noItemsText;
        private ObservableCollection<Grouping<TypeOfItems, ItemPreview>> _items;
        private UpdateModel _updateModel;
        //parameters
        public string Update
        {
            get { return _update; }
            set
            {
                _update = Uri.UnescapeDataString(value ?? string.Empty);
                _updateModel = JsonConvert.DeserializeObject<UpdateModel>(_update);
                if(_updateModel != null)
                {
                    bool hasItems = Items.Count() > 0;
                    switch (_updateModel.UpdateType)
                    {
                        case TypeOfUpdates.Null:
                            //log
                            break;
                        case TypeOfUpdates.Create:
                            UpdateItems(_updateModel.UpdateType).Await(HandleException, false, true, false);
                            break;
                        case TypeOfUpdates.Modify:
                            if (hasItems)
                                UpdateItems(_updateModel.UpdateType).Await(HandleException, false, true, false);
                            break;
                        case TypeOfUpdates.Delete:
                            if (hasItems)
                                Delete(_updateModel.IdToDelete);
                            break;
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
            get { return _refresh; }
        }
        public ICommand AddItem
        {
            get { return _addItem; }
        }
        //actions for commands
        private async void RefreshCommand()
        {
            if (IsInternet())
            {
                IEnumerable<Grouping<TypeOfItems, ItemPreview>> items = Enumerable.Empty<Grouping<TypeOfItems, ItemPreview>>();
                await PageService.PushPopupAsync(new WaitForActionView(), false);
                IsRefreshing = true;
                try
                {
                    items = await RefreshPageAsync();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
                IsRefreshing = false;

                await PageService.PopAllAsync(false);
                if (IsListChanged(items))
                {
                    Items = UpdateItems(items);
                }
                else
                {
                    await PageService.PushPopupAsync(new WarningView("Your items are up to date!"));
                }
            }
            else
            {
                IsRefreshing = false;
            }
        }
        private async void SelectItemToAdd()
        {
            if (Shell.Current != null)
                await Shell.Current.GoToAsync("ListItem");
        }
        private protected abstract Task<IEnumerable<Grouping<TypeOfItems, ItemPreview>>> RefreshPageAsync();
        //abstract functions
        private async Task UpdateItems(TypeOfUpdates updateType)
        {
            if (IsInternet())
            {
                ItemPreview newItem = await EntireItemsProcessor.GetUpdate(ApiHelper.ApiClient, updateType);
                var currentItems = Items.Where(s => s.Key == newItem.ItemType).FirstOrDefault();
                if(currentItems is null)
                {
                    DisplayMsg("",false);
                    Items.Add(new Grouping<TypeOfItems, ItemPreview>(newItem.ItemType, new List<ItemPreview>() { newItem }));
                }
                else if (updateType == TypeOfUpdates.Create)
                {
                    currentItems.Add(newItem);
                }
                else if(updateType == TypeOfUpdates.Modify)
                {
                    var itemToBeModified = currentItems.FirstOrDefault(s => s.Id == newItem.Id);
                    if(itemToBeModified != null)
                    {
                        int index = currentItems.IndexOf(itemToBeModified);
                        currentItems.SetNewItem(index, newItem);
                    }
                }
            }
        }
        private void Delete(int id)
        {
            foreach (var item in Items)
            {
                var itemToDelete = item.Where(s => s.Id == id).FirstOrDefault();
                if(itemToDelete != null)
                {
                    item.Remove(itemToDelete);
                    break;
                }
            }
            if(Items.Count == 1 && Items.FirstOrDefault().Count == 0)
            {
                Items.Clear();
                DisplayMsg("You have no passwords yet, click on button below to add a new one!", true);
            }
        }
        private protected abstract Task GetDataAsync();
        //methods
        private protected void DisplayMsg(string text, bool hasItems)
        {
            NoItemsText = text;
            HasItems = hasItems;
        }
        private async Task ViewSelectedItem(int id, TypeOfItems itemType)
        {
            if (IsInternet())
            {
                //create object
                CreatePage pageToCreate = new CreatePage(ItemPageState.View, id);
                //serialize it
                string pageToCreateString = JsonConvert.SerializeObject(pageToCreate);
                //send it
                await Shell.Current.GoToAsync($"Create{itemType}?createPage={pageToCreateString}");
            }
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
        private protected void DisplayItems(IEnumerable<Grouping<TypeOfItems, ItemPreview>> itemsToDisplay)
        {
            if (itemsToDisplay != null && itemsToDisplay.Count() > 0)
            {
                DisplayMsg(string.Empty, false);
                Items = new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>(itemsToDisplay);
            }
            else if (itemsToDisplay.Count() == 0)
            {
                DisplayMsg($"You have no items yet, click on button below to add a new one!", true);
            }
        }
        private protected ObservableCollection<Grouping<TypeOfItems, ItemPreview>> UpdateItems(IEnumerable<Grouping<TypeOfItems, ItemPreview>> newList)
        {
            return new ObservableCollection<Grouping<TypeOfItems, ItemPreview>>(newList);
        }
    }
}