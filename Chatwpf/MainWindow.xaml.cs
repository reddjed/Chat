
using MyLib.Library;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int port;
        IPAddress ip;
        ChatClient chatClient;

        public MainWindow()
        {
            InitializeComponent();
            scrl.ScrollToEnd();
            btn.IsEnabled = false;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbMsg.Text = "";
            try
            {
                if ((lbClients.SelectedItem as string).Equals("All"))
                {

                    chatClient.Send(tbMsg.Text);

                }
                else
                {
                    string to = lbClients.SelectedItem.ToString();

                    chatClient.Send(tbMsg.Text, to);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                btn.IsEnabled = false;
            }
            
        }

        private void ReciveAll(Message m)
        {
            Messagectrl msg = new Messagectrl
            {

                Msg = m.Msg,
                UserName = m.From,
                Time = m.Date,
                Padding = new Thickness { Bottom = 0, Left = 10, Right = 10, Top = 10 }
                
            };

            (scrl.Content as StackPanel).Children.Add(msg);
        }

        private void btnConn_Click(object sender, RoutedEventArgs e)
        {
            btn.IsEnabled = true;
            bool correcctPort = int.TryParse(tbPort.Text, out port);
            if (!IPAddress.TryParse(tbIp.Text, out ip) && !correcctPort)
            {
                MessageBox.Show("You entered wrong ip or port!", "Error", MessageBoxButton.OK);
                return;
            }

            TcpClient client = new TcpClient();
            ip = ip is null ? IPAddress.Parse("0.0.0.0") : ip;
            IPEndPoint ep = new IPEndPoint(ip, port);


            try
            {

                //btnConn.Visibility = Visibility.Collapsed;

                client.Connect(ep);
                chatClient = new ChatClient(client);
                chatClient.Recive += ReciveAll;
                chatClient.Login = tbLogin.Text;
                chatClient.SendList += ReciveClnts;
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void ReciveClnts(List<string> list)
        {
            list.Add("All");
        
            lbClients.ItemsSource = list;
            lbClients.SelectedItem = "All";
        }
    }
}
