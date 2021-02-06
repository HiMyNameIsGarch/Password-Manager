using PassManager.ViewModels.Popups;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayActionSheetView : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
        public Task<bool> PopupClosedTask => _taskCompletionSource.Task;
        public DisplayActionSheetView(string title, string message, string accept, string cancel)
        {
            InitializeComponent();
            BindingContext = new DisplayAlertViewModel(title, message, accept, cancel);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _taskCompletionSource = new TaskCompletionSource<bool>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(((DisplayAlertViewModel)BindingContext).ReturnValue);
        }
    }
}