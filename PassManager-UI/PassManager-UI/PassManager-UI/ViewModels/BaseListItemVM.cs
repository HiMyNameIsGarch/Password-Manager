using PassManager.Models.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using PassManager.Models.Interfaces;

namespace PassManager.ViewModels
{
    public class BaseListItemVM : BaseViewModel
    {
        public BaseListItemVM(IPageService pageService) : base(pageService)
        {
            _selectItem = new Command(SelectItemToAdd);
        }
        private ICommand _selectItem;
        private ObservableCollection<ItemPreview> _passwords;
        public ObservableCollection<ItemPreview> Passwords
        {
            get { return _passwords; }
            private protected set { _passwords = value; }
        }
        public ICommand SelectItem
        {
            get { return _selectItem; }
        }
        private async void SelectItemToAdd()
        {
            if (Shell.Current != null)
                await Shell.Current.GoToAsync("ListItem");
        }
    }
}
