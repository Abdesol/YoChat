using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace YoChat
{
    public class MessageModel
    {
        public bool is_mine { get; set; }
        public string user { get; set; }
        public string message { get; set; }
        public string time { get; set; }
        public double msg_w { get; set; }
        //public LayoutOptions position { get; set; }
        //public Color msg_color_dark { get; set; }
        //public Color msg_color_light { get; set; }

        //public Color msg_color { get; set; }

        private double max_msg_width = DeviceDisplay.MainDisplayInfo.Width / 2.25;

        public MessageModel(bool from_me, string user, string message, long timestamp)
        {
            is_mine = from_me;
            this.user = from_me ? "Me" : user;

            /*
            if(from_me == true)
            {
                this.user = "Me";
                //position = LayoutOptions.EndAndExpand;
                //var brush = Application.Current.Resources["sent_msg_color"] as SolidColorBrush;
                //msg_color = brush.Color;
                //msg_color = System.Drawing.Color.FromArgb(brush);
                //msg_color_dark = Color.FromHex("3E6189");
                //msg_color_light = Color.FromHex("3590EA");
            }
            else
            {
                this.user = user;
                //position = LayoutOptions.StartAndExpand;
                // brush = Application.Current.Resources["recv_msg_color"] as SolidColorBrush;
                //msg_color = brush.Color;
                //msg_color_dark = Color.FromHex("222E3A");
                //msg_color_light = Color.FromHex("CADCF2");
            }*/

            

            var msg_cur = (message.Length*8)+80;
            msg_w = msg_cur > max_msg_width ? max_msg_width : msg_cur;

            this.message = message;

            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            this.time = dtDateTime.ToString("h:mm tt");
        }

    }
}
