using Android.Content;
using PassManager.CustomRenderer;
using PassManager_UI.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEditorRenderer))]
namespace PassManager_UI.Droid.CustomRenderer
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var transparentColor = Android.Graphics.Color.Transparent;
                Control.SetBackgroundColor(transparentColor);
            }
        }
    }
}