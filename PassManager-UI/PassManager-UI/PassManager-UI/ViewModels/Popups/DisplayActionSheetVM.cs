using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassManager.ViewModels.Popups
{
    public class DisplayActionSheetVM
    {
        public DisplayActionSheetVM(string title, string message, string cancel, IEnumerable<string> options)
        {
            //set values for page
            Title = title;
            Message = message;
            Cancel = cancel;
            //set some commands
            _selectOption = new Command(Select);
            CancelCommand = new Command(async () =>
            {
                await ClosePopUp(Cancel);
            });
            //add options
            Options = new List<Models.Option>();
            foreach (var option in options)
            {
                Options.Add(new Models.Option(option));
            }
        }
        //variables
        private ICommand _selectOption;
        private Models.Option _currentItem;
        public string ReturnValue;
        //props
        public ICollection<Models.Option> Options { get; set; }
        public Models.Option CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                SelectOption.Execute(_currentItem.Text);
            }
        }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Cancel { get; set; }
        //commands
        public ICommand SelectOption
        {
            get { return _selectOption; }
        }
        public ICommand CancelCommand { get; }
        //functions
        private async void Select(object obj)
        {
            string option = obj?.ToString();
            if (!string.IsNullOrEmpty(option))
            {
                await ClosePopUp(option);
            }
        }
        private async Task ClosePopUp(string value)
        {
            ReturnValue = value;
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
