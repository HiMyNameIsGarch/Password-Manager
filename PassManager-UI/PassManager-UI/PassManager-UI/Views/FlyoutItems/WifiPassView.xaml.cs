using PassManager.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.FlyoutItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WifiPassView : ContentPage
    {
        public WifiPassView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.FlyoutItems.WifiPassViewModel();
        }
    }
}