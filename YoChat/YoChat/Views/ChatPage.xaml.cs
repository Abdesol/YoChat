﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace YoChat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        public ChatPage(int which_page, string room_code, ClientSocket cli_sock)
        {
            BindingContext = new ChatViewModel( room_code);
            InitializeComponent();

            if (which_page == 0)
            {
                (BindingContext as ChatViewModel).user_type_leave_text = "Close Room";
            }
            else
            {
                (BindingContext as ChatViewModel).user_type_leave_text = "Leave Room";
                copy_code_btn.IsVisible = false;
            }

            var recv_thr = new Thread(cli_sock.LiveRecv);
            recv_thr.Start();
            MessagingCenter.Subscribe<RecvModel>(this, "recv", async (msg_obj) =>
            {
                if(msg_obj.RoomClosed != true)
                {
                    var msg_mod = new MessageModel(false, msg_obj.User, msg_obj.Message, msg_obj.timestamp);
                    (BindingContext as ChatViewModel).AllMessages.Add(msg_mod);
                    (BindingContext as ChatViewModel).RefreshScrollDown();
                }
                else
                {
                    recv_thr.Abort();
                    cli_sock.sender.Close();
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                    await Task.Delay(TimeSpan.FromSeconds(0.2));
                    DependencyService.Get<IToast>().ToastShow("The room is closed by the creator!");
                    leaving();
                }
            });

            MessagingCenter.Subscribe<MessageModel>(this, "send", (msg_obj) => 
            {
                new_count = 0;
                var msg_dict = new Dictionary<string, string>();
                msg_dict.Add("Leave", "false");
                msg_dict.Add("Message", msg_obj.message);

                cli_sock.send(msg_dict);
            });

            MessagingCenter.Subscribe<string>(this, "leave", (s) =>
            {
                var msg_dict = new Dictionary<string, string>();
                msg_dict.Add("Leave", "true");
                msg_dict.Add("Message", "");
                if (s == "NotClose")
                {
                    msg_dict.Add("Close", "No");
                }
                else
                {
                    msg_dict.Add("Close", "Yes");
                }

                recv_thr.Abort();
                cli_sock.send(msg_dict);
                cli_sock.sender.Close();
            });

            MessageList.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if (e.Item == null) return;
                if (sender is ListView lv) lv.SelectedItem = null;
            };
            
        }

        public async void leaving()
        {
            await Navigation.PushModalAsync(new MainPage());
        }

        public async void BackClicked(object sender, EventArgs args)
        {
            var btn = (ImageButton)sender;
            await btn.ScaleTo(0.3, 80);
            await btn.ScaleTo(0.4, 80);

            DependencyService.Get<IKeyboardHelper>().HideKeyboard();
            MessagingCenter.Send("NotClose", "leave");

            await Navigation.PushModalAsync(new MainPage());
        }

        public int new_count = 0;
        public void EditorEdited(object sender, TextChangedEventArgs e)
        {

            var old_text = String.IsNullOrEmpty(e.OldTextValue) ? "" : e.OldTextValue;
            var new_text = String.IsNullOrEmpty(e.NewTextValue) ? "" : e.NewTextValue;


            if (new_text.Length == 0)
            {
                new_count = 0;
                viewModel.msg_h = 60;
            }

            else if(old_text.Length < new_text.Length)
            {
                if (new_text[new_text.Length - 1] == '\n')
                {
                    if (viewModel.msg_h <= Application.Current.MainPage.Height / 3) viewModel.msg_h += 20;
                    new_count = 0;
                }
                else
                {
                    new_count ++;
                    if(new_count > msg_entry.Width/10 + 4)
                    {
                        if (viewModel.msg_h <= Application.Current.MainPage.Height / 3) viewModel.msg_h += 20;
                        new_count = 0;
                    }
                }
            }
            else
            {
                new_count--;
                Console.WriteLine($"Backspace clicked {new_count} , {new_text.Length}");
                if(new_count == -1 & new_text.Length > 0)
                {
                    int no_line = 3;
                    foreach(var s in new_text) if (s == '\n') no_line++;

                    if(no_line*20 <= Application.Current.MainPage.Height / 3) viewModel.msg_h -= 20;

                    int n = 0;
                    for(int i = new_text.Length-1; i>=0; i--)
                    {
                        if (new_text[i] == '\n') break;
                        n++;
                    }
                    new_count = n;
                }
            }
        }

        private ChatViewModel viewModel
        {
            get { return BindingContext as ChatViewModel; }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            viewModel.RefreshScrollDown = () => {
                if (viewModel.AllMessages.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() => {

                        try
                        {
                            var msg = viewModel.AllMessages.Cast<MessageModel>().LastOrDefault();
                            if (msg != null)
                            {
                                MessageList.ScrollTo(msg, ScrollToPosition.End, false);
                            }
                        }
                        catch
                        {

                        }
                    });
                }
            };
        }

        public async void LeaveClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            await btn.ScaleTo(0.8, 80);
            await btn.ScaleTo(1, 80);
            DependencyService.Get<IKeyboardHelper>().HideKeyboard();
            MessagingCenter.Send("", "leave");

            await Navigation.PushModalAsync(new MainPage());
        }

        public async void CopyClicked(object sender , EventArgs e)
        {
            await Clipboard.SetTextAsync((BindingContext as ChatViewModel).room_code);
            DependencyService.Get<IToast>().ToastShow("Code copied to clipboard");
        }

    }
}
