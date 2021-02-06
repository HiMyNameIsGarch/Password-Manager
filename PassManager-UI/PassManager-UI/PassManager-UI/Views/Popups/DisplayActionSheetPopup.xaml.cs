using PassManager.ViewModels.Popups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayActionSheetPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<string> _taskCompletionSource;
        public Task<string> PopupClosedTask => _taskCompletionSource.Task;
        public DisplayActionSheetPopup(string title, string message, string cancel, IEnumerable<string> options)
        {
            InitializeComponent();
            BindingContext = new DisplayActionSheetVM(title, message, cancel, options);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _taskCompletionSource = new TaskCompletionSource<string>();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(((DisplayActionSheetVM)BindingContext).ReturnValue);
        }
    }
}