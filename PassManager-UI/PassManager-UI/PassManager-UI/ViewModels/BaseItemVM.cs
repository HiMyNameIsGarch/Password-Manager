using PassManager.Enums;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PassManager.ViewModels
{
    public abstract class BaseItemVM : BaseViewModel
    {
        //constructors
        public BaseItemVM()
        {
            _save = new Command(ChangePageType);
        }
        //variables
        protected private ItemPageState PageState;
        private string _actionBtnText;
        private bool _readOnly;
        private ICommand _save;
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
        //commands
        public ICommand SaveChanges {
            get { return _save; }
        }
        //functions for commands
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
        private protected void ChangeProps(ItemPageState pageState, string btnText, string pageTitle, bool isReadOnly)
        {
            PageState = pageState;
            ActionBtnText = btnText;
            PageTitle = pageTitle;
            ReadOnly = isReadOnly;
        }
        private protected async Task GoTo(string itemPage)
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
            await Shell.Current.GoToAsync(baseRoute, false);
            if (!path[path.Length - 1].Contains(path[0]))
            {
                await Shell.Current.GoToAsync($"///{itemPage}", false);
            }
            if (IsUwp)
            {
                await Task.WhenAll(Shell.Current.GoToAsync("///EntireItems", false), Shell.Current.GoToAsync($"///{itemPage}", false));
            }
        }
        private void HandleError(Exception ex)
        {

        }
    }
}
