using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YoChat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MsgCtrl : Grid
    {
        private readonly IThemeService _themeService;

        public MsgCtrl()
        {
            InitializeComponent();

            _themeService = SimpleIoc.Default.GetInstance<IThemeService>();
            _themeService.ThemeChanged += HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            SetAppearance();
        }

        private void SetAppearance()
        {
            if (!IsMine.HasValue) return;
            msg_frame.HorizontalOptions = IsMine.Value ? LayoutOptions.End : LayoutOptions.Start;
            msg_frame.BackgroundColor = IsMine.Value ? GetMineColor(_themeService.IsLightTheme) : GetRemoteColor(_themeService.IsLightTheme);
        }

        private Color GetMineColor(bool isLight)
        {
            return isLight ? Color.FromHex("5BA6F0") : Color.FromHex("3E6189");
        }
        private Color GetRemoteColor(bool isLight)
        {
            return isLight ? Color.FromHex("4979AF") : Color.FromHex("222E3A");
        }

        public static readonly BindableProperty IsMineProperty = BindableProperty.Create(
            nameof(IsMine),
            typeof(bool?),
            typeof(MsgCtrl),
            propertyChanged: SetMine);


        public bool? IsMine
        {
            get => (bool?)GetValue(IsMineProperty);
            set => SetValue(IsMineProperty, value);
        }


        private static void SetMine(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not MsgCtrl ctrl || newValue is not bool) return;

            ctrl.SetAppearance();
        }

        // Message Text Property ------------------------------------------------------------------------------
        public static readonly BindableProperty MsgTextProperty = BindableProperty.Create(
            nameof(MsgText),
            typeof(string),
            typeof(MsgCtrl),
            propertyChanged: SetMsgText);


        private bool MsgText
        {
            get => (bool)GetValue(MsgTextProperty);
            set => SetValue(MsgTextProperty, value);
        }

        private static void SetMsgText(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not MsgCtrl ctrl) return;
            ctrl.msg_label.Text = (string)newValue;
        }

        // User Text Property ------------------------------------------------------------------------------
        public static readonly BindableProperty UserTextProperty = BindableProperty.Create(
            nameof(UserText),
            typeof(string),
            typeof(MsgCtrl),
            propertyChanged: SetUserText);


        private bool UserText
        {
            get => (bool)GetValue(UserTextProperty);
            set => SetValue(UserTextProperty, value);
        }

        private static void SetUserText(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not MsgCtrl ctrl) return;
            ctrl.user_name_label.Text = (string)newValue;
        }

        // Time Text Property ------------------------------------------------------------------------------
        public static readonly BindableProperty TimeTextProperty = BindableProperty.Create(
            nameof(TimeText),
            typeof(string),
            typeof(MsgCtrl),
            propertyChanged: SetTimeText);


        private bool TimeText
        {
            get => (bool)GetValue(TimeTextProperty);
            set => SetValue(TimeTextProperty, value);
        }

        private static void SetTimeText(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not MsgCtrl ctrl) return;
            ctrl.time_label.Text = (string)newValue;
        }


    }
}