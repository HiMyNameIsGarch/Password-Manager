using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassManager.ViewModels.Popups
{
    public class DisplayAlertViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Accept { get; set; }
        public string Cancel { get; set; }
        public bool ReturnValue;
        public DisplayAlertViewModel(string title, string message, string accept, string cancel)
        {
            Title = title;
            Message = message;
            Accept = accept;
            Cancel = cancel;
        }
        private async Task ClosePopUp(bool value)
        {
            ReturnValue = value;
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        public ICommand YesCommand 
        {
            get => new Command(async () =>
            {
                await ClosePopUp(true);
            });
        }
        public ICommand CancelCommand 
        {
            get => new Command(async () =>
            {
                await ClosePopUp(false);
            });
        }
    }
}
