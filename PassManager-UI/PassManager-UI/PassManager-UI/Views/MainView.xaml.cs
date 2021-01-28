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
            Routing.RegisterRoute("CreatePassword", typeof(CreatePasswordView));
            Routing.RegisterRoute("CreateWifi", typeof(CreateWifiView));
            Routing.RegisterRoute("CreateNote",typeof(CreateNoteView));
            Routing.RegisterRoute("SearchItem", typeof(SearchView));
        }
    }
}