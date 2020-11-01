using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new Pages.MainPage();
        }
    }
}