﻿using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using PassManager.CustomRenderer;
using PassManager_UI.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace PassManager_UI.Droid.CustomRenderer
{
    public class CustomEntryRenderer: EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        { 
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            
            if(Control != null)
            {
                var transparentColor = Android.Graphics.Color.Transparent;
                Control.SetBackgroundColor(transparentColor);
            }
        }
    }
}