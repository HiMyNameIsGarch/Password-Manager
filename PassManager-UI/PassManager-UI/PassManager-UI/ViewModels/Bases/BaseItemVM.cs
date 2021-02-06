using PassManager.Enums;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using PassManager.Views.Popups;
using Xamarin.Essentials;
using PassManager.Models.Interfaces;
using PassManager.Models.Items;
using Rg.Plugins.Popup.Services;
using System.Linq;
using PassManager.Models.Api.Processors;
using PassManager.Models.Api;
using PassManager.Models.CallStatus;
using System.Collections;
using System.Collections.Generic;

namespace PassManager.ViewModels.Bases
{
    [QueryProperty("CreatePage", "createPage")]
    public abstract class BaseItemVM : BaseViewModel, IBackButtonBehavior
    {
        //constructors
        public BaseItemVM(TypeOfItems itemType)
        {
            //set defaults values in case no parameter passed
            ChangeProps(ItemPageState.Null, "Save", "No data provided", true);
            ItemType = itemType;
            _goBack = new Command(GoBackButton);
            _save = new Command(ChangePageType);
            _displayMoreActions = new Command(DisplayMore);
        }
        //variables
        private readonly TypeOfItems ItemType;
        private ItemPageState PageState;
        private bool CanDelete;
        private string _actionBtnText;
        private bool _readOnly;
        private bool _canCopy;
        private ICommand _save;
        private ICommand _goBack;
        private ICommand _displayMoreActions;
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
                            PageTitle = $"Add {ItemType.ToSampleString()}";
                            ReadOnly = false;
                            break;
                        case ItemPageState.View:
                            if(_itemId > 0 && IsInternet())//check if id is valid
                            {
                                //get data from api
                                GetDataAsync(_itemId).AwaitWithPopup(HandleException, false);
                            }
                            ChangeProps(ItemPageState.View, "Edit", $"View {ItemType.ToSampleString()}", true);
                            break;
                        case ItemPageState.Edit:
                            PageTitle = $"Edit {ItemType.ToSampleString()}";
                            break;
                        default:
                            PageTitle = $"Your {ItemType.ToSampleString()} is invalid!";
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
        public ICommand DisplayMoreActions
        {
            get { return _displayMoreActions; }
        }
        public ICommand SaveChanges {
            get { return _save; }
        }
        public ICommand GoBack
        {
            get { return _goBack; } 
        }

        //functions for commands
        public async void GoBackButton()
        {
            if (IsItemChanged())
            {
                bool wantsToLeave = await PageService.DisplayAlert("Wait!", "Are you sure you want to leave?", "Yes", "No");
                if (wantsToLeave)
                    await Shell.Current.Navigation.PopToRootAsync();
            }
            else
                await Shell.Current.Navigation.PopToRootAsync();
        }
        private async Task AskToDeleteItemAsync()
        {
            bool accept = await PageService.DisplayAlert("Delete","Do you really want to delete this item?","Yes","No");
            if (accept)
            {
                if (IsInternet())
                {
                    await PageService.PushPopupAsync(new WaitForActionView());
                    try
                    {
                        var status = await DeleteAsync();
                        if (status.IsCallSucces)
                        {
                            UpdateModel model = new UpdateModel(TypeOfUpdates.Delete, new ItemPreview() { Id = status.Id, ItemType = ItemType});
                            await GoTo(ItemType.ToString(), model);
                        }
                        else
                            await PageService.PushPopupAsync(new ErrorView($"Something went wrong and your {ItemType.ToSampleString()} has not been deleted, try again!"));
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                    if(!PopupNavigation.Instance.PopupStack.Where(s => s.ToString().Contains("Error")).Any())
                        await PageService.PopPopupAsync(false);
                }
            }
        }
        private async void DisplayMore()
        {
            ICollection<string> options = new List<string>(){"Generate Password" };
            if (CanDelete)
                options.Add("Delete Item");
            var response = await PageService.DisplayActionSheet("What do you want to do?","Cancel",null, options);
            switch (response)
            {
                case "Generate Password":
                    await PageService.PushPopupAsync(new PasswordGeneratorView());
                    break;
                case "Delete Item":
                    if (CanDelete)
                        await AskToDeleteItemAsync();
                    break;
            }
        }
        private async void ChangePageType()
        {
            if (IsInternet())
            {
                switch (PageState)
                {
                    case ItemPageState.Create:
                        //handle creation of item
                        var createStatus = IsModelValid();
                        if (createStatus.IsError)
                        {
                            await DisplayPopupError(createStatus.Message);
                            break;
                        }
                        await PageService.PopAllAsync();
                        await PageService.PushPopupAsync(new WaitForActionView(), false);
                        bool isCallSucces = false;
                        try
                        {
                            isCallSucces = await CreateAsync();
                        }
                        catch (Exception ex)
                        {
                            HandleException(ex);
                            break;
                        }
                        await PageService.PopPopupAsync(false);
                        if (isCallSucces)
                        {
                            var latestCreatedItem = await EntireItemsProcessor.GetLatestCreated(ApiHelper.ApiClient, ItemType);
                            if (latestCreatedItem is null)
                            {
                                await PageService.PushPopupAsync(new ErrorView($"Something went wrong and your {ItemType.ToSampleString()} has not been created, try again!"));
                            }
                            else
                            {
                                UpdateModel model = new UpdateModel(TypeOfUpdates.Create, latestCreatedItem);
                                await GoTo(ItemType.ToString(), model);
                            }
                        }
                        else
                            await PageService.PushPopupAsync(new ErrorView($"Something went wrong and your {ItemType.ToSampleString()} has not been created, try again!"));
                        break;
                    case ItemPageState.View:
                        ChangeProps(ItemPageState.Edit, "Save", $"Edit {ItemType.ToSampleString()}", false);
                        break;
                    case ItemPageState.Edit:
                        //handle editing of an item
                        var editStatus = IsModelValid();
                        if (editStatus.IsError)
                        {
                            await DisplayPopupError(editStatus.Message);
                            break;
                        }
                        if (!IsItemChanged())
                        {
                            await Shell.Current.Navigation.PopToRootAsync();
                            break;
                        }
                        await PageService.PopAllAsync();
                        await PageService.PushPopupAsync(new WaitForActionView(), false);
                        ModifyCallStatus status = null;
                        try
                        {
                            status = await ModifyAsync(_itemId);
                        }
                        catch (Exception ex)
                        {
                            HandleException(ex);
                            break;
                        }
                        await PageService.PopPopupAsync(false);

                        if (status.IsCallSucces)
                        {
                            if (status.ItemPreviewChanged)
                            {
                                UpdateModel model = new UpdateModel(TypeOfUpdates.Modify, status.ItemPreview);
                                await GoTo(ItemType.ToString(), model);
                            }
                            else
                                await GoTo(ItemType.ToString());
                        }
                        else
                            await PageService.PushPopupAsync(new ErrorView($"Something went wrong and your {ItemType.ToSampleString()} has not been modified, try again!"));
                        break;
                }
            }
        }
        //basic actions for item page
        private protected abstract bool IsItemChanged();
        private protected abstract Models.TaskStatus IsModelValid();//this function will check if the item is valid(title not to be more than 25 char etc etc) other wise will display a popup with the info
        private protected abstract Task GetDataAsync(int id);
        private protected abstract Task<bool> CreateAsync();
        private protected abstract Task<ModifyCallStatus> ModifyAsync(int id);
        private protected abstract Task<DeleteCallStatus> DeleteAsync();
        private protected abstract object EncryptItem(object obj);//gen an object(an item for that particular page) and return the encrypted version
        private protected abstract object DecryptItem(object obj);//get an object(an item for that particular page) and return the decrypted version
        //functions
        private async Task DisplayPopupError(string msg)
        {
            //pop all popups
            await PageService.PopAllAsync();
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
