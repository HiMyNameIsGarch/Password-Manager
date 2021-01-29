using PassManager.Enums;
using PassManager.Models;
using PassManager.Views.CreateItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : Shell
    {
        public MainView()
        {
            InitializeComponent();
            //register routes
            Routing.RegisterRoute("ListItem", typeof(ListItemView));
            Routing.RegisterRoute("Create" + TypeOfItems.Password.ToSampleString(), typeof(CreatePasswordView));
            Routing.RegisterRoute("Create" + TypeOfItems.Wifi.ToSampleString(), typeof(CreateWifiView));
            Routing.RegisterRoute("Create" + TypeOfItems.Note.ToSampleString(), typeof(CreateNoteView));
            Routing.RegisterRoute("Create" + TypeOfItems.PaymentCard.ToSampleString(),typeof(CreatePaymentCardView));
            Routing.RegisterRoute("SearchItem", typeof(SearchView));
        }
    }
}