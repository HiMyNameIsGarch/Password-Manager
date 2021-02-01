using Android.OS;
using PassManager.Models.Interfaces;
using Plugin.CurrentActivity;
using System;
using System.Drawing;

[assembly: Xamarin.Forms.Dependency(typeof(PassManager_UI.Droid.CustomRenderer.StatusBarRenderer))]
namespace PassManager_UI.Droid.CustomRenderer
{
    public class StatusBarRenderer : IStatusBarPlatformSpecific
    {
        public StatusBarRenderer()
        {

        }
        public void ChangeNavigationBarColor(Android.Graphics.Color color)
        {
            var Window = CrossCurrentActivity.Current.Activity.Window;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.SetNavigationBarColor(color);
            }
        }
        public void ChangeStatusBarColor(Android.Graphics.Color color)
        {
            var Window = CrossCurrentActivity.Current.Activity.Window;
            Window.SetStatusBarColor(color);
        }
    }
}