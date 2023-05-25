using NTech.Base.Commons.BaseViewModel;
using NTech.Base.Resources.NNetSocket;
using PLCCommunication.Commands;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace PLCCommunication.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public log4net.ILog Logger { get; } = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public NServerSocket MainServerSocket { get; set; }
        public int PORT = 8500;

        private readonly Dispatcher _dispatcher;
        public static MainViewModel Instance { get; private set; }
        public MainWindow MainView { get; private set; }
        public PLCViewModel PLCViewModel { get; }
        public MainViewModel(Dispatcher dispatcher, MainWindow mainview, PLCViewModel plcViewModel) 
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
                return;

            _dispatcher = dispatcher;
            MainView= mainview;
            PLCViewModel= plcViewModel;

            if (PLCViewModel.ConnectPLC())
            {
                InitServer();
                PLCViewModel.PLCModel.StartProcess(200);
            }

            ResetCmd = new ResetCmd(PLCViewModel.PLCModel);
            StopCmd = new StopCmd(PLCViewModel.PLCModel);
        }

        private void InitServer()
        {
            MainServerSocket = new NServerSocket();
            MainServerSocket.ConnectionEventCallback += MainServerSocket_ConnectionEventCallback;
            MainServerSocket.ServerErrorEventCallback += MainServerSocket_ServerErrorEventCallback;
            MainServerSocket.StartListening(PORT);
        }
        private void MainServerSocket_ConnectionEventCallback(NServerSocket.EConnectionEventServer e, object obj)
        {

            switch (e)
            {
                case NServerSocket.EConnectionEventServer.SERVER_LISTEN:
                    PLCViewModel.PLCModel.StatusServer = "ServicePLC Started";
                    break;
                case NServerSocket.EConnectionEventServer.SERVER_RECEIVEDATA:
                    this._dispatcher.BeginInvoke(new Action(async() =>
                    {
                        if (MainServerSocket.ReceiveString != null)
                        {
                            await HandleReceivedDataEvent(MainServerSocket.ReceiveString);
                            //await HandleReceivedDataEvent_2(MainServerSocket.ReceiveString);
                        }
                    }));
                    break;
                default:
                    break;
            }
        }
        private Task HandleReceivedDataEvent(string data)
        {
            return Task.Run(() =>
            {
                //syntax: StrTag + "," + "Rst" . For example: L3P1,R
                string[] arr = data.Split(new char[] { ',' });
                if (arr[0] != null && arr[1].Equals("R"))
                {
                    switch (arr[0])
                    {
                        //Line 2
                        case "L2P1":
                        case "L3P1":
                            PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["1"]);
                            break;
                        case "L2P2":
                        case "L3P2":
                            PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["2"]);
                            break;
                        case "L2P3":
                        case "L3P3":
                            PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["3"]);
                            break;
                        case "L2P4":
                        case "L3P4":
                            PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["4"]);
                            break;
                        case "L2P5":
                        case "L3P5":
                            PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["5"]);
                            break;
                        case "L2P6":
                        case "L3P6":
                            PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["6"]);
                            break;
                    }
                }
            });
        }
        private Task HandleReceivedDataEvent_2(string data)
        {
            return Task.Run(() =>
            {
                switch (data)
                {
                    case "3":
                    case "9":
                        PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["1"]);
                        Logger.InfoFormat("Tag Printer: {0}", data);
                        break;
                    case "4":
                    case "10":
                        PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["2"]);
                        Logger.InfoFormat("Tag Printer: {0}", data);
                        break;
                    case "5":
                    case "11":
                        PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["3"]);
                        Logger.InfoFormat("Tag Printer: {0}", data);
                        break;
                    case "6":
                    case "12":
                        PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["4"]);
                        Logger.InfoFormat("Tag Printer: {0}", data);
                        break;
                    case "7":
                    case "13":
                        PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["5"]);
                        Logger.InfoFormat("Tag Printer: {0}", data);
                        break;
                    case "8":
                    case "14":
                        PLCViewModel.PLCModel.SetBit(PLCViewModel.PLCModel.DicBitReset["6"]);
                        Logger.InfoFormat("Tag Printer: {0}", data);
                        break;
                    default:
                        break;
                }
            });
        }
        private void MainServerSocket_ServerErrorEventCallback(string errorMsg)
        {
            Logger.Info($"Server PLC error: {errorMsg}");
        }
        public void ShowData(TextBlock tb, string data)
        {
            tb.Dispatcher.Invoke(new Action(() =>
            {
                tb.Text = data;
            }));
        }

        public ICommand ResetCmd { get; set; }
        public ICommand StopCmd { get; set;}
    }
}
