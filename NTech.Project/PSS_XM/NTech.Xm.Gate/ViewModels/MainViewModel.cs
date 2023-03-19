using Newtonsoft.Json;
using NTech.Base.Commons.BaseCommand;
using NTech.Base.DataBase.Models;
using NTech.Base.DataBase.Utils;
using NTech.Base.Resources.NNetSocket;
using NTech.Xm.Commons.Defines;
using NTech.Xm.Database;
using NTech.Xm.Database.Models;
using NTech.Xm.Gate.Command;
using NTech.Xm.Gate.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace NTech.Xm.Gate.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dispatcher _dispatcher;
        private NServerSocket _nServerSocket;
        private TaskManagerDB _taskManagerDB;
        private const string DBName = "XmDb";

        private const int PORT = 9000;
        private string _connectionState;
        private string _dataReceived = string.Empty;

        public MainView MainView { get; }
        public LoginViewModel LoginViewModel { get; }
        public SettingsViewModel SettingsViewModel { get; }

        ObservableCollection<MessagesDetailModel> _newMessagesDetailList = new ObservableCollection<MessagesDetailModel>();
        ObservableCollection<MessagesDetailModel> _printingMessagesDetailList = new ObservableCollection<MessagesDetailModel>();
        MessagesDetailModel _messagesDetailModel = new MessagesDetailModel();

        ObservableCollection<CustomersModel> _customerModelList = new ObservableCollection<CustomersModel>();
        public ObservableCollection<CustomersModel> CustomerModelList
        {
            get { return _customerModelList; }
            set
            {
                if (Set(ref _customerModelList, value))
                {

                }
            }
        }
        CustomersModel _customerModel = new CustomersModel();
        public CustomersModel CustomerModel
        {
            get { return _customerModel; }
            set
            {
                Set(ref _customerModel, value);
            }
        }
        public void SelectCustomers()
        {
            var database = _taskManagerDB.SelectCustomers(DBName);
            var listCustomer = database?.DataSet?.Tables[0].ToObservableCollection<CustomersModel>();
            if (listCustomer != null)
                CustomerModelList = listCustomer;
        }

        public static MainViewModel Instance { get; private set; }


        public MainViewModel(Dispatcher dispatcher, MainView mainView, LoginViewModel loginViewModel, SettingsViewModel settingsViewModel)
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
            this.SettingsViewModel = settingsViewModel;

            this.InsertMessageDetailCmd = new InsertMessageDetailCmd(this);
            Init();

            this.LoginViewModel.LoginEvent += LoginViewModel_LoginEvent;

            this.CancelSelectionsCmd = new CancelSelectionsCmd(this);
            this.SearchCmd = new SearchCmd(this);
            this.DeleteMsgNewCmd = new DeleteMsgNewCmd(this);
        }

        private void LoginViewModel_LoginEvent(object sender, LOGIN_STATE loginState, ROLE role)
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

        public void Init()
        {
            this.AppVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            this._taskManagerDB = new TaskManagerDB();

            this._nServerSocket = new NServerSocket();
            this._nServerSocket.ConnectionEventCallback += _nServerSocket_ConnectionEventCallback;
            this._nServerSocket.ServerErrorEventCallback += _nServerSocket_ServerErrorEventCallback;
            this._nServerSocket.StartListening(PORT);

            this.DateNow = DateTime.Now.ToString("dd-MM-yyyy");
            SelectMessagesDetail_New(DateNow);
            SelectMessagesDetail_Printing(DateNow);
            SelectCustomers();
        }

        private void _nServerSocket_ConnectionEventCallback(NServerSocket.EConnectionEventServer e, object obj)
        {
            switch (e)
            {
                case NServerSocket.EConnectionEventServer.SERVER_LISTEN:
                    this._connectionState = "Server đã sẵn sàng";
                    this.WirteLogSystem(this.MainView.paraLog, "Server bắt đầu lắng nghe...", CommonDefines.SolidColorOK);
                    break;
                case NServerSocket.EConnectionEventServer.SERVER_RECEIVEDATA:
                    DATA_RECEIVED = _nServerSocket.ReceiveString;
                    if (!DATA_RECEIVED.Equals("Hi,Server!"))
                    {
                        if (DATA_RECEIVED.Contains("Prt") || DATA_RECEIVED.Contains("PrD"))
                        {
                            this.SelectMessagesDetail_Printing(DateNow);
                            this.SelectMessagesDetail_New(DateNow);
                        }
                        else if (DATA_RECEIVED.Contains("NPrD"))
                        {

                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private void _nServerSocket_ServerErrorEventCallback(string errorMsg)
        {
            this.WirteLogSystem(this.MainView.paraLog, errorMsg, CommonDefines.SolidColorFail);
        }

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
        public MessagesDetailModel MessagesDetailModel
        {
            get { return _messagesDetailModel; }
            set
            {
                if (Set(ref _messagesDetailModel, value))
                {

                }
            }
        }
        public MessagesDetailModel _messagesDetailModelSelected;
        public MessagesDetailModel MessagesDetailModelSelected
        {
            get { return _messagesDetailModelSelected; }
            set
            {
                if (Set(ref _messagesDetailModelSelected, value))
                {
                    this.IsEditMsg = false;
                }
            }
        }
        private bool _isEditMsg;
        public bool IsEditMsg
        {
            get => _isEditMsg;
            set
            {
                Set(ref _isEditMsg, value);
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
        public string DateNow { get; set; }
        public void WirteLogSystem(Paragraph para, string info = null, SolidColorBrush color = null)
        {
            para.Dispatcher.BeginInvoke(new Action(() =>
            {
                Run timeStamp = new Run();
                timeStamp.Text = DateTime.Now.ToString("HH:mm:ss:fff") + " ";
                Run runText = new Run(info + Environment.NewLine);
                runText.Foreground = color;

                para.Inlines.Add(timeStamp);
                para.Inlines.Add(runText);
            }));
        }
        //public void SendJsonString()
        //{
        //    string jsonString = JsonConvert.SerializeObject(MessagesDetailModel);
        //    _nClientSocket.SendMsg(jsonString, "utf8");
        //}
        public void ServerSendToClient(string strCmd)
        {
            _nServerSocket.SendMsg(strCmd);
        }

        #region Select Commands
        public ICommand SelectArrangeProductsCmd { get; }
        public ICommand SelectBagCoverCmd { get; }
        public ICommand SelectCementBrandCmd { get; }
        public ICommand SelectCustomersCmd { get; }
        public ICommand SelectMessagesDetailCmd { get; }
        public ICommand SelectProductsCmd { get; }
        public ICommand SelectRolesCmd { get; }
        public ICommand CancelSelectionsCmd { get; }
        #endregion

        #region Insert Commands
        public ICommand InsertMessageDetailCmd { get; }

        #endregion

        public ICommand SearchCmd { get; }
        public ICommand DeleteMsgNewCmd { get; }

        #region Select commands
        public bool CheckExistGuid(Guid guid)
        {
            try
            {
                var database = _taskManagerDB.SelectMessageDetailByGuidAndDateNow(DBName, new object[] { DateTime.Now.ToString("dd-MM-yyyy"), guid });
                var listMessageDetail = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();

                var listMsg = listMessageDetail?.Where(x => x.Guid == guid).ToList();
                if (listMsg.Count == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public void SelectMessagesDetail_New(string date)
        {
            try
            {
                var database = _taskManagerDB.SelectMessagesDetailByDate(DBName, new string[1] { date });
                var listMessageDetail = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();

                var newMsgDetailList = listMessageDetail.Where(x => x.NumberBagsPrinted == 0 && x.MessageState == "Chưa in").ToList();
                ObservableCollection<MessagesDetailModel> newMsgDetail = new ObservableCollection<MessagesDetailModel>(newMsgDetailList);
                this.NewMessagesDetailList = newMsgDetail;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SelectMessagesDetail_Printing(string date)
        {
            try
            {
                var database = _taskManagerDB.SelectMessagesDetailByDate(DBName, new string[1] { date });
                var listMessageDetail = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();

                var printingMsgDetailList = listMessageDetail.Where(x => x.MessageState == "Đang in").ToList();
                ObservableCollection<MessagesDetailModel> printingMsgDetailObser = new ObservableCollection<MessagesDetailModel>(printingMsgDetailList);
                this.PrintingMessagesDetailList = printingMsgDetailObser;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void DeleteMessagesDetail()
        {
            _taskManagerDB.DeleteMessageDetail(DBName, this.MessagesDetailModelSelected);
        }
        #endregion

        #region Method for Insert commands
        public void InsertMessageDetail(MessagesDetailModel messagesDetailModel)
        {
            var database = _taskManagerDB.InsertMessageDetail(DBName, messagesDetailModel);
        }
        #endregion
        #region Test RelayCommand
        //public ICommand TestCommand { get; } =
        //    new RelayCommand<string>
        //    (
        //        (a) => true,
        //        (a) => ShowMsg(a as string)
        //    );
        //public static void ShowMsg(string msg)
        //{
        //    MessageBox.Show("Hellu" + msg);
        //}

        //public ICommand SaveDBTestCmd { get; } =
        //    new RelayCommand<string>
        //    (
        //        (agr) => true,
        //        (agr) => SaveInfoTest(new Test() { MyName = "namlp", MyCode = "1993"})
        //    );
        //public static DBResultModel SaveInfoTest(Test test)
        //{
        //    var database = _taskManagerDB.InsertTest("XmDb", test);
        //    return database;
        //}
        #endregion
    }
}
