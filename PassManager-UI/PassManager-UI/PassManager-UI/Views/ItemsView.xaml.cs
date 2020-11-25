using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassManager.Views.CreateItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsView : Shell
    {
        public ItemsView()
        {
            InitializeComponent();
            //register routes
            Routing.RegisterRoute("ListItem", typeof(ListItemView));
            Routing.RegisterRoute("CreatePassword", typeof(CreatePasswordView));
            Routing.RegisterRoute("CreateWifi", typeof(CreateWifiView));
            //make the binding
            BindingContext = new ViewModels.ItemsViewModel();
        }
    }
}