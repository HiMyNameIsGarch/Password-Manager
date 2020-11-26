using Xamarin.Forms.Xaml;

namespace PassManager.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternetErrorView : Rg.Plugins.Popup.Pages.PopupPage
    {
        public InternetErrorView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.Popups.InternetErrorViewModel();
        }
    }
}