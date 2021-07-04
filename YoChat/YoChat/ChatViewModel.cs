using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace YoChat
{
    public partial class ChatViewModel : INotifyPropertyChanged
    {
        //private readonly IThemeService _themeService;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<MessageModel> AllMessages { get; set; }

        public ChatViewModel()
        {
           // _themeService = SimpleIoc.Default.GetInstance<IThemeService>();
            AllMessages = new ObservableCollection<MessageModel>();
            msg_h = 60;
            var other = new MessageModel(false, "Ahmed", "Hello Bro. How are you? you you you ", 1625037314);
            var me = new MessageModel(true, "Abdesol", "I am very fine. What about you? hmmhmhmhmh", 1625037314);
            for (int i = 0; i <= 7; i++)
            {
                if (i % 2 == 0)
                {
                    AllMessages.Add(me);
                }
                else
                {
                    AllMessages.Add(other);
                }
            }
        }

        public System.Action RefreshScrollDown;


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
                var m = message.Trim();
                if (String.IsNullOrEmpty(m) == false)
                {
                    var msg_model = new MessageModel(true, "Abdesol", m, DateTimeOffset.Now.ToUnixTimeSeconds());
                    AllMessages.Add(msg_model);
                    message = "";
                    msg_h = 60;
                    RefreshScrollDown();
                }
            }
            catch { }
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

    }
}
