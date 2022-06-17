using MyLib.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace ChatServer
{
    class Program
    {

        static List<TcpClient> clients = new List<TcpClient>();
        static int port = 50_505;
        static void Main(string[] args)
        {
            IPAddress iPAddress = IPAddress.Loopback;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);

            TcpListener listener = new TcpListener(iPEndPoint);
            try
            {
                listener.Start();
                Console.WriteLine("Server is running. Waiting for connection...");
                while (true)
                {
                    TcpClient c = listener.AcceptTcpClient();
                    string ep = c.Client.RemoteEndPoint.ToString();
                    Console.WriteLine("Connected to " + ep);
                    clients.Add(c);


                    Task.Factory.StartNew(new Action(() =>
                    {

                        Message m = GetCLientList();
                        string json = JsonConvert.SerializeObject(m);
                        byte[] Dataclients = Encoding.UTF8.GetBytes(json);

                        clients.ForEach(u => u.GetStream().Write(Dataclients, 0, Dataclients.Length));


                    }));
                    ClientListener(c);
                }

            }
            catch (SocketException)
            {

                Console.WriteLine("Something went wrong(");
            }
        }
        private static async void ClientListener(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                while (true)
                {

                    byte[] buff = new byte[255];

                    await stream.ReadAsync(buff, 0, buff.Length);

                    string json = Encoding.UTF8.GetString(buff);
                    Message m = JsonConvert.DeserializeObject<Message>(json);
                    if (m.MsgType == Message.Type.All)
                    {
                        clients.ForEach(async (c) =>
                        {
                            if (c != client)
                            {
                                var s = c.GetStream();
                                await s.WriteAsync(buff, 0, buff.Length);

                            }
                        });
                    }
                    else
                    {
                        var ToClient = clients.FirstOrDefault(c => c.Client.RemoteEndPoint.ToString().Equals(m.To));
                        if (ToClient != null)
                        {
                            var s = ToClient.GetStream();
                            await s.WriteAsync(buff, 0, buff.Length);
                        }
                    }

                }

            }
            catch (System.IO.IOException)
            {

                Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected");
                clients.Remove(client);
                await Task.Run(new Action(() =>
                {
                    GetCLientList();
                }));
            }
        }
        static private Message GetCLientList()
        {
            List<string> list = clients.Select(c => c.Client.RemoteEndPoint.ToString()).ToList();
            string json = JsonConvert.SerializeObject(list);
            return new Message { MsgType = Message.Type.ClientList, Msg = json };
        }
    }
}

