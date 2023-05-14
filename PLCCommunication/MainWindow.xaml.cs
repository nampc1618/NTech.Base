using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PLCCommunication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PLC plc = new PLC();
        Thread threadPlc;
        public MainWindow()
        {
            InitializeComponent();

            plc.CheckLineWithIPPC();
            plc.LoadPLCXml();
            plc.InitServer();

            this.DataContext = plc;

            if(plc.ConnectPLC())
            {
                threadPlc = new Thread(new ThreadStart(ReadValues));
                threadPlc.IsBackground = false;
                threadPlc.Start();
            }
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if(threadPlc.IsAlive)
                threadPlc.Abort();
        }
        void ReadValues()
        {
            while(true)
            {
                plc.Count1 = (uint)plc.PLCInstance.Read(plc.DicRegister["1"]);
                plc.Count2 = (uint)plc.PLCInstance.Read(plc.DicRegister["2"]);
                plc.Count3 = (uint)plc.PLCInstance.Read(plc.DicRegister["3"]);
                plc.Count4 = (uint)plc.PLCInstance.Read(plc.DicRegister["4"]);
                plc.Count5 = (uint)plc.PLCInstance.Read(plc.DicRegister["5"]);
                plc.Count6 = (uint)plc.PLCInstance.Read(plc.DicRegister["6"]);

                //Send to data for station
                string data = plc.Count1 + "," + plc.Count2 + "," + plc.Count3 
                              + "," + plc.Count4 + "," + plc.Count5 + "," + plc.Count6;
                plc.NServerSocket.SendMsg(data);

                Dispatcher.Invoke(new Action(() =>
                {
                    lbCountNumberPLC1.Text = plc.Count1.ToString();
                    lbCountNumberPLC2.Text = plc.Count2.ToString();
                    lbCountNumberPLC3.Text = plc.Count3.ToString();
                    lbCountNumberPLC4.Text = plc.Count4.ToString();
                    lbCountNumberPLC5.Text = plc.Count5.ToString();
                    lbCountNumberPLC6.Text = plc.Count6.ToString();
                }));

                Thread.Sleep(200);
            }
        }

        private void btnReset4_Click(object sender, RoutedEventArgs e)
        {
            plc.PLCInstance.Write(plc.DicBitReset["4"], 0);
        }
    }
}
