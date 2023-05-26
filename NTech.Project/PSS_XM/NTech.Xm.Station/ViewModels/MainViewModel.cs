#define LINE2
#define LINE3

using Newtonsoft.Json;
using NTech.Base.DataBase.Utils;
using NTech.Base.Resources.NNetSocket;
using NTech.Xm.Commons.Defines;
using NTech.Xm.Database;
using NTech.Xm.Database.Models;
using NTech.Xm.Station.Command;
using NTech.Xm.Station.Commons;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.Models;
using NTech.Xm.Station.Utils;
using NTech.Xm.Station.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using Kis.Toolkit;

namespace NTech.Xm.Station.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dispatcher _dispatcher;
        private NClientSocket _nClientSocket;
        private NClientSocket _nClientSocketLocalHost;
        private TaskManagerDB _taskManagerDB;
        private const string DBName = "XmDb";
        private string _connectionState = string.Empty;
        private string _dataReceived = string.Empty;
        private static object _objLock = new object();

        private readonly bool dataReceiveIsUTF8 = true;
        private const string GatePCIP = "192.168.0.1";
        private const string LocalHostIP = "127.0.0.1";
        private const int PORT = 9000;
        private const int LOCALHOST_PORT = 8500;
        private NServerSocket _nServerSocket;

        //Thread detect day change
        Thread _threadDetectDayChange;

        //Timer detect day change
        //DispatcherTimer _disTimerDetectDayChange;

#if !LINE2
        private NClientSocket _nClientSocketLine2;
#endif
#if LINE3
        private NClientSocket _nClientSocketLine3;
#endif

        public static MainViewModel Instance { get; private set; }
        public MainView MainView { get; private set; }

        ObservableCollection<MessagesDetailModel> _newMessagesDetailList = new ObservableCollection<MessagesDetailModel>();
        ObservableCollection<MessagesDetailModel> _printingMessagesDetailList = new ObservableCollection<MessagesDetailModel>();
        ObservableCollection<MessagesDetailModel> _allMessagesDetailList = new ObservableCollection<MessagesDetailModel>();
        MessagesDetailModel _messagesDetailModel = new MessagesDetailModel();
        MessagesDetailModel _messagesDetailModelHistory = new MessagesDetailModel();

        ObservableCollection<MessagesDetailModel> _newMessagesDetailListClone = new ObservableCollection<MessagesDetailModel>();
        ObservableCollection<MessagesDetailModel> _allMessagesDetailListClone = new ObservableCollection<MessagesDetailModel>();

        public MainViewModel(Dispatcher dispatcher, MainView mainView, LoginViewModel loginViewModel,
                             LineViewModel lineViewModel, PrinterViewModel printerViewModel,
                             TroughViewModel troughViewModel, SettingsViewModel settingsViewModel, EditMessagePrintedViewModel editMessagePrintedViewModel)
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                return;
            }
            this._dispatcher = dispatcher;
            this.MainView = mainView;
            this.LoginViewModel = loginViewModel;
            this.LineViewModel = lineViewModel;
            this.PrinterViewModel = printerViewModel;
            this.TroughViewModel = troughViewModel;
            this.SettingsViewModel = settingsViewModel;
            this.EditMessagePrintedViewModel = editMessagePrintedViewModel;


            this.SelectedItemChangeCmd = new SelectedItemChangeCmd(this);
            this.PushMessageAndContentLEDCmd = new PushMessageAndContentLEDCmd(this.LineViewModel, this.PrinterViewModel, this.TroughViewModel);
            this.CancelSelectionsCmd = new CancelSelectionsCmd(this);
            this.PreviewMouseRightButtonDownCmd = new PreviewMouseRightButtonDownCmd(this);
            this.SearchCmd = new SearchCmd(this);
            this.ReprintCmd = new ReprintCmd(this);
            this.ClosingCmd = new ClosingCmd(this);
            this.ReconnectCmd = new ReconnectCmd(this);
            this.PressF5KeyCmd = new PressF5KeyCmd(this.PrinterViewModel);
            this.EditMessagePrintedCmd = new EditMessagePrintedCmd(this.EditMessagePrintedViewModel);

            Init();

            this.LoginViewModel.LoginEvent += LoginViewModel_LoginEvent;
        }


        private void Init()
        {
            this.AppVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            this._taskManagerDB = new TaskManagerDB();

            this._nClientSocket = new NClientSocket(GatePCIP, PORT);
            this._nClientSocket.ConnectionEventCallback += _nClientSocket_ConnectionEventCallback;
            this._nClientSocket.ClientErrorEventCallback += _nClientSocket_ClientErrorEventCallback;
            this._nClientSocket.ClientConnect(dataReceiveIsUTF8);

            if (SettingsViewModel.UsePLC.Equals("true"))
            {
                _nClientSocketLocalHost = new NClientSocket(LocalHostIP, LOCALHOST_PORT);
                _nClientSocketLocalHost.ConnectionEventCallback += ConnectionCallBack_LocalHost;
                _nClientSocketLocalHost.ClientErrorEventCallback += ErrorCallBack_LocalHost;
                _nClientSocketLocalHost.ClientConnect(dataReceiveIsUTF8);
                //_nClientSocketLocalHost.SendMsg("Run");
            }

            this._nServerSocket = new NServerSocket();
            this._nServerSocket.ConnectionEventCallback += _nServerSocket_ConnectionEventCallback;
            this._nServerSocket.ServerErrorEventCallback += _nServerSocket_ServerErrorEventCallback;
            this._nServerSocket.StartListening(PORT);

#if !LINE2
            _nClientSocketLine2 = new NClientSocket("192.168.0.3", PORT);
            _nClientSocketLine2.ClientConnect();
#endif
#if LINE3
            _nClientSocketLine3 = new NClientSocket("192.168.0.2", PORT);
            _nClientSocketLine3.ClientConnect();
#endif

            this.DateNow = DateTime.Now;

            this._startDate = DateTime.Now.Date;
            this._endDate = DateTime.Now.Date.AddDays(1).AddTicks(-1);
            this.StartDateConverted = _startDate?.ToString("dd-MM-yyyy");
            this.EndDateConverted = _endDate?.ToString("dd-MM-yyyy");

            _nowDate = DateTime.Now.Date;
            _lastDate = DateTime.Now.Date.AddDays(-1);

            this.StrDateNow = DateNow.ToString("dd-MM-yyyy");
            //this.SelectMessagesDetail_NewAndAll(StrDateNow);
            SelectMessagesDetail_All2Day();


            this._threadDetectDayChange = new Thread(new ThreadStart(DetectDayChange));
            this._threadDetectDayChange.Start();
            this._threadDetectDayChange.IsBackground = true;

            //_disTimerDetectDayChange = new DispatcherTimer();
            //_disTimerDetectDayChange.Interval = TimeSpan.FromSeconds(5);
            //_disTimerDetectDayChange.Tick += _disTimerDetectDayChange_Tick;
            //_disTimerDetectDayChange.Start();

        }

        private void ErrorCallBack_LocalHost(string errorMsg)
        {
            MessageBox.Show(errorMsg);
        }

        private void ConnectionCallBack_LocalHost(NClientSocket.EConnectionEventClient e, object obj)
        {
            switch (e)
            {
                case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                    //Handle data received from PLC
                    this._dispatcher.BeginInvoke(new Action(async () =>
                    {
                        if (_nClientSocketLocalHost.ReceiveString != null)
                        {
                            await HandleDataReceivedFromPlc(_nClientSocketLocalHost.ReceiveString);
                        }
                    }));
                    //this.WirteLogSystem(this.MainView.paraLog, _nClientSocketLocalHost.ReceiveString, Define.SolidColorOK);
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                    this.WirteLogSystem(this.MainView.paraLog, $"Đã kết nối với Service PLC", Define.SolidColorOK);
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                    break;
                default:
                    break;
            }
        }

        private Task HandleDataReceivedFromPlc(string data)
        {
            return Task.Factory.StartNew(() =>
            {
                string[] arr = data.Split(new char[] { ',' });
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] != null)
                        LineViewModel.LINEByUse.Printers[i].PlcCount = uint.Parse(arr[i]);
                }
            });
        }
        //private void _disTimerDetectDayChange_Tick(object sender, EventArgs e)
        //{

        //    TimeSpan span = new TimeSpan(24, 0, 0);
        //    TimeSpan comparision;

        //    DateTime B = DateTime.Now.AddDays(0);
        //    comparision = B - DateNow;
        //    if (comparision > span)
        //    {
        //        DateNow = DateTime.Now;
        //        StrDateNow = DateNow.ToString("dd-MM-yyyy");
        //        this.SelectMessagesDetail_NewAndAll(StrDateNow);
        //    }

        //}

        void DetectDayChange()
        {
            while (true)
            {
                TimeSpan span = new TimeSpan(24, 0, 0);
                TimeSpan comparision;

                DateTime B = DateTime.Now.AddDays(0);
                comparision = B - DateNow;
                if (comparision > span)
                {
                    lock (_objLock)
                    {
                        //DateNow = DateTime.Now;
                        //StrDateNow = DateNow.ToString("dd-MM-yyyy");
                        //this.SelectMessagesDetail_NewAndAll(StrDateNow);

                        _nowDate = DateTime.Now.Date;
                        _lastDate = DateTime.Now.Date.AddDays(-1);

                        SelectMessagesDetail_All2Day();
                    }
                }
                Thread.Sleep(2000);
            }
        }
        private void _nServerSocket_ServerErrorEventCallback(string errorMsg)
        {

        }

        private void _nServerSocket_ConnectionEventCallback(NServerSocket.EConnectionEventServer e, object obj)
        {
            this._dispatcher.BeginInvoke(new Action(() =>
            {
                switch (e)
                {
                    case NServerSocket.EConnectionEventServer.SERVER_LISTEN:
                        break;
                    case NServerSocket.EConnectionEventServer.SERVER_RECEIVEDATA:
                        DATA_RECEIVED = _nServerSocket.ReceiveString;
                        if (!DATA_RECEIVED.Equals("Hi,Server!"))
                        {
                            if (DATA_RECEIVED.Contains("Prt"))
                            {
                                //this.SelectMessagesDetail_NewAndAll(StrDateNow);
                                SelectMessagesDetail_All2Day();
                            }
                            else if (DATA_RECEIVED.Contains("NPrD"))
                            {

                            }
                        }
                        break;
                    default:
                        break;
                }
            }));
        }

        private void _nClientSocket_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            this._dispatcher.BeginInvoke(new Action(() =>
            {
                switch (e)
                {
                    case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                        DATA_RECEIVED = _nClientSocket.ReceiveString;
                        if (DATA_RECEIVED.Contains("NeM") || DATA_RECEIVED.Contains("DeM"))
                        {
                            //this.SelectMessagesDetail_NewAndAll(StrDateNow);
                            SelectMessagesDetail_All2Day();
                        }
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                        this.ConnectionState = "Đã kết nối tới Server";
                        this.WirteLogSystem(this.MainView.paraLog, $"Đã kết nối tới Server", Define.SolidColorOK);
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                        this.ConnectionState = "Mất kết nối tới Server";
                        this.WirteLogSystem(this.MainView.paraLog, $"Mất kết nối tới Server", Define.SolidColorFail);
                        break;
                    default:
                        break;
                }
            }));
        }
        private void _nClientSocket_ClientErrorEventCallback(string errorMsg)
        {
            this._dispatcher.BeginInvoke(new Action(() =>
            {
                this.ConnectionState = "Không thể kết nối tới Server. Hãy thử lại!";
                this.WirteLogSystem(this.MainView.paraLog, $"kết nối thất bại", Define.SolidColorFail);
                MessageBox.Show(errorMsg);
            }));
        }
        private void LoginViewModel_LoginEvent(object sender, Xm.Commons.Defines.LOGIN_STATE loginState, Xm.Commons.Defines.ROLE role)
        {
            switch (loginState)
            {
                case LOGIN_STATE.LOGOUT:
                    this.IsLoginSuccess = false;
                    this.MainView.flyLoginView.IsOpen = false;
                    break;
                case LOGIN_STATE.LOGIN_SUCCESS:
                    this.IsLoginSuccess = true;
                    this.MainView.flyLoginView.IsOpen = false;
                    break;
                case LOGIN_STATE.LOGIN_FAILED:
                    this.IsLoginSuccess = false;
                    break;
            }
        }

        public EditMessagePrintedViewModel EditMessagePrintedViewModel { get; }
        public LoginViewModel LoginViewModel { get; }
        public SettingsViewModel SettingsViewModel { get; }
        public PrinterViewModel PrinterViewModel { get; }
        public TroughViewModel TroughViewModel { get; }
        public LineViewModel LineViewModel { get; }

        public ObservableCollection<MessagesDetailModel> NewMessagesDetailList
        {
            get { return _newMessagesDetailList; }
            set
            {
                if (Set(ref _newMessagesDetailList, value))
                {

                }
            }
        }
        public ObservableCollection<MessagesDetailModel> NewMessagesDetailListClone
        {
            get { return _newMessagesDetailListClone; }
            set
            {
                if (Set(ref _newMessagesDetailListClone, value))
                {

                }
            }
        }
        public ObservableCollection<MessagesDetailModel> PrintingMessagesDetailList
        {
            get { return _printingMessagesDetailList; }
            set
            {
                if (Set(ref _printingMessagesDetailList, value))
                {

                }
            }
        }
        public ObservableCollection<MessagesDetailModel> AllMessagesDetailList
        {
            get { return _allMessagesDetailList; }
            set
            {
                if (Set(ref _allMessagesDetailList, value))
                {

                }
            }
        }
        public ObservableCollection<MessagesDetailModel> AllMessagesDetailListClone
        {
            get { return _allMessagesDetailListClone; }
            set
            {
                if (Set(ref _allMessagesDetailListClone, value))
                {

                }
            }
        }
        public MessagesDetailModel MessagesDetailModel
        {
            get { return _messagesDetailModel; }
            set
            {
                if (Set(ref _messagesDetailModel, value))
                {
                    if (_messagesDetailModel != null)
                    {
                        this.SetTextInfoToLEDFix();
                        if (!SetTextInfoToTrough()) { return; }
                        if (!SetTextInfoSendToPrinter()) { return; }
                        if (!SetStateIsCanStart()) return;
                        if (!CheckStateMessage()) return;
                        this.PrinterViewModel.PRINTERSelected.NotifyMsgOfPrinter = "[...]";
                        this.PrinterViewModel.PRINTERSelected.NumberBag = _messagesDetailModel.NumberBags - _messagesDetailModel.NumberBagsPrinted;
                        this.PrinterViewModel.PRINTERSelected.MessagePrintCountShow = _messagesDetailModel.NumberBags;
                        this.Guid = _messagesDetailModel.Guid;
                    }
                }

            }
        }
        public MessagesDetailModel MessagesDetailModelHistory
        {
            get { return _messagesDetailModelHistory; }
            set
            {
                if (Set(ref _messagesDetailModelHistory, value))
                {
                    if (_messagesDetailModelHistory != null)
                    {
                        this.MessagesDetailModel = this.MessagesDetailModelHistory;
                        this.EditMessagePrintedViewModel.MessagesDetailModel = (MessagesDetailModel)this.MessagesDetailModelHistory.Clone();
                    }
                }
            }
        }

        private DateTime? _startDate; //DateTime.Now.Add(new TimeSpan(0,0,0));
        public DateTime? StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (value != null)
                {
                    if (Set(ref _startDate, value))
                    {
                        this.StartDateConverted = _startDate?.ToString("dd-MM-yyyy");
                    }
                }
            }
        }

        private DateTime? _endDate; //DateTime.Now.Add(new TimeSpan(23, 59, 0));
        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (value != null)
                {
                    if (Set(ref _endDate, value))
                    {
                        this.EndDateConverted = _endDate?.ToString("dd-MM-yyyy");
                    }
                }
            }
        }
        public string StartDateConverted { get; set; }
        public string EndDateConverted { get; set; }


        private DateTime? _lastDate;
        public DateTime? LastDate
        {
            get
            {
                return _lastDate;
            }
            set
            {
                if (value != null)
                {
                    if (Set(ref _lastDate, value))
                    {
                        this.LastDateConverted = _lastDate?.ToString("dd-MM-yyyy");
                    }
                }
            }
        }

        private DateTime? _nowDate;
        public DateTime? NowDate
        {
            get
            {
                return _nowDate;
            }
            set
            {
                if (value != null)
                {
                    if (Set(ref _nowDate, value))
                    {
                        this.NowDateConverted = _nowDate?.ToString("dd-MM-yyyy");
                    }
                }
            }
        }
        public string LastDateConverted { get; set; }
        public string NowDateConverted { get; set; }

        #region Methods
        public void SendResetCmdToPlc(string cmd)
        {
            _nClientSocketLocalHost.SendMsg(cmd);
        }
        private bool SetTextInfoSendToPrinter()
        {

            if (this.PrinterViewModel.PRINTERSelected != null && this._messagesDetailModel != null)
            {
                this.PrinterViewModel.PRINTERSelected.TextInfoSendToPrinter = _messagesDetailModel.CustomerCode + " " +
                                                                              _messagesDetailModel.ManufacturingDate + " " +
                                                                              _messagesDetailModel.LicensePlate;
                return true;
            }
            else
            {
                MessageBox.Show("Chưa chọn MÁY IN!\nXin vui lòng chọn MÁNG trước khi thực hiện thao tác này");
                return false;
            }

        }
        private bool SetTextInfoToTrough()
        {
            if (this.PrinterViewModel.PRINTERSelected == null)
                return false;
            if (this.PrinterViewModel.PRINTERSelected.IsUseTrough == false)
                return true;
            if (this.TroughViewModel.TROUGHSelected != null && this._messagesDetailModel != null)
            {
                this.TroughViewModel.TROUGHSelected.Text1Info = _messagesDetailModel.CustomerName.ToUpper() + " - " + _messagesDetailModel.LicensePlate.ToUpper();
                this.TroughViewModel.TROUGHSelected.Text2Info = _messagesDetailModel.Brand.ToUpper() + " - " + _messagesDetailModel.CementType.ToUpper() + " - " + _messagesDetailModel.BagCoverType.ToUpper();
                this.TroughViewModel.TROUGHSelected.Text3Info = _messagesDetailModel.NumberBags + "";
                return true;
            }
            else
            {
                MessageBox.Show("Chưa chọn MÁNG!\nXin vui lòng chọn MÁNG trước khi thực hiện thao tác này");
                return false;
            }
        }
        public void SetTextInfoToLEDFix()
        {

            if (this.PrinterViewModel.PRINTERSelected != null && this.PrinterViewModel.PRINTERSelected.LEDTableFixed != null && this._messagesDetailModel != null)
            {
                this.PrinterViewModel.PRINTERSelected.LEDTableFixed.Text1Info = _messagesDetailModel.CustomerName.ToUpper() + " - " + _messagesDetailModel.LicensePlate.ToUpper();
                this.PrinterViewModel.PRINTERSelected.LEDTableFixed.Text2Info = _messagesDetailModel.Brand.ToUpper() + " - " + _messagesDetailModel.CementType.ToUpper() + " - " + _messagesDetailModel.BagCoverType.ToUpper();
                this.PrinterViewModel.PRINTERSelected.LEDTableFixed.Text3Info = _messagesDetailModel.NumberBags + "";
            }
        }
        public bool SetStateIsCanStart()
        {
            if (this.PrinterViewModel.PRINTERSelected != null)
            {
                if (this.PrinterViewModel.PRINTERSelected.PRINTER_STATES == PRINTER_STATES.IS_CAN_PRINT
                    && this.PrinterViewModel.PRINTERSelected.IsUseTrough == false)
                {
                    this.IsCanStart = true;
                    return true;
                }
            }
            if (this.PrinterViewModel.PRINTERSelected != null && this.TroughViewModel.TROUGHSelected != null)
            {
                if (this.PrinterViewModel.PRINTERSelected.PRINTER_STATES == PRINTER_STATES.IS_CAN_PRINT &&
                    this.TroughViewModel.TROUGHSelected.TROUGH_STATES == TROUGH_STATES.ACTIVE_READY)
                {
                    this.IsCanStart = true;
                    return true;
                }
                else
                {
                    this.IsCanStart = false;
                    return false;
                }
            }
            else
            {
                this.MainView.dgNewMessages.SelectedItem = null;
                this.IsCanStart = false;
                MessageBox.Show("Chưa chọn MÁY IN và MÁNG!\nXin vui lòng chọn trước khi thực hiện thao tác này");
                return false;
            }
        }
        private bool CheckStateMessage()
        {
            if (_messagesDetailModel.MessageState.Equals(Define.GetEnumDescription(MESSAGE_STATE.NOT_YET_PRINT)))
            {
                this.PrinterViewModel.PRINTERSelected.MESSAGE_STATE = MESSAGE_STATE.NOT_YET_PRINT;
                return this.IsReprint = true;
            }
            else
            {
                if (_messagesDetailModel.NumberBags == _messagesDetailModel.NumberBagsPrinted)
                {
                    this.PrinterViewModel.PRINTERSelected.MESSAGE_STATE = MESSAGE_STATE.PRINT_DONE;
                    this.IsMessagePrintedDone = true;
                    return false;
                }
                else
                {
                    this.PrinterViewModel.PRINTERSelected.MESSAGE_STATE = MESSAGE_STATE.NOT_PRINT_DONE;
                    this.IsMessagePrintedDone = false;
                    return true;
                }
            }
        }
        public void WirteLogSystem(Paragraph para, string info = null, SolidColorBrush color = null)
        {
            para.Dispatcher.BeginInvoke(new Action(() =>
            {
                Run timeStamp = new Run();
                timeStamp.Text = DateTime.Now.ToString("HH:mm:ss:fff") + " ";
                //timeStamp.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#00897B");
                Run runText = new Run(info + Environment.NewLine);
                runText.Foreground = color;

                para.Inlines.Add(timeStamp);
                para.Inlines.Add(runText);
            }));
        }
        public void Reconnect()
        {
            this._nClientSocket.ClientConnect();
        }
        public void SendMsgToServer(string text)
        {
            this._nClientSocket.SendMsg(text);
        }
#if !LINE2
        public void SendMsgToLine3(string text)
        {
            this._nClientSocketLine2.SendMsg(text);
        }
#endif
#if LINE3
        public void SendMsgToLine2(string text)
        {
            this._nClientSocketLine3.SendMsg(text);
        }
#endif
        #endregion

        #region Database
        public void SelectMessagesDetail_NewAndAll(string date)
        {
            var database = _taskManagerDB.SelectMessagesDetailByDate(DBName, new string[1] { date });
            var listMessageDetail = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();
            if (listMessageDetail == null)
                return;

            var newMsgDetailList = listMessageDetail.Where(x => x.NumberBagsPrinted == 0 && x.MessageState == "Chưa in")
                                                    .ToList();
            var allMsgDetailList = listMessageDetail.Where(x => !x.MessageState.Equals("Chưa in"))
                                                    .ToList();

            ObservableCollection<MessagesDetailModel> newMsgDetail = new ObservableCollection<MessagesDetailModel>(newMsgDetailList);
            ObservableCollection<MessagesDetailModel> allMsgDetail = new ObservableCollection<MessagesDetailModel>(allMsgDetailList);
            this.NewMessagesDetailList = newMsgDetail;
            this.NewMessagesDetailListClone = this.NewMessagesDetailList;
            this.AllMessagesDetailList = allMsgDetail;
            this.AllMessagesDetailListClone = this.AllMessagesDetailList;

        }
        public void SelectMessagesDetail_All2Day()
        {
            var database = _taskManagerDB.SelectMessagesDetailBetween2Date(DBName, LastDate, NowDate);
            var listMessageDetail = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();
            if (listMessageDetail == null)
                return;

            var newMsgDetailList = listMessageDetail.Where(x => x.NumberBagsPrinted == 0 && x.MessageState == "Chưa in")
                                                    .ToList();
            var allMsgDetailList = listMessageDetail.Where(x => !x.MessageState.Equals("Chưa in"))
                                                    .ToList();

            ObservableCollection<MessagesDetailModel> newMsgDetail = new ObservableCollection<MessagesDetailModel>(newMsgDetailList);
            ObservableCollection<MessagesDetailModel> allMsgDetail = new ObservableCollection<MessagesDetailModel>(allMsgDetailList);
            this.NewMessagesDetailList = newMsgDetail;
            this.NewMessagesDetailListClone = this.NewMessagesDetailList;
            this.AllMessagesDetailList = allMsgDetail;
            this.AllMessagesDetailListClone = this.AllMessagesDetailList;
        }
        public void SelectMessagesDetail_Printing(string date)
        {
            var database = _taskManagerDB.SelectMessagesDetailByDate(DBName, new string[1] { date });
            var listMessageDetail = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();
            if (listMessageDetail == null)
                return;

            var printingMsgDetailList = listMessageDetail.Where(x => x.MessageState == "Đang in" && x.Line == this.LineViewModel.LINEByUse.LineName)
                                                         .ToList();
            ObservableCollection<MessagesDetailModel> printingMsgDetailObser = new ObservableCollection<MessagesDetailModel>(printingMsgDetailList);
            this.PrintingMessagesDetailList = printingMsgDetailObser;
        }

        public void SelectMessagesDetail_AllPrinting()
        {
            var lastDate = NowDate?.AddDays(-10);
            var database = _taskManagerDB.SelectMessagesDetailBetween2Date(DBName, lastDate, NowDate);
            var listMessageDetail = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();
            if (listMessageDetail == null)
                return;

            var printingMsgDetailList = listMessageDetail.Where(x => x.MessageState == "Đang in" && x.Line == this.LineViewModel.LINEByUse.LineName)
                                                         .ToList();
            ObservableCollection<MessagesDetailModel> printingMsgDetailObser = new ObservableCollection<MessagesDetailModel>(printingMsgDetailList);
            this.PrintingMessagesDetailList = printingMsgDetailObser;
        }

        public void SelectMessagesDetailBetween2Date()
        {
            var database = _taskManagerDB.SelectMessagesDetailBetween2Date(DBName, StartDate, EndDate);
            var listMessageDetail = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();
            if (listMessageDetail == null)
                return;
            List<MessagesDetailModel> allMsgDetailList = new List<MessagesDetailModel>();
            if (this.IsOnlyLINE)
            {
                allMsgDetailList = listMessageDetail.Where(x => x.Line == this.LineViewModel.LINEByUse.LineName)
                                                    .ToList();
            }
            else
            {
                allMsgDetailList = listMessageDetail.ToList();
            }
            ObservableCollection<MessagesDetailModel> allMsgDetail = new ObservableCollection<MessagesDetailModel>(allMsgDetailList);
            this.AllMessagesDetailList = allMsgDetail;
            this.AllMessagesDetailListClone = this.AllMessagesDetailList;
        }
        public void UpdateMessageDetail()
        {
            var database = _taskManagerDB.UpdateMessageDetail(DBName, this._messagesDetailModel);

        }

        //function update after when print done or stop print
        public void UpdateNumberBagPrintedAndMessageState(MessagesDetailModel messagesDetailModel)
        {
            var database = _taskManagerDB.UpdateNumberBagPrintedAndMessageState(DBName, messagesDetailModel);
        }
        #endregion

        #region Properties


        public string StrDateNow { get; set; }
        public DateTime DateNow { get; set; }

        private bool _isOnlyLINE = true;
        public bool IsOnlyLINE
        {
            get => _isOnlyLINE;
            set
            {
                Set(ref _isOnlyLINE, value);
            }
        }
        private bool _isLoginSuccess;
        public bool IsLoginSuccess
        {
            get => _isLoginSuccess;
            set
            {
                Set(ref _isLoginSuccess, value);
            }
        }
        public Guid Guid { get; set; }
        public string AppVersion { get; set; }
        public string ConnectionState
        {
            get { return _connectionState; }
            set
            {
                Set(ref _connectionState, value);
            }
        }
        public string DATA_RECEIVED
        {
            get { return _dataReceived; }
            set
            {
                Set(ref _dataReceived, value);
            }
        }

        private bool _isCanStart;
        public bool IsCanStart
        {
            get => _isCanStart;
            set
            {
                if (Set(ref _isCanStart, value))
                {

                }
            }
        }

        private bool _isMessagePrintedDone;
        public bool IsMessagePrintedDone
        {
            get => _isMessagePrintedDone;
            set
            {
                if (Set(ref _isMessagePrintedDone, value))
                {

                }
            }
        }

        private bool _isReprint;
        public bool IsReprint
        {
            get => _isReprint;
            set
            {
                if (Set(ref _isReprint, value))
                {

                }
            }
        }

        #endregion

        #region Commands
        public ICommand SelectedItemChangeCmd { get; }
        public ICommand PreviewMouseRightButtonDownCmd { get; }
        public ICommand PushMessageAndContentLEDCmd { get; }
        public ICommand CancelSelectionsCmd { get; }
        public ICommand SearchCmd { get; }
        public ICommand ReprintCmd { get; }
        public ICommand ClosingCmd { get; }
        public ICommand ReconnectCmd { get; }
        public ICommand PressF5KeyCmd { get; }
        public ICommand EditMessagePrintedCmd { get; }

        #endregion

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }
}
