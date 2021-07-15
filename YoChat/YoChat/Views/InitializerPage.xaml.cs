using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace YoChat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitializerPage : ContentPage
    {
        public int which_page { get; set; }
        public InitializerPage(int which_page)
        {
            InitializeComponent();

            if (which_page == 0)
            {
                RoomCodeFrame.IsVisible = false;
            }
            this.which_page = which_page;
        }

        public bool isbusy = false;
        public ClientSocket cli_sock = new ClientSocket();

        private bool was_join = false;

        private string room_code = "";

        public async void ButtonClickedGesture(object sender, EventArgs args)
        {
            if(isbusy == false)
            {

                isbusy = true;
                try
                {
                    var frame = (Frame)sender;
                    await frame.ScaleTo(0.8, 80);
                    await frame.ScaleTo(1, 80);
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        
                        var conn = await cli_sock.Connect();
                        if (conn == true)
                        {
                            try
                            {
                                if (RoomCodeFrame.IsVisible == true)
                                {
                                    was_join = true;
                                    string room_code = roomcode_entry.Text;
                                    if (String.IsNullOrEmpty(room_code) == false)
                                    {
                                        var roomsetup_resp = cli_sock.RoomSetup(1, room_code);
                                        if (roomsetup_resp["Error"] != "true")
                                        {
                                            RoomCodeFrame.IsVisible = false;
                                        }
                                        else
                                        {
                                            not_exist_label.IsVisible = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (was_join == false)
                                    {
                                        var roomsetup_resp = cli_sock.RoomSetup(0, "");
                                        room_code = roomsetup_resp["RoomCode"];
                                    }

                                    string nickname = nickname_entry.Text;
                                    if (String.IsNullOrEmpty(nickname) == false)
                                    {

                                        var setnick = cli_sock.SetNickName(nickname);
                                        if (!setnick)
                                        {
                                            DependencyService.Get<IToast>().ToastShow("Unknown Error Occurred!");
                                        }
                                        else
                                        {
                                            DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                                            await Task.Delay(TimeSpan.FromSeconds(0.2));
                                            await Navigation.PushModalAsync(new ChatPage(which_page, room_code, cli_sock));
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                DependencyService.Get<IToast>().ToastShow("Unknown Error Occurred!");
                            }
                        }
                        else
                        {
                            DependencyService.Get<IToast>().ToastShow("Unknown Error Occurred!");
                        }
                    }
                    else
                    {
                        DependencyService.Get<IToast>().ToastShow("Internet connection is not available!");
                    }
                }
                catch
                {
                    var btn = (ImageButton)sender;
                    await btn.ScaleTo(0.3, 80);
                    await btn.ScaleTo(0.4, 80);
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                    await Navigation.PopModalAsync(true);
                }
                isbusy = false;
            }
        }
    }
}