using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using NTech.Base.Resources.NNetSocket;

namespace NTech.Xm.Station
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NServerSocket serverSocket;
        NClientSocket clientSocket;
        public MainWindow()
        {
            InitializeComponent();
            serverSocket = new NServerSocket();
            clientSocket = new NClientSocket("192.168.0.10", 29043);
            serverSocket.ConnectionEventCallback += ServerSocket_ConnectionEventCallback;
            clientSocket.ConnectionEventCallback += ClientSocket_ConnectionEventCallback;
            clientSocket.ClientConnect();
            serverSocket.StartListening(9000);
            
        }

        private void ClientSocket_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            switch (e)
            {
                case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                    var s = clientSocket.ReceiveString;
                    //MessageBox.Show((string)obj);
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                    //MessageBox.Show("Connect Success!");
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                    break;
                default:
                    break;
            }
        }

        private void ServerSocket_ConnectionEventCallback(NServerSocket.EConnectionEventServer e, object obj)
        {
            switch (e)
            {
                case NServerSocket.EConnectionEventServer.SERVER_LISTEN:
                    break;
                case NServerSocket.EConnectionEventServer.SERVER_RECEIVEDATA:
                    tbMsgReceive.Dispatcher.Invoke(new Action(() =>
                    {
                        tbMsgReceive.Text = serverSocket.ReceiveString;
                    }));
                    break;
                default:
                    break;
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            //serverSocket.SendMsg(textBoxMsg.Text);
            //textBoxMsg.Text = "";
            byte[] byteMsg = new byte[]
            {
                0x1B,
                0x2,
                0x12,
                0x1B,
                0x3
            };
            clientSocket.SendMsg(byteMsg);

        }

        private void btnStartPrint_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteMsg = new byte[]
            {
                0x1B,
                0x2,
                0x11,
                0x1B,
                0x3
            };
            clientSocket.SendMsg(byteMsg);
        }

        private void btnGetCountPrint_Click(object sender, RoutedEventArgs e)
        {
            //byte[] byteMsg = new byte[]
            //{
            //    0x1B,
            //    0x2,
            //    0x1F,
            //    0x1B,
            //    0x3
            //};
            //clientSocket.SendMsg(byteMsg);
            string s = "" + (char)0x1B + (char)0x2 + "" + (char)0x1F + "" + (char)0x1B + (char)0x3;
            clientSocket.SendMsg(s);
        }
    }
}
