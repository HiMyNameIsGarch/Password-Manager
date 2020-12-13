using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthenticationView : ContentPage
    {
        public AuthenticationView()
        {
            InitializeComponent();
            BindingContext = new AuthenticationViewModel();
        }
    }
}