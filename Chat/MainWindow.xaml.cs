using Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var stream = chatClient.Client.GetStream();
            byte[] msg = Encoding.Unicode.GetBytes(tbMsg.Text);
            stream.Write(msg, 0, msg.Length);

           
        }

        private void btnConn_Click(object sender, RoutedEventArgs e)
        {

            bool correcctPort = int.TryParse(tbPort.Text, out port);
            if (!IPAddress.TryParse(tbIp.Text, out ip) && !correcctPort)
            {
                MessageBox.Show("You entered wrong ip or port!", "Error", MessageBoxButton.OK);
                return;
            }

            TcpClient client = new TcpClient();
            ip = ip is null ? IPAddress.Parse("0.0.0.0"): ip;
            IPEndPoint ep = new IPEndPoint(ip, port);
           
            
            try
            {
                chatClient.Login = tbLogin.Text;
                

                client.Connect(ep);

                client.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
    }
}
