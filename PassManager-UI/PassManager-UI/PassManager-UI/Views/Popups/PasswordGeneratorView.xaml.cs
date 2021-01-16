using Rg.Plugins.Popup.Pages;
using PassManager.ViewModels.Popups;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordGeneratorView : PopupPage
    {
        public PasswordGeneratorView()
        {
            InitializeComponent();
            BindingContext = new PasswordGeneratorVM();
        }
    }
}