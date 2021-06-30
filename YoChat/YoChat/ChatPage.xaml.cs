using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YoChat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        public ChatPage(int which_page)
        {
            BindingContext = new ChatViewModel();
            InitializeComponent();

            if (which_page == 0)
            {
                (BindingContext as ChatViewModel).user_type_leave_text = "Close Room";
            }
            else
            {
                (BindingContext as ChatViewModel).user_type_leave_text = "Leave Room";
            }
        }

        public async void BackClicked(object sender, EventArgs args)
        {
            var btn = (ImageButton)sender;
            await btn.ScaleTo(0.3, 80);
            await btn.ScaleTo(0.4, 80);
            await Navigation.PushModalAsync(new MainPage());
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

                        MessageList.ScrollTo(viewModel.AllMessages[viewModel.AllMessages.Count - 1], ScrollToPosition.End, true);
                    });
                }
            };
        }

        public void EditorEdited(object sender, TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (text.Count() == 0)
            {
                viewModel.msg_h = 60;
            }
            else
            {
                var new_text = text[text.Count() - 1];


                if (new_text == '\n')
                {
                    if (viewModel.msg_h <= Application.Current.MainPage.Height / 3)
                    {
                        viewModel.msg_h += 20;
                    }
                }
            }
            Console.WriteLine($"Current Length -> {viewModel.msg_h}");
        }

        public async void LeaveClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            await btn.ScaleTo(0.8, 80);
            await btn.ScaleTo(1, 80);
            await Navigation.PushModalAsync(new MainPage());
        }
    }

    public partial class ChatEditor : Editor
    {

        public ChatEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~ChatEditor()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateMeasure();
        }
    }
}
