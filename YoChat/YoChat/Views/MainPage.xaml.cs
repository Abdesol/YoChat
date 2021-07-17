using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace YoChat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async void ButtonClickedGesture(object sender, EventArgs args)
        {
            var frame = (Frame)sender;
            await frame.ScaleTo(0.8, 80);
            await frame.ScaleTo(1, 80);

            int which_page = 0;

            if (frame != choice_btns.Children[0])
            {
                which_page = 1;
            }
            await Navigation.PushModalAsync(new InitializerPage(which_page));
            

        }

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android)
                DependencyService.Get<IAndroidTerminate>().CloseApp();

            return base.OnBackButtonPressed();
        }
    }
}
