﻿using PassManager.CustomRenderer;
using PassManager_UI.UWP.CustomRenderer;
using Windows.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace PassManager_UI.UWP.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var transparentBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Transparent);
                Control.Background = transparentBrush;
                Control.BackgroundFocusBrush = transparentBrush;
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
                Control.BorderBrush = transparentBrush;
                Control.Margin = new Windows.UI.Xaml.Thickness(15, 5, 15, 5);
            }
        }
    }
}
