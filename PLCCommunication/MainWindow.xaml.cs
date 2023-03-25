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

namespace PLCCommunication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PLC plc = null;
        public MainWindow()
        {
            InitializeComponent();

            plc = new PLC();
            this.DataContext = plc;
        }

        private void btnConnectPLC_Click(object sender, RoutedEventArgs e)
        {
            plc.NServerSocket.SendMsg("Xin chao! Tao la PLC");
        }
    }
}
