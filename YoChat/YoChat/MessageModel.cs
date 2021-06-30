using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace YoChat
{
    public class MessageModel
    {
        public string user { get; set; }
        public string message { get; set; }
        public string time { get; set; }
        public LayoutOptions position { get; set; }
        public Color msg_color { get; set; }

        public MessageModel(bool from_me, string user, string message, long timestamp)
        {
            if(from_me == true)
            {
                this.user = "Me";
                position = LayoutOptions.EndAndExpand;
                msg_color = Color.FromHex("#3E6189");
            }
            else
            {
                this.user = user;
                position = LayoutOptions.StartAndExpand;
                msg_color = Color.FromHex("#222E3A");
            }

            this.message = message;

            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            this.time = dtDateTime.ToString("h:mm tt");
        }
    }
}
