using PassManager.Models;
using PassManager.ViewModels.FlyoutItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassManager.Views.FlyoutItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntireItemsView : ContentPage
    {
        public EntireItemsView()
        {
            InitializeComponent();
            BindingContext = new EntireItemsViewModel();
            ((EntireItemsViewModel)this.BindingContext).OnScroll = ((obj, list) =>
            { //We tell the action to scroll to the passed in object here
                list.ScrollTo(obj, ScrollToPosition.MakeVisible, false);
            });
        }
    }
}