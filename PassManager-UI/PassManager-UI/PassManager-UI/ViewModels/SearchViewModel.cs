using Newtonsoft.Json;
using PassManager.Enums;
using PassManager.Models;
using PassManager.Models.Api;
using PassManager.Models.Items;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using PassManager.ViewModels.Bases;
using PassManager.Models.Api.Processors;

namespace PassManager.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel() : base("Search...")
        {
            _items = new ObservableCollection<Grouping<string, ItemPreview>>();
        }
        private ItemPreview _selectedItem;
        private string _searchString;
        private int _maxLength = int.MaxValue;
        private Timer _timer;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                //set the value for search string
                _searchString = value;
                NotifyPropertyChanged();

                //when the items is changed, if the times is not null, dispose(reset)
                if (_timer != null)
                    _timer.Dispose();
                //create a new timer, after 500 miliseconds the functions will be called
                _timer = new Timer(OnTimerElapsed, null, 500, 0);
            }
        }
        private async void OnTimerElapsed(object sender)
        {
            if (!string.IsNullOrEmpty(_searchString) && _searchString.Length < _maxLength)
            {
                try
                {
                    await DisplayItems(_searchString);
                }
                catch(Exception ex)
                {
                    HandleException(ex);
                }
                _searchString = null;
            }
            else
            {
                _maxLength = int.MaxValue;
                Items = null;
            }
            _timer.Dispose();
        }
        private async Task DisplayItems(string searchString)
        {
            if (IsInternet())
            {
                var items = await EntireItemsProcessor.GetPreviews(ApiHelper.ApiClient, searchString.ToLower());
                if(items.Count() > 0)
                {
                    Items = new ObservableCollection<Grouping<string, ItemPreview>>(items);
                }
                else
                {
                    _maxLength = searchString.Length;
                }
            }
        }
        private ObservableCollection<Grouping<string, ItemPreview>> _items;
        public ObservableCollection<Grouping<string, ItemPreview>> Items
        {
            get { return _items; }
            private set { _items = value; NotifyPropertyChanged(); }
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
            if (IsInternet())
            {
                //create object
                CreatePage pageToCreate = new CreatePage(ItemPageState.View, id);
                //serialize it
                string pageToCreateString = JsonConvert.SerializeObject(pageToCreate);
                //send it
                await Shell.Current.GoToAsync($"Create{itemType.ToSampleString()}?createPage={pageToCreateString}");
            }
        }
    }
}
