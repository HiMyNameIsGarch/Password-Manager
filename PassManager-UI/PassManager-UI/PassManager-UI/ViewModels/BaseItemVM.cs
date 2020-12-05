using PassManager.Enums;
using System.Windows.Input;
using Xamarin.Forms;

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
        public string ActionBtnText
        {
            get { return _actionBtnText; }
            protected private set { _actionBtnText = value; NotifyPropertyChanged(); }
        }
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; NotifyPropertyChanged(); }
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
                    Create();
                    ChangeProps(ItemPageState.View, "Edit", "View item", true);
                    break;
                case ItemPageState.View:
                    ChangeProps(ItemPageState.Edit, "Save", "Edit the item", false);
                    break;
                case ItemPageState.Edit:
                    Modify();
                    ChangeProps(ItemPageState.View, "Edit", "View item", true);
                    break;
            }
        }
        //basic actions for item page
        private protected abstract void Create();
        private protected abstract void Delete();
        private protected abstract void Modify();
        private protected void ChangeProps(ItemPageState pageState, string btnText, string pageTitle, bool isReadOnly)
        {
            PageState = pageState;
            ActionBtnText = btnText;
            PageTitle = pageTitle;
            ReadOnly = isReadOnly;
        }
    }
}
