using Android.Graphics.Drawables;
using Android.Text;
using YoChat.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Java.Lang;
using Android.Views;
using System.ComponentModel;

#pragma warning disable CS0612 // Type or member is obsolete
[assembly: ExportRenderer(typeof(Entry), typeof(NoUnderlineEntry))]
[assembly: ExportRenderer(typeof(Editor), typeof(NoUnderlineEditor))]
#pragma warning restore CS0612 // Type or member is obsolete
namespace YoChat.Droid
{
    [System.Obsolete]
    public class NoUnderlineEntry : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }

    [System.Obsolete]
    public class NoUnderlineEditor : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}