using PassManager.ViewModels.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorView : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ErrorView(string errorMsg, bool canQuit = false)
        {
            InitializeComponent();
            BindingContext = new ErrorViewModel(errorMsg, canQuit);
        }
    }
}