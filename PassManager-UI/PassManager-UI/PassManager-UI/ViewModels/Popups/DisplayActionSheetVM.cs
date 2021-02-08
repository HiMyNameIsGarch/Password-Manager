using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassManager.ViewModels.Popups
{
    public class DisplayActionSheetVM
    {
        public DisplayActionSheetVM(string title, string cancel, IEnumerable<string> options, ScrollView scrollView)
        {
            //set values for page
            Title = title;
            Cancel = cancel;
            //set some commands
            _selectOption = new Command(Select);
            CancelCommand = new Command(async () =>
            {
                await ClosePopUp(Cancel);
            });
            //set font size for button
            int fontSize = 0;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    fontSize = 15;
                    break;
                case Device.UWP:
                    fontSize = 17;
                    break;
            }
            //add options
            StackLayout st = new StackLayout();
            foreach (var option in options)
            {
                var btn = new Button()
                {
                    Text = option,
                    TextColor = Color.FromHex("A8DADC"),
                    FontSize = fontSize,
                    BackgroundColor = Color.Transparent,
                    Command = SelectOption,
                    CommandParameter = option
                };
                st.Children.Add(btn);
            }
            scrollView.Content = st;
        }
        //variables
        private ICommand _selectOption;
        public string ReturnValue;
        //props
        public string Title { get; set; }
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
