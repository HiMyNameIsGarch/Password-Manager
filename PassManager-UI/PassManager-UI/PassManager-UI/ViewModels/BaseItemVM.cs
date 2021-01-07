using PassManager.Enums;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PassManager.ViewModels
{
    [QueryProperty("PageType", "pageType")]
    [QueryProperty("Id", "id")]
    public abstract class BaseItemVM : BaseViewModel
    {
        //constructors
        public BaseItemVM(TypeOfItems itemType)
        {
            //set defaults values in case no parameter passed
            ChangeProps(ItemPageState.Null, "Create", "No data provided", true);
            ItemType = itemType.ToString();
            _save = new Command(ChangePageType);
            _displayMoreActions = new Command(DisplayMore);
            _deleteItem = new Command(AskToDeleteItemAsync);
        }
        //variables
        private readonly string ItemType;
        protected private ItemPageState PageState;
        private string _actionBtnText;
        private bool _readOnly;
        private ICommand _save;
        private ICommand _displayMoreActions;
        private ICommand _deleteItem;
        private bool _needMoreActions = false;
        private bool _canDelete;
        private string _actionsText = "More";
        private string _pageType;
        private string _id;
        //parameters
        public string Id
        {
            private protected get { return _id; }
            set
            {
                _id = Uri.UnescapeDataString(value ?? string.Empty);
                if (PageState != ItemPageState.Null)
                {
                    if (int.TryParse(Id, out int newId))
                    {
                        GetDataAsync(newId).Await(HandleException, true, true, false);
                    }
                    else
                    {
                        //handle error from id
                    }
                }
                else
                {
                    //handle error
                    PageTitle = "Your item is invalid!";
                }
            }
        }
        public string PageType
        {
            private protected get { return _pageType; }
            set
            {
                _pageType = Uri.UnescapeDataString(value ?? string.Empty);
                Enum.TryParse(_pageType, out ItemPageState pageState);
                PageState = pageState;
                switch (PageState)
                {
                    case ItemPageState.Create:
                        PageTitle = $"Create {ItemType}!";
                        ReadOnly = false;
                        break;
                    case ItemPageState.View:
                        ChangeProps(ItemPageState.View, "Edit", $"View {ItemType}", true);
                        break;
                    case ItemPageState.Edit:
                        PageTitle = $"Edit {ItemType}";
                        break;
                    default:
                        PageTitle = $"Your {ItemType} is invalid!";
                        break;
                }
            }
        }
        //props
        public bool CanDelete
        {
            get { return _canDelete; }
            private set { _canDelete = value; NotifyPropertyChanged(); }
        }
        public string ActionsText
        {
            get { return _actionsText; }
            private set { _actionsText = value; NotifyPropertyChanged(); }
        }
        public bool NeedMoreActions
        {
            get { return _needMoreActions; }
            private set { _needMoreActions = value; NotifyPropertyChanged(); }
        }
        public bool IsUwp
        {
            get { return Device.RuntimePlatform == Device.UWP; }
        }
        public string ActionBtnText
        {
            get { return _actionBtnText; }
            protected private set { _actionBtnText = value; NotifyPropertyChanged(); }
        }
        public bool ReadOnly
        {
            get { return _readOnly; }
            protected private set { _readOnly = value; NotifyPropertyChanged(); }
        }
        //commands
        public ICommand DeleteItem
        {
            get { return _deleteItem; }
        }
        public ICommand SaveChanges {
            get { return _save; }
        }
        public ICommand DisplayMoreActions
        {
            get { return _displayMoreActions; }
        }
        //functions for commands
        private async void AskToDeleteItemAsync()
        {
            bool accept = await PageService.DisplayAlert("Delete","Do you really want to delete this item?","Yes","No");
            if (accept)
            {
                await PageService.PushPopupAsync(new Views.Popups.WaitForActionView());
                try
                {
                    await Delete();
                }
                catch(Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
        private void DisplayMore()
        {
            if (NeedMoreActions)
            {
                NeedMoreActions = false;
                ActionsText = "More";
            }
            else
            {
                NeedMoreActions = true;
                ActionsText = "Hide";
            }
        }
        private async void ChangePageType()
        {
            //open popup
            if(PageState == ItemPageState.Create || PageState == ItemPageState.Edit)
            {
                await PageService.PushPopupAsync(new Views.Popups.WaitForActionView(),false); 
            }
            switch (PageState)
            {
                case ItemPageState.Create:
                    if (await IsModelValid())
                    {
                        Create().Await(HandleException);
                    }
                    break;
                case ItemPageState.View:
                    ChangeProps(ItemPageState.Edit, "Save", $"Edit {ItemType}", false);
                    break;
                case ItemPageState.Edit:
                    if (await IsModelValid())
                    {
                        Modify().Await(HandleException);
                    }
                    break;
            }
        }
        //basic actions for item page
        private protected abstract Task Create();
        private protected abstract Task Delete();
        private protected abstract Task Modify();
        private protected abstract Task<bool> IsModelValid();//this function will check if the item is valid(title not to be more than 25 char etc etc) other wise will display a popup with the info
        //functions
        private protected abstract Task GetDataAsync(int id);
        private protected void ChangeProps(ItemPageState pageState, string btnText, string pageTitle, bool isReadOnly)
        {
            if(pageState == ItemPageState.View || pageState == ItemPageState.Edit)
            {
                CanDelete = true;
            }
            else
            {
                CanDelete = false;
            }
            PageState = pageState;
            ActionBtnText = btnText;
            PageTitle = pageTitle;
            ReadOnly = isReadOnly;
        }
        private protected async Task GoTo(string itemPage, string parameters = "")
        {
            string location = Shell.Current.CurrentState.Location.ToString();
            location = location.Replace("//", "");
            string[] path = location.Split('/');
            int navigationMax = Shell.Current.Navigation.NavigationStack.Count() - 1;
            string baseRoute = string.Empty;
            for (int i = 0; i < navigationMax; i++)
            {
                if (i == 0)
                    baseRoute += "..";
                else
                    baseRoute += "/..";
            }
            if(path[0] == "EntireItems")
            {
                baseRoute += "?update=true";
            }
            else
            {    
                baseRoute += parameters;
            }
            await Shell.Current.GoToAsync(baseRoute, false);
            if (!path[path.Length - 1].Contains(path[0]))
            {
                await Shell.Current.GoToAsync($"///{itemPage}{parameters}", false);
            }
            if (IsUwp)
            {
                await Task.WhenAll(Shell.Current.GoToAsync("///EntireItems", false), Shell.Current.GoToAsync($"///{itemPage}", false));
            }
        }
    }
}
