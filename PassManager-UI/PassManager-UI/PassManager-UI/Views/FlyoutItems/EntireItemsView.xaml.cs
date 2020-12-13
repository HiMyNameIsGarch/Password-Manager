using PassManager.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.FlyoutItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntireItemsView : ContentPage
    {
        public EntireItemsView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.FlyoutItems.EntireItemsViewModel();
        }
    }
}