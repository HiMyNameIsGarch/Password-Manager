using PassManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchView : ContentPage
    {
        public SearchView()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel();
        }
    }
}