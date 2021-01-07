using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListItemView : ContentPage
    {
        public ListItemView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.ListItemViewModel();
        }
    }
}