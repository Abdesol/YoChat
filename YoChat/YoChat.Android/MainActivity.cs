using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Android.Widget;
using Android.Views;
using Android.Graphics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Android.Views.InputMethods;
using Android.Content;

[assembly: Dependency(typeof(YoChat.Droid.Environment))]
[assembly: Dependency(typeof(YoChat.Droid.ToastMessage))]
[assembly: Dependency(typeof(YoChat.Droid.KeyboardHelper))]

namespace YoChat.Droid
{
    [Activity(Label = "YoChat", Icon = "@mipmap/icon", WindowSoftInputMode =SoftInput.AdjustResize, Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            KeyboardHelper.Init(this);
            LoadApplication(new App());


        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        private bool _lieAboutCurrentFocus;
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            _lieAboutCurrentFocus = true;
            var result = base.DispatchTouchEvent(ev);
            _lieAboutCurrentFocus = false;

            return result;
        }
        
        public override Android.Views.View CurrentFocus
        {
            get
            {
                if (_lieAboutCurrentFocus)
                {
                    return null;
                }
                return base.CurrentFocus;
            }
        }

    }

    public class Environment : IEnvironment
    {
        [Obsolete]
        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
                return;

            var activity = Platform.CurrentActivity;
            var window = activity.Window;
            window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(color.ToPlatformColor());

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                var flag = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar;
                window.DecorView.SystemUiVisibility = darkStatusBarTint ? flag : 0;
            }

        }
    }

    public class ToastMessage : IToast
    {
        public void ToastShow(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }

    [Preserve(AllMembers = true)]
    public class KeyboardHelper : IKeyboardHelper
    {
        static Context _context;

        public static void Init(Context context) => _context = context;

        public void HideKeyboard()
        {
            var inputMethodManager = _context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && _context is Activity)
            {
                var activity = _context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }
    }
}