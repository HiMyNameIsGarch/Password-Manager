using PassManager.ViewModels.FlyoutItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.FlyoutItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteView : ContentPage
    {
        public NoteView()
        {
            InitializeComponent();
            BindingContext = new NoteViewModel();
        }
    }
}