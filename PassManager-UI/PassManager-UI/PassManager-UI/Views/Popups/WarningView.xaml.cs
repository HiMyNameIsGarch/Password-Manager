using PassManager.ViewModels.Popups;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WarningView : PopupPage
    {
        public WarningView(string warningText)
        {
            InitializeComponent();
            BindingContext = new WarningViewModel(warningText);
        }
    }
}