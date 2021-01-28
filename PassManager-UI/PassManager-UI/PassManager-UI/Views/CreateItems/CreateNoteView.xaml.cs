using PassManager.ViewModels.CreateItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.CreateItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNoteView : ContentPage
    {
        public CreateNoteView()
        {
            InitializeComponent();
            BindingContext = new CreateNoteVM();
        }
    }
}