using PassManager.Enums;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using PassManager.Views.Popups;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;

namespace PassManager.ViewModels.Bases
{
    [QueryProperty("CreatePage", "createPage")]
    public abstract class BaseItemVM : BaseViewModel
    {
        //constructors
        public BaseItemVM(TypeOfItems itemType)
        {
            //set defaults values in case no parameter passed
            ChangeProps(ItemPageState.Null, "Save", "No data provided", true);
            ItemType = itemType.ToSampleString();
            _save = new Command(ChangePageType);
            _displayMoreActions = new Command(DisplayMore);
            _deleteItem = new Command(AskToDeleteItemAsync);
        }
        //variables
        private readonly string ItemType;
        protected private ItemPageState PageState;
        private string _actionBtnText;
        private bool _readOnly;
        private bool _canCopy;
        private ICommand _save;
        private ICommand _displayMoreActions;
        private ICommand _deleteItem;
        private bool _needMoreActions = false;
        private bool _canDelete;
        private string _actionsText = "More";
        private int _itemId;
        private string _createPage;
        //parameters
        public string CreatePage
        {
            private get { return _createPage; }
            set
            {
                _createPage = Uri.UnescapeDataString(value ?? string.Empty);
                var page = JsonConvert.DeserializeObject<CreatePage>(_createPage);
                if(page.IdForItem != 0 && page.PageState != ItemPageState.Null)
                {
                    PageState = page.PageState;
                    _itemId = page.IdForItem;
                    //change page based on state
                    switch (PageState)
                    {
                        case ItemPageState.Create:
                            PageTitle = $"Add {ItemType}";
                            ReadOnly = false;
                            break;
                        case ItemPageState.View:
                            if(_itemId > 0)//check if id is valid
                            {
                                //get data from api
                                GetDataAsync(_itemId).Await(HandleException, true, true, false);
                            }
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
                else
                {
                    ChangeProps(ItemPageState.Null, "Error","Something went wrong!", true);
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
        public bool CanCopy
        {
            get { return _canCopy; }
            private set { _canCopy = value; NotifyPropertyChanged(); }
        }
        //commands
        public ICommand GeneratePassword
        {
            get
            {
                return new Command(async () => 
                {
                    await PageService.PushPopupAsync(new PasswordGeneratorView());
                });
            }
        }
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
                if (IsInternet())
                {
                    await PageService.PushPopupAsync(new WaitForActionView());
                    try
                    {
                        await Delete();
                    }
                    catch(Exception ex)
                    {
                        HandleException(ex);
                    }
                    await PageService.PopPopupAsync(false);
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
            if (IsInternet())
            {
                //open popup
                if(PageState == ItemPageState.Create || PageState == ItemPageState.Edit)
                {
                    await PageService.PushPopupAsync(new WaitForActionView(),false); 
                }
                switch (PageState)
                {
                    case ItemPageState.Create:
                        var createStatus = IsModelValid();
                        if (!createStatus.IsError)
                            Create().Await(HandleException, false, true, false);
                        else
                            await DisplayPopupError(createStatus.Message);
                        break;
                    case ItemPageState.View:
                        ChangeProps(ItemPageState.Edit, "Save", $"Edit {ItemType}", false);
                        break;
                    case ItemPageState.Edit:
                        var editStatus = IsModelValid();
                        if (!editStatus.IsError)
                            Modify(_itemId).Await(HandleException, false, true, false);
                        else 
                            await DisplayPopupError(editStatus.Message);
                        break;
                }
            }
        }
        //basic actions for item page
        private protected abstract Task Create();
        private protected abstract Task Delete();
        private protected abstract Task Modify(int id);
        private protected abstract Task GetDataAsync(int id);
        private protected abstract Models.TaskStatus IsModelValid();//this function will check if the item is valid(title not to be more than 25 char etc etc) other wise will display a popup with the info
        private protected abstract object EncryptItem(object obj);//gen an object(an item for that particular page) and return the encrypted version
        private protected abstract object DecryptItem(object obj);//get an object(an item for that particular page) and return the decrypted version
        //functions
        private async Task DisplayPopupError(string msg)
        {
            //pop all popups
            await PopupNavigation.Instance.PopAllAsync();
            //push main
            await PageService.PushPopupAsync(new ErrorView(msg));
        }
        private protected void ChangeProps(ItemPageState pageState, string btnText, string pageTitle, bool isReadOnly)
        {
            if(pageState == ItemPageState.View)
            {
                CanCopy = CanDelete = true;
            }
            else if(pageState == ItemPageState.Edit)
            {
                CanCopy = false;
                CanDelete = true;
            }
            else
            {
                CanCopy = CanDelete = false;
            }
            PageState = pageState;
            ActionBtnText = btnText;
            PageTitle = pageTitle;
            ReadOnly = isReadOnly;
        }
        private protected async Task GoTo(string itemPage, UpdateModel updateModel = null)
        {
            //construct parameters based on model
            string parameters = updateModel != null ? JsonConvert.SerializeObject(updateModel) : "";
            //get current location
            string location = Shell.Current.CurrentState.Location.ToString();
            location = location.Replace("//", "");
            string[] path = location.Split('/');
            if (path[0] == "EntireItems")
            {
                await Shell.Current.GoToAsync($"///EntireItems?update={parameters}", false);
                await Shell.Current.GoToAsync($"///{itemPage}?update={parameters}", false);
            }
            else
            {
                await Task.WhenAll(Shell.Current.GoToAsync($"///{itemPage}?update={parameters}", false),
                    Shell.Current.GoToAsync($"///EntireItems?update={parameters}", false),
                    Shell.Current.GoToAsync($"///{itemPage}", false));
            }
            if (IsUwp)
            {
                await Task.WhenAll(Shell.Current.GoToAsync("///EntireItems", false), Shell.Current.GoToAsync($"///{itemPage}", false));
            }
        }
        private protected async Task CopyToClipboard(string textToCopy)
        {
            if (!string.IsNullOrEmpty(textToCopy))
            {
                string clipboardText = await Clipboard.GetTextAsync() ?? "";
                if (textToCopy != clipboardText)
                    await Clipboard.SetTextAsync(textToCopy);
            }
        }
    }
}
