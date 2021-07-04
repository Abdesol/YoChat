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

        private double max_msg_width = DeviceDisplay.MainDisplayInfo.Width / 2.25;

        public MessageModel(bool from_me, string user, string message, long timestamp)
        {
            is_mine = from_me;
            //this.user = 1;
            this.user = from_me ? "Me" : user;

            var msg_cur = (message.Length*8)+80;
            msg_w = msg_cur > max_msg_width ? max_msg_width : msg_cur;

            this.message = message;

            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            time = dtDateTime.ToString("h:mm tt");


            
        }

    }
}
