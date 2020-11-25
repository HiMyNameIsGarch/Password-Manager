using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.CreateItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePasswordView : ContentPage
    {
        public CreatePasswordView()
        {
            InitializeComponent();
            BindingContext = new ViewModels.CreateItems.CreatePasswordVM();
        }
    }
}