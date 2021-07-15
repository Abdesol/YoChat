using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace YoChat
{
    public class ClientSocket
    {
        private string ip = "192.168.1.2";
        private int port = 4545;

        public Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public async Task<bool> Connect()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    sender.Connect(ip, port);
                    return true;
                }
                catch { }
            }
            return false;
        }

        public bool send(Dictionary<string, string> data) // Data sending method
        {
            try
            {
                var data_str = JsonConvert.SerializeObject(data);
                sender.Send(Encoding.ASCII.GetBytes(data_str));
            }
            catch { return false; }
            return true;
        }

        public Dictionary<string, string> recv() // Data Receiving method
        {
            var bt = new byte[1024];
            sender.Receive(bt);
            var data_bt = Encoding.ASCII.GetString(bt);
            var data_dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(data_bt);
            return data_dict;
        }

        public Dictionary<string, string> RoomSetup(int type_of_join, string room_code)
        {
            var return_data = new Dictionary<string, string>();
            var join_type_dict = new Dictionary<string, string>();
            if (type_of_join == 0)
            {
                join_type_dict.Add("Type", "Create");
                join_type_dict.Add("RoomCode", "");
            }
            else
            {
                join_type_dict.Add("Type", "Join");
                join_type_dict.Add("RoomCode", room_code);
            }
            var send_join_type = send(join_type_dict);
            if (send_join_type == true)
            {
                var recv_response = recv();
                if (recv_response["Error"] == "true")
                {
                    return_data.Add("Error", "true");
                    return_data.Add("ErrorDesc", recv_response["ErrorDesc"]);
                }
                else
                {
                    return_data.Add("Error", "false");
                    return_data.Add("ErrorDesc", "");
                    return_data.Add("RoomCode", recv_response["RoomCode"]);
                }
            }
            else
            {
                return_data.Add("Error", "true");
                return_data.Add("ErrorDesc", "Unknown Error occurred!");
            }
            return return_data;
        }

        public bool SetNickName(string nick_name)
        {
            try
            {
                sender.Send(Encoding.ASCII.GetBytes(nick_name));
            }
            catch {return false;}
            
            return true;
        }

        public bool islive = true;
        public void LiveRecv()
        {
            var bt = new byte[1024];
            while (islive)
            {
                try
                {
                    sender.Receive(bt);
                    Console.WriteLine("Receving...");
                    var data_bt = Encoding.ASCII.GetString(bt);
                    var data = JsonConvert.DeserializeObject<RecvModel>(data_bt);
                    Device.BeginInvokeOnMainThread(() => 
                    {
                        MessagingCenter.Send(data, "recv");
                    });
                }
                catch { }
            }
            sender.Close();
        }
    }
}
