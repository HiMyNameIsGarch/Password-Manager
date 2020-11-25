using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntireItemsView : ContentPage
    {
        public EntireItemsView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.EntireItemsViewModel(new ViewModels.PageService());
        }
    }
}