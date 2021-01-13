using PassManager.CustomRenderer;
using PassManager_UI.UWP.CustomRenderer;
using Windows.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace PassManager_UI.UWP.CustomRenderer
{
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var transparentBackground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Transparent);
                Control.BackgroundFocusBrush = transparentBackground;
                Control.Background = transparentBackground;
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
                Control.Margin = new Windows.UI.Xaml.Thickness(15, 5, 15, 5);
            }
        }
    }
}
