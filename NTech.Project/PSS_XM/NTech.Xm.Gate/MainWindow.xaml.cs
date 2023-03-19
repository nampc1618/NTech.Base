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
using NTech.Base.DataBase.Utils;
using NTech.Base.Resources.NNetSocket;
using NTech.Xm.Database;
using NTech.Xm.Database.Models;

namespace NTech.Xm.Gate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NClientSocket clientSocket;
        TaskManagerDB _taskManagerDB = null;
        public MainWindow()
        {
            InitializeComponent();

            clientSocket = new NClientSocket("127.0.0.1", 9000);
            clientSocket.ConnectionEventCallback += ClientSocket_ConnectionEventCallback;
            clientSocket.ClientErrorEventCallback += ClientSocket_ClientErrorEventCallback;

            _taskManagerDB = new TaskManagerDB();
        }

        private void ClientSocket_ClientErrorEventCallback(string errorMsg)
        {
            tbMsgError.Dispatcher.Invoke(new Action(() =>
            {
                tbMsgError.Text = errorMsg;
            }));
        }

        private void ClientSocket_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            switch (e)
            {
                case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                    tbMsgReceive.Dispatcher.Invoke(new Action(() =>
                    {
                        tbMsgReceive.Text = clientSocket.ReceiveString;
                    }));
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                    break;
                default:
                    break;
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            clientSocket.SendMsg(textBoxMsg.Text);
            textBoxMsg.Text = "";
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            clientSocket.ClientConnect();
        }

        private void btnTestDB_Click(object sender, RoutedEventArgs e)
        {
            var database = _taskManagerDB.SelectCustomers("XmDb");
            var listCustomer = database?.DataSet?.Tables[0].ToObservableCollection<CustomersModel>();
            if(listCustomer != null && listCustomer.Count > 0)
            {
                MessageBox.Show("Co " + listCustomer.Count + " khach hang");
            }
        }
    }
}
