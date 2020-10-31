using PassManager.CustomRenderer;
using PassManager_UI.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomeEntry), typeof(CustomEntryRenderer))]
namespace PassManager_UI.UWP.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
                var transparentBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Transparent);
                Control.Background = transparentBrush;
                Control.BorderBrush = transparentBrush;
                Control.Margin = new Windows.UI.Xaml.Thickness(15,5,15,5);
            }
        }
    }
}
