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
        public Android.Graphics.Color ColorFromHex(string hexValue)
        {
            if (hexValue.Length != 6) return Android.Graphics.Color.Red;
            var color = ColorTranslator.FromHtml(hexValue);
            int r = Convert.ToInt16(color.R);
            int g = Convert.ToInt16(color.G);
            int b = Convert.ToInt16(color.B);
            return Android.Graphics.Color.Rgb(r,g,b);
        }
    }
}