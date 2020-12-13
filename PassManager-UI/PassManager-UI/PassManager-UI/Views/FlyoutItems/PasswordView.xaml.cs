using PassManager.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.FlyoutItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordView : ContentPage
    {
        public PasswordView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.FlyoutItems.PasswordViewModel();
        }
    }
}