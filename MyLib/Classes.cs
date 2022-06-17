using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace MyLib
{
    

    namespace Library
    {
        public class Message
        {
            public enum Type
            {
                All, Private, ClientList
            }
            public string From { get; set; }
            public string To { get; set; }
            public DateTime Date { get; set; } = DateTime.Now;
            public string Msg { get; set; }
            public Type MsgType { get; set; } = Type.All;

        }
        public class ChatClient
        {
            public event Action<Message> Recive;
            public event Action<List<string>> SendList;
            public string Login { get; set; }
            public TcpClient Client { get; set; }
            public ChatClient(TcpClient client)
            {
                Client = client;
                Listener();
                
            }
            public ChatClient(string ip, int port)
            {
                Client = new TcpClient();
                try
                {
                    Client.Connect(ip, port);
                }
                catch (SocketException ex)
                {


                }
            }
            private async void Listener()
            {
                try
                {
                    var s = Client.GetStream();

                    while (true)
                    {
                        byte[] buff = new byte[255];
                        await s.ReadAsync(buff, 0, buff.Length);

                        string json = Encoding.UTF8.GetString(buff);
                        Message m = JsonConvert.DeserializeObject<Message>(json);
                        switch (m.MsgType)
                        {
                            case Message.Type.All:
                                Recive?.Invoke(m);
                                break;
                            case Message.Type.Private:
                                Recive?.Invoke(m);
                                break;
                            case Message.Type.ClientList:
                                List<string> list = JsonConvert.DeserializeObject<List<string>>(m.Msg);
                                list.Remove(Client.Client.LocalEndPoint.ToString());
                                SendList?.Invoke(list);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch(System.IO.IOException ex)
                {
                    MessageBox.Show(ex.Message);
                    Client = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
               
                


            }
            public async void Send(string msg)
            {
                Message m = new Message
                {
                    From = Client.Client.LocalEndPoint.ToString(),
                    Msg = msg,
                    MsgType = Message.Type.All
                };
                string json = JsonConvert.SerializeObject(m);

                byte[] data = Encoding.UTF8.GetBytes(json);
                var stream = Client.GetStream();
                await stream.WriteAsync(data, 0, data.Length);
            }
            public async void Send(string msg,string to)
            {
                Message m = new Message
                {
                    From = Client.Client.LocalEndPoint.ToString(),
                    Msg = msg,
                    MsgType = Message.Type.Private,
                    To = to
                };
                string json = JsonConvert.SerializeObject(m);

                byte[] data = Encoding.UTF8.GetBytes(json);
                var stream = Client.GetStream();
                await stream.WriteAsync(data, 0, data.Length);
            }


        }
    }

}
