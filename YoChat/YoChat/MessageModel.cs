using System;
using System.Collections.Generic;
using System.Linq;
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
            this.user = from_me ? "Me" : user;

            var all_length = new List<int>();
            int k = 0;
            foreach(char i in message)
            {
                if(i == '\n')
                {
                    all_length.Add(k);
                    k = 0;
                    continue;
                }
                k += 1;
            }
            all_length.Add(k);

            int msg_max_length = all_length.Max();

            var msg_cur = (msg_max_length * 8)+78;
            msg_w = msg_cur > max_msg_width ? max_msg_width : msg_cur;

            this.message = message;

            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            time = dtDateTime.ToString("h:mm tt");


            
        }

    }
}
