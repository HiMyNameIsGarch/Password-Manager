using PassManager.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PassManager.Models;

namespace PassManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthenticationView : ContentPage
    {
        public AuthenticationView()
        {
            InitializeComponent();
            BindingContext = new AuthenticationViewModel(new PageService());
        }
    }
}