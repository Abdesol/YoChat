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
    public partial class InitializerPage : ContentPage
    {
        public int which_page { get; set; }
        public InitializerPage(int which_page)
        {
            this.which_page = which_page;
            InitializeComponent();
            if (which_page == 0)
            {
                RoomCodeFrame.IsVisible = false;
            }
        }


        public async void ButtonClickedGesture(object sender, EventArgs args)
        {
            
            try
            {
                var frame = (Frame)sender;
                await frame.ScaleTo(0.8, 80);
                await frame.ScaleTo(1, 80);
                if (RoomCodeFrame.IsVisible == true)
                {
                    string room_code = roomcode_entry.Text;
                    if(room_code == "ABCDE")
                    {
                        RoomCodeFrame.IsVisible = false;
                    }
                    else
                    {
                        not_exist_label.IsVisible = true;
                    }
                }
                else
                {
                    await Navigation.PushModalAsync(new ChatPage(this.which_page));
                }
            }
            catch
            {
                var btn = (ImageButton)sender;
                await btn.ScaleTo(0.3, 80);
                await btn.ScaleTo(0.4, 80);
                await Navigation.PopModalAsync(true);
            }

        }
    }
}