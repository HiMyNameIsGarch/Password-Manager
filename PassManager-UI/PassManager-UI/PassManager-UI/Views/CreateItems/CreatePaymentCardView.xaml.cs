using PassManager.ViewModels.CreateItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.CreateItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePaymentCardView : ContentPage
    {
        public CreatePaymentCardView()
        {
            InitializeComponent();
            BindingContext = new CreatePaymentCardVM();
        }
    }
}