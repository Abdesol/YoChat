using System;
using System.Collections.Generic;
using System.Text;

namespace YoChat
{
    public class RecvModel
    {
        public string Message { get; set; }
        public string User { get; set; }
        public bool RoomClosed { get; set; }
        public long timestamp { get; set; }

    }
}
