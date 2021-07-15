using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace YoChat
{
    public partial class ChatViewModel : INotifyPropertyChanged
    {
        //private readonly IThemeService _themeService;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<MessageModel> AllMessages { get; set; }
        public ChatViewModel(string room_code)
        {
            AllMessages = new ObservableCollection<MessageModel>();
            msg_h = 60;
            this.room_code = room_code;
        }

        public Action RefreshScrollDown;


        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string _message {get;set;}
        public string message
        {
            get
            {
                return _message;
            }
            set
            {
                if(value != this._message)
                {
                    _message = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public ICommand SendMessageCommand => new Command(SendMessage);
        async void SendMessage(Object sender)
        {
            var btn = (ImageButton)sender;
            await btn.ScaleTo(0.3, 80);
            await btn.ScaleTo(0.4, 80);
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var m = message.Trim();
                    if (String.IsNullOrEmpty(m) == false)
                    {
                        var msg_model = new MessageModel(true, "Abdesol", m, DateTimeOffset.Now.ToUnixTimeSeconds());
                        MessagingCenter.Send(msg_model, "send");
                        AllMessages.Add(msg_model);
                        message = "";
                        msg_h = 60;
                        RefreshScrollDown();
                    }
                }
                else
                {
                    DependencyService.Get<IToast>().ToastShow("Internet connection is not available!");
                    await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                }
            }
            catch 
            {
                DependencyService.Get<IToast>().ToastShow("Unknown Error Occurred!");
                await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
            }
        }

        public int _msg_h { get; set; }
        public int msg_h
        {
            get
            {
                return _msg_h;
            }
            set
            {
                if (value != this._msg_h)
                {
                    _msg_h = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string _user_type_leave_text { get; set; }
        public string user_type_leave_text
        {
            get
            {
                return _user_type_leave_text;
            }
            set
            {
                if (value != this._user_type_leave_text)
                {
                    _user_type_leave_text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string _room_code { get; set; }
        public string room_code
        {
            get
            {
                return _room_code;
            }
            set
            {
                if (value != this._room_code)
                {
                    _room_code = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }
}
