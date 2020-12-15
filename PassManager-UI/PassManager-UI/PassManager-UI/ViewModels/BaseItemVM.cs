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
        public BaseItemVM(string title = "") : base(title)
        {
            //set defaults values in case no parameter passed
            ChangeProps(ItemPageState.Null, "Create", "No data provided", true);
            _save = new Command(ChangePageType);
            _displayMoreActions = new Command(DisplayMore);
            _deleteItem = new Command(AskToDeleteItem);
        }
        //variables
        protected private ItemPageState PageState;
        private string _actionBtnText;
        private bool _readOnly;
        private ICommand _save;
        private ICommand _displayMoreActions;
        private ICommand _deleteItem;
        private bool _needMoreActions = false;
        private string _actionsText = "More";
        private string _pageType;
        private string _id;
        //props
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
        public string Id
        {
            private protected get { return _id; }
            set
            {
                _id = Uri.UnescapeDataString(value ?? string.Empty);
                AfterSettingId();
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
                AfterSettingPageType();
            }
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
        private async void AskToDeleteItem()
        {
            bool accept = await PageService.DisplayAlert("Delete","Do you really want to delete this item?","Yes","No");
            if (accept)
            {
                try
                {
                    await Delete();
                }
                catch(Exception ex)
                {
                    HandleError(ex);
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
        private void ChangePageType()
        {
            switch (PageState)
            {
                case ItemPageState.Create:
                    Create().Await(HandleError);
                    break;
                case ItemPageState.View:
                    ChangeProps(ItemPageState.Edit, "Save", "Edit the item", false);
                    break;
                case ItemPageState.Edit:
                    Modify().Await(HandleError);
                    break;
            }
        }
        //basic actions for item page
        private protected abstract Task Create();
        private protected abstract Task Delete();
        private protected abstract Task Modify();
        //functions
        private protected abstract void AfterSettingId();
        private protected abstract void AfterSettingPageType();
        private protected void ChangeProps(ItemPageState pageState, string btnText, string pageTitle, bool isReadOnly)
        {
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
            baseRoute += parameters;
            await Shell.Current.GoToAsync(baseRoute, false);
            if (!path[path.Length - 1].Contains(path[0]))
            {
                await Shell.Current.GoToAsync($"///{itemPage}{parameters}", false);
            }
            if (IsUwp)
            {
                await Task.WhenAll(Shell.Current.GoToAsync("///EntireItems", false), Shell.Current.GoToAsync($"///{itemPage}{parameters}", false));
            }
        }
        private void HandleError(Exception ex)
        {

        }
    }
}
