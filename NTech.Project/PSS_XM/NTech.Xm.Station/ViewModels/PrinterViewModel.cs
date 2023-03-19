#define LINE1
#define LINE2
#define LINE3
#define SHORTCUT

using BespokeFusion;
using ControlzEx.Standard;
using MahApps.Metro.Controls.Dialogs;
using NTech.Base.DataBase.Utils;
using NTech.Base.Resources.NNetSocket;
using NTech.Xm.Database;
using NTech.Xm.Database.Models;
using NTech.Xm.Station.Command;
using NTech.Xm.Station.Commons;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.Models;
using NTech.Xm.Station.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace NTech.Xm.Station.ViewModels
{
    public class PrinterViewModel : ViewModelBase
    {
        private readonly Dispatcher _dispatcher;

        #region Printers

        public PRINTER PRINTER1_LINE1 { get; }
        public PRINTER PRINTER2_LINE1 { get; }

        public PRINTER PRINTER1_LINE2 { get; }
        public PRINTER PRINTER2_LINE2 { get; }
        public PRINTER PRINTER3_LINE2 { get; }
        public PRINTER PRINTER4_LINE2 { get; }
        public PRINTER PRINTER5_LINE2 { get; }
        public PRINTER PRINTER6_LINE2 { get; }

        public PRINTER PRINTER1_LINE3 { get; }
        public PRINTER PRINTER2_LINE3 { get; }
        public PRINTER PRINTER3_LINE3 { get; }
        public PRINTER PRINTER4_LINE3 { get; }
        public PRINTER PRINTER5_LINE3 { get; }
        public PRINTER PRINTER6_LINE3 { get; }

        #endregion

        public LineViewModel LineViewModel { get; }
        public static PrinterViewModel Instance { get; private set; }

        private TaskManagerDB _taskManagerDB;
        private const string DBName = "XmDb";
        private char _commandExecuted = (char)0x6;
        private char _commandNoExecuted = (char)0x15;


        public PrinterViewModel(LineViewModel lineViewModel, Dispatcher dispatcher)
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
            this.LineViewModel = lineViewModel;
            this._taskManagerDB = new TaskManagerDB();
            this.GuidMsgNewest = new GuidMsgNewest();

            this.IPPrinterList = this.DeserializePrinterXML();
            this.SelectAllGuidMsgNewest();

            this.PRINTER1_LINE1 = new PRINTER() { PrinterName = "MÁY IN 1", Tag = 1, StrTag = "L1P1", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER1_LINE1", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER1_LINE1 };
            this.PRINTER2_LINE1 = new PRINTER() { PrinterName = "MÁY IN 2", Tag = 2, StrTag = "L1P2", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER2_LINE1", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER2_LINE1 };

            this.PRINTER1_LINE2 = new PRINTER() { PrinterName = "MÁY IN 1", Tag = 3, StrTag = "L2P1", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER1_LINE2", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER1_LINE2 };
            this.PRINTER2_LINE2 = new PRINTER() { PrinterName = "MÁY IN 2", Tag = 4, StrTag = "L2P2", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER2_LINE2", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER2_LINE2 };
            this.PRINTER3_LINE2 = new PRINTER() { PrinterName = "MÁY IN 3", Tag = 5, StrTag = "L2P3", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER3_LINE2", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER3_LINE2 };
            this.PRINTER4_LINE2 = new PRINTER() { PrinterName = "MÁY IN 4", Tag = 6, StrTag = "L2P4", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER4_LINE2", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER4_LINE2 };
            this.PRINTER5_LINE2 = new PRINTER() { PrinterName = "MÁY IN 5", Tag = 7, StrTag = "L2P5", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER5_LINE2", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER5_LINE2 };
            this.PRINTER6_LINE2 = new PRINTER() { PrinterName = "MÁY IN 6", Tag = 8, StrTag = "L2P6", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER6_LINE2", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER6_LINE2 };

            this.PRINTER1_LINE3 = new PRINTER() { PrinterName = "MÁY IN 1", Tag = 9, StrTag = "L3P1", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER1_LINE3", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER1_LINE3 };
            this.PRINTER2_LINE3 = new PRINTER() { PrinterName = "MÁY IN 2", Tag = 10, StrTag = "L3P2", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER2_LINE3", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER2_LINE3 };
            this.PRINTER3_LINE3 = new PRINTER() { PrinterName = "MÁY IN 3", Tag = 11, StrTag = "L3P3", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER3_LINE3", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER3_LINE3 };
            this.PRINTER4_LINE3 = new PRINTER() { PrinterName = "MÁY IN 4", Tag = 12, StrTag = "L3P4", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER4_LINE3", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER4_LINE3 };
            this.PRINTER5_LINE3 = new PRINTER() { PrinterName = "MÁY IN 5", Tag = 13, StrTag = "L3P5", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER5_LINE3", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER5_LINE3 };
            this.PRINTER6_LINE3 = new PRINTER() { PrinterName = "MÁY IN 6", Tag = 14, StrTag = "L3P6", PRINTER_STATES = PRINTER_STATES.DISCONNECTED, GuidMsgNewestName = "GuidMsgNewestPRINTER6_LINE3", GuidMsgNewest = this.GuidMsgNewest.GuidMsgNewestPRINTER6_LINE3 };

            this.AddPRINTERList();
            //this.AddPrintersName();
            this.AssignIPForPrinters();
            this.AddPrinterInLine();

            this.CreatePrinterClientSocketAndRiseEvent();

            this.PrintCmd = new RequestPrintStatusCmd(this);
            this.RequestPrintStatusCmd = new RequestPrintStatusCmd(this);
            this.StartJetCmd = new StartJetCmd(this);
            this.StopPrintCmd = new StopPrintCmd(this);
            this.DownloadMessageDataCmd = new DownloadMessageDataCmd(this);
            this.DeleteMessageDataCmd = new DeleteMessageDataCmd(this);
            this.LoadPrintMessageCmd = new LoadPrintMessageCmd(this);


            this.DateNow = DateTime.Now.ToString("dd-MM-yyyy");
            this._printersName = new List<string>();
            this._fieldOrientations = new List<string>() { "Bình thường", "Lật ngang", "Lật dọc", "Lật ngang + dọc" };

            //this.SelectAllMessageDetaiNewestlForPrinter();
            this.LoadMsgParam();
        }

        #region Properties
        public IPPrinterList IPPrinterList { get; set; }

        List<PRINTER> _printerList;
        public List<PRINTER> PRINTERList
        {
            get { return _printerList; }
            set
            {
                Set(ref _printerList, value);
            }
        }

        private List<string> _printersName;
        public List<string> PRINTERSName
        {
            get { return _printersName; }
            set
            {
                Set(ref _printersName, value);
            }
        }

        private PRINTER _printerSelected;
        public PRINTER PRINTERSelected
        {
            get { return _printerSelected; }
            set
            {
                if (Set(ref _printerSelected, value))
                {
                    //switch (_printerSelected.PRINTER_STATES)
                    //{
                    //    case PRINTER_STATES.CONNECTED:
                    //        this.IsCanStart = true;
                    //        break;
                    //    case PRINTER_STATES.DISCONNECTED:
                    //    case PRINTER_STATES.PRINTING:
                    //    case PRINTER_STATES.PRINT_STOP:
                    //    case PRINTER_STATES.PRINT_DONE:
                    //        this.IsCanStart = false;
                    //        break;
                    //}
                }
            }
        }
        private List<string> _fieldOrientations;
        public List<string> FieldOrientations
        {
            get => _fieldOrientations;
            set
            {
                Set(ref _fieldOrientations, value);
            }
        }
        public string DateNow { get; set; }

        private bool _isManual;
        public bool IsManual
        {
            get => _isManual;
            set
            {
                if (Set(ref _isManual, value))
                {

                }
            }
        }
        private GuidMsgNewest GuidMsgNewest { get; set; }
        #endregion

        #region Methods
        void SelectAllGuidMsgNewest()
        {

            var database = _taskManagerDB.SelectGuidMsgNewest(DBName);
            var listGuidMsgNewest = database?.DataSet?.Tables[0].ToObservableCollection<GuidMsgNewest>();
            if (listGuidMsgNewest == null)
                return;
            this.GuidMsgNewest = listGuidMsgNewest.FirstOrDefault();
        }
        void SelectAllMessageDetaiNewestlForPrinter()
        {
            foreach (var printer in PRINTERList)
            {
                object[] customdata = new object[]
                {
                    DateNow,
                    printer.GuidMsgNewest
                };
                var database = _taskManagerDB.SelectMessageDetailByGuidAndDateNow(DBName, customdata);
                var listMessageDetailModel = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();
                printer.MessagesDetailModel = listMessageDetailModel.FirstOrDefault();
            }
        }
        public void SelectMessageDetaiNewestlForPrinter(PRINTER printer)
        {
            object[] customdata = new object[]
            {
                    DateNow,
                    printer.GuidMsgNewest
            };
            var database = _taskManagerDB.SelectMessageDetailByGuidAndDateNow(DBName, customdata);
            var listMessageDetailModel = database?.DataSet?.Tables[0].ToObservableCollection<MessagesDetailModel>();
            printer.MessagesDetailModel = listMessageDetailModel.FirstOrDefault();

        }
        public void UpdateGuidMsgNewest()
        {
            object[] customdata = new object[]
            {
                this.PRINTERSelected.GuidMsgNewestName,
                this.PRINTERSelected.MessagesDetailModel.Guid
            };
            this._taskManagerDB.UpdateGuidMsgNewest(DBName, customdata);
        }
        public IPPrinterList DeserializePrinterXML()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(IPPrinterList));
                using (var reader = new StreamReader(Define.IPPrinterPath))
                {
                    var config = (IPPrinterList)serializer.Deserialize(reader);
                    return config;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private void AssignIPForPrinters()
        {
            if (IPPrinterList.IPAddressPrinterList.Count == PRINTERList.Count)
            {
                int idxPrinter = 0;
                foreach (var printer in PRINTERList)
                {
                    printer.IPAddress = IPPrinterList.IPAddressPrinterList[idxPrinter];
                    idxPrinter++;
                }
            }
        }
        void AddPRINTERList()
        {
            this._printerList = new List<PRINTER>();

            this.PRINTERList.Add(PRINTER1_LINE1);
            this.PRINTERList.Add(PRINTER2_LINE1);

            this.PRINTERList.Add(PRINTER1_LINE2);
            this.PRINTERList.Add(PRINTER2_LINE2);
            this.PRINTERList.Add(PRINTER3_LINE2);
            this.PRINTERList.Add(PRINTER4_LINE2);
            this.PRINTERList.Add(PRINTER5_LINE2);
            this.PRINTERList.Add(PRINTER6_LINE2);

            this.PRINTERList.Add(PRINTER1_LINE3);
            this.PRINTERList.Add(PRINTER2_LINE3);
            this.PRINTERList.Add(PRINTER3_LINE3);
            this.PRINTERList.Add(PRINTER4_LINE3);
            this.PRINTERList.Add(PRINTER5_LINE3);
            this.PRINTERList.Add(PRINTER6_LINE3);
        }

        //void AddPrintersName()
        //{
        //    this._printersName = new List<string>();

        //    this.PRINTERSName.Add(PRINTER1_LINE1.PrinterName);
        //    this.PRINTERSName.Add(PRINTER2_LINE1.PrinterName);

        //    this.PRINTERSName.Add(PRINTER1_LINE2.PrinterName);
        //    this.PRINTERSName.Add(PRINTER2_LINE2.PrinterName);
        //    this.PRINTERSName.Add(PRINTER3_LINE2.PrinterName);
        //    this.PRINTERSName.Add(PRINTER4_LINE2.PrinterName);
        //    this.PRINTERSName.Add(PRINTER5_LINE2.PrinterName);
        //    this.PRINTERSName.Add(PRINTER6_LINE2.PrinterName);

        //    this.PRINTERSName.Add(PRINTER1_LINE3.PrinterName);
        //    this.PRINTERSName.Add(PRINTER2_LINE3.PrinterName);
        //    this.PRINTERSName.Add(PRINTER3_LINE3.PrinterName);
        //    this.PRINTERSName.Add(PRINTER4_LINE3.PrinterName);
        //    this.PRINTERSName.Add(PRINTER5_LINE3.PrinterName);
        //    this.PRINTERSName.Add(PRINTER6_LINE3.PrinterName);
        //}
        void AddPrinterInLine()
        {
            this.LineViewModel.LINE1.Printers.Add(PRINTER1_LINE1);
            this.LineViewModel.LINE1.Printers.Add(PRINTER2_LINE1);

            this.LineViewModel.LINE2.Printers.Add(PRINTER1_LINE2);
            this.LineViewModel.LINE2.Printers.Add(PRINTER2_LINE2);
            this.LineViewModel.LINE2.Printers.Add(PRINTER3_LINE2);
            this.LineViewModel.LINE2.Printers.Add(PRINTER4_LINE2);
            this.LineViewModel.LINE2.Printers.Add(PRINTER5_LINE2);
            this.LineViewModel.LINE2.Printers.Add(PRINTER6_LINE2);

            this.LineViewModel.LINE3.Printers.Add(PRINTER1_LINE3);
            this.LineViewModel.LINE3.Printers.Add(PRINTER2_LINE3);
            this.LineViewModel.LINE3.Printers.Add(PRINTER3_LINE3);
            this.LineViewModel.LINE3.Printers.Add(PRINTER4_LINE3);
            this.LineViewModel.LINE3.Printers.Add(PRINTER5_LINE3);
            this.LineViewModel.LINE3.Printers.Add(PRINTER6_LINE3);
        }
        private readonly bool dataReceiveIsUTF8 = false;
        public void ConnectPrinter(PRINTER printer)
        {

            if (printer != null)
            {
                printer.PRINTERClientSocket.ClientConnect(dataReceiveIsUTF8);
            }
            else
            {
                MessageBox.Show("Chưa chọn máy in để kết nối!");
            }

        }
        public void DisconnectPrinter(PRINTER printer)
        {
            if (printer != null)
            {
                printer.PRINTERClientSocket.ClientDisconnect();
            }
        }

        /// <summary>
        /// Các bước thực hiện
        /// </summary>
        /// <param name="printer"></param>
        /// <param name="nameStep"></param>
        public void StepsPerform(PRINTER printer, STEPS_PERFORM nameStep)
        {
            if (printer == null)
                return;
            switch (nameStep)
            {
                case STEPS_PERFORM.CONNECT_PRINTER:
                    if (printer.PRINTER_STATES == PRINTER_STATES.DISCONNECTED)
                    {
                        this.ConnectPrinter(printer);
                    }
                    else if (printer.PRINTER_STATES == PRINTER_STATES.CONNECTED)
                    {
                        this.DisconnectPrinter(printer);
                    }
                    break;
                case STEPS_PERFORM.DISCONNECT_PRINTER:
                    if (printer.PRINTER_STATES != PRINTER_STATES.PRINTING)
                        this.DisconnectPrinter(printer);
                    break;
                case STEPS_PERFORM.REQUEST_PRINTER_STATUS:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.CmdRequestPrinterStatus);
                    break;
                case STEPS_PERFORM.REQUEST_MESSAGE:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.CmdRequestMessage);
                    break;
                case STEPS_PERFORM.DOWNLOAD_REMOTE_FIELD_DATA:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.DownloadRemoteFieldData(printer.TextInfoSendToPrinter));
                    break;
                case STEPS_PERFORM.DOWNLOAD_MESSAGE_DATA:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.DownloadMessageData(printer.MessageName, printer.TextInfoSendToPrinter));
                    break;
                case STEPS_PERFORM.DELETE_MESSAGE_DATA:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.DeleteMessageData(printer.MessageName));
                    break;
                case STEPS_PERFORM.LOAD_PRINT_MESSAGE:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.LoadPrintMessage(printer.MessageName));
                    break;
                case STEPS_PERFORM.SET_MESSAGE_PRINT_COUNT:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.CmdSetMessagePrintCount);
                    break;
                case STEPS_PERFORM.START_JET:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.CmdStartJet);
                    break;
                case STEPS_PERFORM.START_PRINT:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.CmdStartPrint);
                    break;
                case STEPS_PERFORM.REQUEST_MESSAGE_PRINT_COUNT:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.CmdRequestMessagePrintCount);
                    break;
                case STEPS_PERFORM.STOP_PRINT:
                    printer.PRINTERClientSocket.SendMsg(CommandPrinter.CmdStopPrint);
                    break;
                default:
                    break;
            }
        }

        void AssignMsgParam(PRINTER printer, XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "LenghtMsgInRaster":
                        printer.LenghtMsgInRaster = byte.Parse(node.InnerText);
                        break;
                    case "HeightMsg":
                        printer.HeightMsg = byte.Parse(node.InnerText);
                        break;
                    case "WidthMsg":
                        printer.WidthMsg = byte.Parse(node.InnerText);
                        break;
                    case "Delay":
                        printer.Delay = byte.Parse(node.InnerText);
                        break;
                    case "FieldOrientation":
                        printer.FieldOrientation = byte.Parse(node.InnerText);
                        switch (printer.FieldOrientation)
                        {
                            case 0:
                                printer.StrFieldOrientation = "Bình thường";
                                break;
                            case 4:
                                printer.StrFieldOrientation = "Lật ngang";
                                break;
                            case 8:
                                printer.StrFieldOrientation = "Lật dọc";
                                break;
                            case 12:
                                printer.StrFieldOrientation = "Lật ngang + dọc";
                                break;
                        }
                        break;
                }
            }
        }
        private void LoadMsgParam()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Define.MsgParamPath);

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "L1P1":
                            this.AssignMsgParam(this.PRINTER1_LINE1, node);
                            break;
                        case "L1P2":
                            this.AssignMsgParam(this.PRINTER2_LINE1, node);
                            break;
                        case "L2P1":
                            this.AssignMsgParam(this.PRINTER1_LINE2, node);
                            break;
                        case "L2P2":
                            this.AssignMsgParam(this.PRINTER2_LINE2, node);
                            break;
                        case "L2P3":
                            this.AssignMsgParam(this.PRINTER3_LINE2, node);
                            break;
                        case "L2P4":
                            this.AssignMsgParam(this.PRINTER4_LINE2, node);
                            break;
                        case "L2P5":
                            this.AssignMsgParam(this.PRINTER5_LINE2, node);
                            break;
                        case "L2P6":
                            this.AssignMsgParam(this.PRINTER6_LINE2, node);
                            break;
                        case "L3P1":
                            this.AssignMsgParam(this.PRINTER1_LINE3, node);
                            break;
                        case "L3P2":
                            this.AssignMsgParam(this.PRINTER2_LINE3, node);
                            break;
                        case "L3P3":
                            this.AssignMsgParam(this.PRINTER3_LINE3, node);
                            break;
                        case "L3P4":
                            this.AssignMsgParam(this.PRINTER4_LINE3, node);
                            break;
                        case "L3P5":
                            this.AssignMsgParam(this.PRINTER5_LINE3, node);
                            break;
                        case "L3P6":
                            this.AssignMsgParam(this.PRINTER6_LINE3, node);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SaveMsgParam(byte lenghtMsgInRaster, byte heightMsg, byte widthMsg, byte delay, byte fieldOrientation)
        {
            if (this.PRINTERSelected == null)
                return;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Define.MsgParamPath);

                doc.SelectSingleNode($"//{this.PRINTERSelected.StrTag}/LenghtMsgInRaster").InnerText = lenghtMsgInRaster.ToString();
                doc.SelectSingleNode($"//{this.PRINTERSelected.StrTag}/HeightMsg").InnerText = heightMsg.ToString();
                doc.SelectSingleNode($"//{this.PRINTERSelected.StrTag}/WidthMsg").InnerText = widthMsg.ToString();
                doc.SelectSingleNode($"//{this.PRINTERSelected.StrTag}/Delay").InnerText = delay.ToString();
                doc.SelectSingleNode($"//{this.PRINTERSelected.StrTag}/FieldOrientation").InnerText = fieldOrientation.ToString();


                doc.Save(Define.MsgParamPath);

                MaterialMessageBox.Show("Lưu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ConnectionEvent - ErrorEvent Callback - Methods Printer
        private async Task PRINTER_ConnectionEventCallbackHandler(NClientSocket.EConnectionEventClient e, object obj, PRINTER printer)
        {
            switch (e)
            {
                case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                    if (obj is bool) return;
                    char[] chs = (char[])obj;
                    if (chs.Length == 0 || chs.Length < 5) return;
                    char commandIdPrinter = chs[4];
                    switch (commandIdPrinter)
                    {
                        case (char)CommandsIDPrinter.RequestMessagePrintCount:

                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {
                                // code
                                if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                {
                                    await Task.Factory.StartNew(async () =>
                                    {
                                        if (printer.MessagesDetailModel == null)
                                        {
                                            string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                              + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                            int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                            printer.MessagePrintCount = decValue;
                                            return;
                                        }
                                        if (printer.PRINTER_STATES == PRINTER_STATES.PRINTING)
                                        {
                                            string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                              + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                            int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                            printer.MessagePrintCount = decValue;
                                            await Task.Delay(2);
                                        }
                                    });
                                }
                                else
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                printer.Stop();
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} không thể đọc được số đếm của bản tin", Define.SolidColorFail);
                            }
                            break;
#if !DEBUG
                            case (char)CommandsIDPrinter.RequestTotalPrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                          + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                        int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code

                                }
                                break;
                            case (char)CommandsIDPrinter.SetMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        if (this.PRINTER1_LINE3.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                }
                                break;
                            case (char)CommandsIDPrinter.DownloadRemoteFieldData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    this.PRINTER1_LINE3.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                    this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.SET_MESSAGE_PRINT_COUNT);
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin thành công";

                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Nạp bản tin thành công", Define.SolidColorOK);
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]";
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]", Define.SolidColorFail);
                                }
                                break;
#endif

                        case (char)CommandsIDPrinter.RequestPrinterStatus:
                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {
                                // code
                                if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                {
                                    printer.PrinterStatusDescription = String.Empty;
                                    printer.PrinterStatusDescription += JetStateList.ToDescription(chs[5]);
                                    printer.PrinterStatusDescription += PrintState.ToDescription(chs[6]);
                                    printer.PrinterStatusDescription += ErrorMask.ToDescription(new char[] { chs[8], chs[9], chs[10], chs[11], });


                                    if (chs[5] == (char)0x00 && chs[6] == (char)0x04)
                                    {
                                        printer.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                        printer.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                        this.SelectMessageDetaiNewestlForPrinter(printer);
                                        this.PRINTERSelected = printer;

                                        if (printer.MessagesDetailModel != null)
                                        {
                                            if (printer.MessagesDetailModel.NumberBagsPrinted < printer.MessagesDetailModel.NumberBags)
                                            {
                                                if (!printer.MessagesDetailModel.Trough.Equals(""))
                                                {

                                                    TroughViewModel.Instance.TROUGHList.Where(x => x.TroughName.Equals(printer.MessagesDetailModel.Trough) && x.PrinterLead == printer)
                                                                                       .ToList().ForEach((x) => { printer.TroughByUse = x; x.IsSelected = true; x.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE; });
                                                    TroughViewModel.Instance.TROUGHSelected = printer.TroughByUse;
                                                }
                                                if (MaterialMessageBox.ShowWithCancel($"Bạn có muốn tiếp tục in bản tin của khách hàng {printer.MessagesDetailModel.CustomerName} không?", "THÔNG BÁO") == MessageBoxResult.OK)
                                                {
                                                    printer.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                    MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(printer.MessagesDetailModel);
                                                    MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                    MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} đang in", Define.SolidColorOK);

                                                    printer.NumberBag = (printer.MessagesDetailModel.NumberBags);
                                                    printer.MessagePrintCountShow = printer.NumberBag;
                                                    printer.Start(500);
                                                    return;
                                                }
                                            }
                                        }

                                    }

                                    bool isCanPrint = (chs[5] == (char)0x00 && chs[6] == (char)0x02);
                                    if (isCanPrint)
                                    {
                                        printer.PRINTER_STATES = PRINTER_STATES.IS_CAN_PRINT;
                                        if (printer.TroughByUse != null)
                                        {
                                            printer.TroughByUse.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                            //Add-new [22-11-2022]
                                            TROUGH tr = TroughViewModel.Instance.TROUGHList.Where(x => x.Tag == printer.TroughByUse.Tag &&
                                                                                                     x.PrinterLead != printer.TroughByUse.PrinterLead)
                                                                                           .FirstOrDefault();
                                            if (tr != null)
                                                tr.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                        }
                                        this.StepsPerform(printer, STEPS_PERFORM.REQUEST_MESSAGE);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Đã bật tia thành công", Define.SolidColorOK);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Máy in có thể in", Define.SolidColorOK);
                                    }
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }

                                }
                                else
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Không thể đọc được trạng thái {printer.PrinterName}", Define.SolidColorFail);
                            }

                            break;
                        case (char)CommandsIDPrinter.StartJetID:
                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {
                                // code
                                if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} Bắt đầu khởi động tia mực...", Define.SolidColorOK);
                                }
                                else
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                }
                                if (this.IsManual)
                                {
                                    MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                    this.IsManual = false;
                                    return;
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} bật tia thất bại", Define.SolidColorFail);
                            }

                            break;
                        case (char)CommandsIDPrinter.StartPrintID:
                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {
                                try
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        // code
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }

                                        if (printer.PRINTER_STATES != PRINTER_STATES.PRINT_STOP)
                                        {
                                            this.UpdateGuidMsgNewest();
                                            MainViewModel.Instance.UpdateMessageDetail();
                                            MainViewModel.Instance.SendMsgToServer("Prt");
#if !LINE2
                                            MainViewModel.Instance.SendMsgToLine3("Prt");
#endif
#if LINE3
                                            MainViewModel.Instance.SendMsgToLine2("Prt");
#endif
                                            MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                            MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                        }
                                        else
                                        {
                                            if (printer.MessagesDetailModel != null)
                                            {
                                                printer.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(printer.MessagesDetailModel);
                                            }
                                            MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                            MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                        }
                                        printer.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                        printer.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} bật in thành công", Define.SolidColorOK);

                                        printer.Start(500);
                                    }
                                    else
                                    {
                                        printer.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} bật in thất bại", Define.SolidColorFail);
                            }

                            break;
                        case (char)CommandsIDPrinter.StopPrintID:
                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {
                                // code
                                if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                {
                                    // code
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }

                                    printer.PRINTER_STATES = PRINTER_STATES.PRINT_STOP; //add new 22-11-2022
                                    printer.Stop();
                                    if (printer.MessagesDetailModel != null)
                                    {
                                        printer.MessagesDetailModel.NumberBagsPrinted += printer.MessagePrintCount;
                                        printer.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.NOT_PRINT_DONE);
                                        MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(printer.MessagesDetailModel);
                                    }
                                    MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                    MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);

                                    //Kiểm tra số lượng đã in và hiển thị bảng chỉnh sửa - 22/11/2022
                                    if (printer.MessagesDetailModel.NumberBagsPrinted > printer.MessagesDetailModel.NumberBags ||
                                       printer.MessagesDetailModel.NumberBagsPrinted < printer.MessagesDetailModel.NumberBags)
                                    {
                                        MainViewModel.Instance.EditMessagePrintedViewModel.MessagesDetailModel = (MessagesDetailModel)printer.MessagesDetailModel.Clone();
                                        EditMessagePrintedView editMessagePrintedView = new EditMessagePrintedView();
                                        EditMessagePrintedViewModel.Instance.EditMessagePrintedView = editMessagePrintedView;
                                        EditMessagePrintedViewModel.Instance.EditMessagePrintedView.Title = $"{printer.PrinterName} - Bản tin khách hàng {printer.MessagesDetailModel.CustomerName}";
                                        EditMessagePrintedViewModel.Instance.EditMessagePrintedView.ShowDialog();
                                    }

                                    //Hiển thị bảng LED sau khi ấn DỪNG in - 22/11/2022
                                    await TroughViewModel.Instance.SetTextToAllLEDPanel(printer, printer.LEDTableFixed, printer.TroughByUse,
                                                                                        true,
                                                                                        printer.PrinterName,
                                                                                        printer.TroughByUse?.TroughName,
                                                                                        "HT");
                                    this.StepsPerform(printer, STEPS_PERFORM.REQUEST_PRINTER_STATUS);

                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} đã DỪNG IN", Define.SolidColorOK);

                                }
                                else
                                {
                                    printer.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} tắt in thất bại", Define.SolidColorFail);
                            }

                            break;
                        case (char)CommandsIDPrinter.RequestMessage:
                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {
                                // code
                                if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                {
                                    char[] msg = new char[chs.Length - 10];
                                    Array.Copy(chs, 5, msg, 0, msg.Length);
                                    String s = new String(msg);
                                    printer.CurrentMessageOnPrinter = s;
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Bản tin hiện tại trên máy in là {s}", Define.SolidColorOK);
                                    if (printer.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                    {
                                        this.StepsPerform(printer, STEPS_PERFORM.START_PRINT);
                                    }
                                }
                                else
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} yêu cầu lấy tên bản tin thất bại", Define.SolidColorFail);
                            }

                            break;



                        case (char)CommandsIDPrinter.DownloadMessageData:
                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {

                                // code
                                if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} TẠO bản tin thành công", Define.SolidColorOK);
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                    this.StepsPerform(printer, STEPS_PERFORM.LOAD_PRINT_MESSAGE);
                                }
                                else
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} TẠO bản tin thất bại [Lỗi máy in: {PrinterFaultList.ToDescription(chs[2])}  Lỗi command: {CommandFaultCodeList.ToDescription(chs[3])}]", Define.SolidColorFail);
                            }
                            break;
                        case (char)CommandsIDPrinter.LoadPrintMessage:
                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {
                                // code

                                if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} TẢI bản tin thành công", Define.SolidColorOK);
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                    printer.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                    this.StepsPerform(printer, STEPS_PERFORM.REQUEST_MESSAGE);

                                }
                                else
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} TẢI bản tin thất bại", Define.SolidColorFail);
                            }
                            break;
                        case (char)CommandsIDPrinter.DeleteMessageData:
                            if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                            {
                                // code
                                if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} XÓA bản tin thành công", Define.SolidColorOK);
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                    this.StepsPerform(printer, STEPS_PERFORM.DOWNLOAD_MESSAGE_DATA);
                                }
                                else
                                {
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                }
                            }
                            else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                            {
                                // code
                                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{printer.PrinterName} XÓA bản tin thất bại", Define.SolidColorFail);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                    printer.PRINTER_STATES = PRINTER_STATES.CONNECTED;
                    this.StepsPerform(printer, STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                    printer.NotifyMsgOfPrinter = $"Đã kết nối {PRINTERSelected.PrinterName} thành công";
                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã kết nối {PRINTERSelected.PrinterName} thành công", Define.SolidColorOK);
                    break;
                case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                    printer.PRINTER_STATES = PRINTER_STATES.DISCONNECTED;
                    printer.CurrentMessageOnPrinter = "[...]";
                    printer.PrinterStatusDescription = "[...]";
                    printer.NotifyMsgOfPrinter = "[...]";
                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã ngắt kết nối với {PRINTERSelected.PrinterName}", Define.SolidColorFail);

                    break;
                default:
                    break;
            }
        }
        private void CreatePrinterClientSocketAndRiseEvent()
        {

#if !LINE1
            PRINTER1_LINE1.PRINTERClientSocket = new NClientSocket(PRINTER1_LINE1.IPAddress, PRINTER1_LINE1.PORT_PRINTER);
            PRINTER2_LINE1.PRINTERClientSocket = new NClientSocket(PRINTER2_LINE1.IPAddress, PRINTER2_LINE1.PORT_PRINTER);
            PRINTER1_LINE1.PRINTERClientSocket.ConnectionEventCallback += PRINTER1_LINE1_ConnectionEventCallback;
            PRINTER2_LINE1.PRINTERClientSocket.ConnectionEventCallback += PRINTER2_LINE1_ConnectionEventCallback;
            PRINTER1_LINE1.PRINTERClientSocket.ClientErrorEventCallback += PRINTER1_LINE1_ClientErrorEventCallback;
            PRINTER2_LINE1.PRINTERClientSocket.ClientErrorEventCallback += PRINTER2_LINE1_ClientErrorEventCallback;
#endif

#if !LINE2
            PRINTER1_LINE2.PRINTERClientSocket = new NClientSocket(PRINTER1_LINE2.IPAddress, PRINTER1_LINE2.PORT_PRINTER);
            PRINTER2_LINE2.PRINTERClientSocket = new NClientSocket(PRINTER2_LINE2.IPAddress, PRINTER2_LINE2.PORT_PRINTER);
            PRINTER3_LINE2.PRINTERClientSocket = new NClientSocket(PRINTER3_LINE2.IPAddress, PRINTER3_LINE2.PORT_PRINTER);
            PRINTER4_LINE2.PRINTERClientSocket = new NClientSocket(PRINTER4_LINE2.IPAddress, PRINTER4_LINE2.PORT_PRINTER);
            PRINTER5_LINE2.PRINTERClientSocket = new NClientSocket(PRINTER5_LINE2.IPAddress, PRINTER5_LINE2.PORT_PRINTER);
            PRINTER6_LINE2.PRINTERClientSocket = new NClientSocket(PRINTER6_LINE2.IPAddress, PRINTER6_LINE2.PORT_PRINTER);

            PRINTER1_LINE2.PRINTERClientSocket.ConnectionEventCallback += PRINTER1_LINE2_ConnectionEventCallback;
            PRINTER2_LINE2.PRINTERClientSocket.ConnectionEventCallback += PRINTER2_LINE2_ConnectionEventCallback;
            PRINTER3_LINE2.PRINTERClientSocket.ConnectionEventCallback += PRINTER3_LINE2_ConnectionEventCallback;
            PRINTER4_LINE2.PRINTERClientSocket.ConnectionEventCallback += PRINTER4_LINE2_ConnectionEventCallback;
            PRINTER5_LINE2.PRINTERClientSocket.ConnectionEventCallback += PRINTER5_LINE2_ConnectionEventCallback;
            PRINTER6_LINE2.PRINTERClientSocket.ConnectionEventCallback += PRINTER6_LINE2_ConnectionEventCallback;
            PRINTER1_LINE2.PRINTERClientSocket.ClientErrorEventCallback += PRINTER1_LINE2_ClientErrorEventCallback;
            PRINTER2_LINE2.PRINTERClientSocket.ClientErrorEventCallback += PRINTER2_LINE2_ClientErrorEventCallback;
            PRINTER3_LINE2.PRINTERClientSocket.ClientErrorEventCallback += PRINTER3_LINE2_ClientErrorEventCallback;
            PRINTER4_LINE2.PRINTERClientSocket.ClientErrorEventCallback += PRINTER4_LINE2_ClientErrorEventCallback;
            PRINTER5_LINE2.PRINTERClientSocket.ClientErrorEventCallback += PRINTER5_LINE2_ClientErrorEventCallback;
            PRINTER6_LINE2.PRINTERClientSocket.ClientErrorEventCallback += PRINTER6_LINE2_ClientErrorEventCallback;
#endif

#if LINE3
            PRINTER1_LINE3.PRINTERClientSocket = new NClientSocket(PRINTER1_LINE3.IPAddress, PRINTER1_LINE3.PORT_PRINTER);
            PRINTER2_LINE3.PRINTERClientSocket = new NClientSocket(PRINTER2_LINE3.IPAddress, PRINTER2_LINE3.PORT_PRINTER);
            PRINTER3_LINE3.PRINTERClientSocket = new NClientSocket(PRINTER3_LINE3.IPAddress, PRINTER3_LINE3.PORT_PRINTER);
            PRINTER4_LINE3.PRINTERClientSocket = new NClientSocket(PRINTER4_LINE3.IPAddress, PRINTER4_LINE3.PORT_PRINTER);
            PRINTER5_LINE3.PRINTERClientSocket = new NClientSocket(PRINTER5_LINE3.IPAddress, PRINTER5_LINE3.PORT_PRINTER);
            PRINTER6_LINE3.PRINTERClientSocket = new NClientSocket(PRINTER6_LINE3.IPAddress, PRINTER6_LINE3.PORT_PRINTER);


            PRINTER1_LINE3.PRINTERClientSocket.ConnectionEventCallback += PRINTER1_LINE3_ConnectionEventCallback;
            PRINTER2_LINE3.PRINTERClientSocket.ConnectionEventCallback += PRINTER2_LINE3_ConnectionEventCallback;
            PRINTER3_LINE3.PRINTERClientSocket.ConnectionEventCallback += PRINTER3_LINE3_ConnectionEventCallback;
            PRINTER4_LINE3.PRINTERClientSocket.ConnectionEventCallback += PRINTER4_LINE3_ConnectionEventCallback;
            PRINTER5_LINE3.PRINTERClientSocket.ConnectionEventCallback += PRINTER5_LINE3_ConnectionEventCallback;
            PRINTER6_LINE3.PRINTERClientSocket.ConnectionEventCallback += PRINTER6_LINE3_ConnectionEventCallback;
            PRINTER1_LINE3.PRINTERClientSocket.ClientErrorEventCallback += PRINTER1_LINE3_ClientErrorEventCallback;
            PRINTER2_LINE3.PRINTERClientSocket.ClientErrorEventCallback += PRINTER2_LINE3_ClientErrorEventCallback;
            PRINTER3_LINE3.PRINTERClientSocket.ClientErrorEventCallback += PRINTER3_LINE3_ClientErrorEventCallback;
            PRINTER4_LINE3.PRINTERClientSocket.ClientErrorEventCallback += PRINTER4_LINE3_ClientErrorEventCallback;
            PRINTER5_LINE3.PRINTERClientSocket.ClientErrorEventCallback += PRINTER5_LINE3_ClientErrorEventCallback;
            PRINTER6_LINE3.PRINTERClientSocket.ClientErrorEventCallback += PRINTER6_LINE3_ClientErrorEventCallback;
#endif
        }

        #region ConnectionEventCallback
        

#if !LINE1
        private void PRINTER1_LINE1_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {

        }
        private void PRINTER2_LINE1_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {

        }
#endif

#if !LINE2
        private async void PRINTER1_LINE2_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER1_LINE2);
#endif

#if !SHORTCUT
                switch (e)
                {
                    case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                        if (obj is bool) return;
                        char[] chs = (char[])obj;
                        if (chs.Length == 0 || chs.Length < 5) return;
                        char commandIdPrinter = chs[4];
                        switch (commandIdPrinter)
                        {
                            case (char)CommandsIDPrinter.RequestMessagePrintCount:

                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        await Task.Factory.StartNew(async () =>
                                        {
                                            if (this.PRINTER1_LINE2.MessagesDetailModel == null)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER1_LINE2.MessagePrintCount = decValue;
                                                return;
                                            }
                                            if (this.PRINTER1_LINE2.PRINTER_STATES == PRINTER_STATES.PRINTING)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER1_LINE2.MessagePrintCount = decValue;
                                                await Task.Delay(2);
                                            }
                                        });
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER1_LINE2.Stop();
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} không thể đọc được số đếm của bản tin", Define.SolidColorFail);
                                }
                                break;
#if !DEBUG
                                            case (char)CommandsIDPrinter.RequestTotalPrintCount:
                                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                                {
                                                    // code
                                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                                    {
                                                        string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                          + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                        int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                    }
                                                }
                                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                                {
                                                    // code

                                                }
                                                break;
                                            case (char)CommandsIDPrinter.SetMessagePrintCount:
                                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                                {
                                                    // code
                                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                                    {
                                                        if (this.PRINTER1_LINE3.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                                        {
                                                            this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.START_PRINT);
                                                        }
                                                    }
                                                }
                                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                                {
                                                    // code
                                                }
                                                break;
                                            case (char)CommandsIDPrinter.DownloadRemoteFieldData:
                                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                                {
                                                    // code

                                                    this.PRINTER1_LINE3.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                                    this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.SET_MESSAGE_PRINT_COUNT);
                                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin thành công";

                                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Nạp bản tin thành công", Define.SolidColorOK);
                                                }
                                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                                {
                                                    // code
                                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]";
                                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]", Define.SolidColorFail);
                                                }
                                                break;
#endif

                            case (char)CommandsIDPrinter.RequestPrinterStatus:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        this.PRINTER1_LINE2.PrinterStatusDescription = String.Empty;
                                        this.PRINTER1_LINE2.PrinterStatusDescription += JetStateList.ToDescription(chs[5]);
                                        this.PRINTER1_LINE2.PrinterStatusDescription += PrintState.ToDescription(chs[6]);
                                        this.PRINTER1_LINE2.PrinterStatusDescription += ErrorMask.ToDescription(new char[] { chs[8], chs[9], chs[10], chs[11], });


                                        if (chs[5] == (char)0x00 && chs[6] == (char)0x04)
                                        {
                                            this.PRINTER1_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER1_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            this.SelectMessageDetaiNewestlForPrinter(this.PRINTER1_LINE2);
                                            this.PRINTERSelected = this.PRINTER1_LINE2;

                                            if (this.PRINTER1_LINE2.MessagesDetailModel != null)
                                            {
                                                if (this.PRINTER1_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER1_LINE2.MessagesDetailModel.NumberBags)
                                                {
                                                    if (!this.PRINTER1_LINE2.MessagesDetailModel.Trough.Equals(""))
                                                    {

                                                        TroughViewModel.Instance.TROUGHList.Where(x => x.TroughName.Equals(this.PRINTER1_LINE2.MessagesDetailModel.Trough) && x.PrinterLead == this.PRINTER1_LINE2)
                                                                                           .ToList().ForEach((x) => { this.PRINTER1_LINE2.TroughByUse = x; x.IsSelected = true; x.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE; });
                                                        TroughViewModel.Instance.TROUGHSelected = this.PRINTER1_LINE2.TroughByUse;
                                                    }
                                                    if (MaterialMessageBox.ShowWithCancel($"Bạn có muốn tiếp tục in bản tin của khách hàng {this.PRINTER1_LINE2.MessagesDetailModel.CustomerName} không?", "THÔNG BÁO") == MessageBoxResult.OK)
                                                    {
                                                        this.PRINTER1_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                        MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER1_LINE2.MessagesDetailModel);
                                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} đang in", Define.SolidColorOK);

                                                        this.PRINTER1_LINE2.NumberBag = (this.PRINTER1_LINE2.MessagesDetailModel.NumberBags);
                                                        this.PRINTER1_LINE2.MessagePrintCountShow = this.PRINTER1_LINE2.NumberBag;
                                                        this.PRINTER1_LINE2.Start(500);
                                                        return;
                                                    }
                                                }
                                            }

                                        }

                                        bool isCanPrint = (chs[5] == (char)0x00 && chs[6] == (char)0x02);
                                        if (isCanPrint)
                                        {
                                            this.PRINTER1_LINE2.PRINTER_STATES = PRINTER_STATES.IS_CAN_PRINT;
                                            if (this.PRINTER1_LINE2.TroughByUse != null)
                                            {
                                                this.PRINTER1_LINE2.TroughByUse.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                                //Add-new [22-11-2022]
                                                TROUGH tr = TroughViewModel.Instance.TROUGHList.Where(x => x.Tag == this.PRINTER1_LINE2.TroughByUse.Tag &&
                                                                                                         x.PrinterLead != this.PRINTER1_LINE2.TroughByUse.PrinterLead)
                                                                                               .FirstOrDefault();
                                                if (tr != null)
                                                    tr.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                            }
                                            this.StepsPerform(PRINTER1_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Đã bật tia thành công", Define.SolidColorOK);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Máy in có thể in", Define.SolidColorOK);
                                        }
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Không thể đọc được trạng thái {this.PRINTER1_LINE2.PrinterName}", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartJetID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} Bắt đầu khởi động tia mực...", Define.SolidColorOK);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} bật tia thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    try
                                    {
                                        // code
                                        if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                        {
                                            // code
                                            if (this.IsManual)
                                            {
                                                MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                                this.IsManual = false;
                                                return;
                                            }

                                            if (this.PRINTER1_LINE2.PRINTER_STATES != PRINTER_STATES.PRINT_STOP)
                                            {
                                                this.UpdateGuidMsgNewest();
                                                MainViewModel.Instance.UpdateMessageDetail();
                                                MainViewModel.Instance.SendMsgToServer("Printing");
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            else
                                            {
                                                if (this.PRINTER1_LINE2.MessagesDetailModel != null)
                                                {
                                                    this.PRINTER1_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                    MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER1_LINE2.MessagesDetailModel);
                                                }
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            this.PRINTER1_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER1_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} bật in thành công", Define.SolidColorOK);

                                            this.PRINTER1_LINE2.Start(500);
                                        }
                                        else
                                        {
                                            this.PRINTER1_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} bật in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StopPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        // code
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }

                                        this.PRINTER1_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_STOP; //add new 22-11-2022
                                        this.PRINTER1_LINE2.Stop();
                                        if (this.PRINTER1_LINE2.MessagesDetailModel != null)
                                        {
                                            this.PRINTER1_LINE2.MessagesDetailModel.NumberBagsPrinted += this.PRINTER1_LINE2.MessagePrintCount;
                                            this.PRINTER1_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.NOT_PRINT_DONE);
                                            MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER1_LINE2.MessagesDetailModel);
                                        }
                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);

                                        //Kiểm tra số lượng đã in và hiển thị bảng chỉnh sửa - 22/11/2022
                                        if (this.PRINTER1_LINE2.MessagesDetailModel.NumberBagsPrinted > this.PRINTER1_LINE2.MessagesDetailModel.NumberBags ||
                                           this.PRINTER1_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER1_LINE2.MessagesDetailModel.NumberBags)
                                        {
                                            MainViewModel.Instance.EditMessagePrintedViewModel.MessagesDetailModel = (MessagesDetailModel)this.PRINTER1_LINE2.MessagesDetailModel.Clone();
                                            EditMessagePrintedView editMessagePrintedView = new EditMessagePrintedView();
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView = editMessagePrintedView;
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.Title = $"{this.PRINTER1_LINE2.PrinterName} - Bản tin khách hàng {this.PRINTER1_LINE2.MessagesDetailModel.CustomerName}";
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.ShowDialog();
                                        }

                                        //Hiển thị bảng LED sau khi ấn DỪNG in - 22/11/2022
                                        await TroughViewModel.Instance.SetTextToAllLEDPanel(this.PRINTER1_LINE2, this.PRINTER1_LINE2.LEDTableFixed, this.PRINTER1_LINE2.TroughByUse,
                                                                                            this.PRINTER1_LINE2.PrinterName,
                                                                                            this.PRINTER1_LINE2.TroughByUse?.TroughName,
                                                                                            "HT");
                                        this.StepsPerform(this.PRINTER1_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);

                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} đã DỪNG IN", Define.SolidColorOK);

                                    }
                                    else
                                    {
                                        this.PRINTER1_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} tắt in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.RequestMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        char[] msg = new char[chs.Length - 10];
                                        Array.Copy(chs, 5, msg, 0, msg.Length);
                                        String s = new String(msg);
                                        this.PRINTER1_LINE2.CurrentMessageOnPrinter = s;
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Bản tin hiện tại trên máy in là {s}", Define.SolidColorOK);
                                        if (this.PRINTER1_LINE2.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(this.PRINTER1_LINE2, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} yêu cầu lấy tên bản tin thất bại", Define.SolidColorFail);
                                }

                                break;



                            case (char)CommandsIDPrinter.DownloadMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {

                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} TẠO bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER1_LINE2, STEPS_PERFORM.LOAD_PRINT_MESSAGE);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} TẠO bản tin thất bại [Lỗi máy in: {PrinterFaultList.ToDescription(chs[2])}  Lỗi command: {CommandFaultCodeList.ToDescription(chs[3])}]", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.LoadPrintMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} TẢI bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER1_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                        this.StepsPerform(PRINTER1_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} TẢI bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.DeleteMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} XÓA bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER1_LINE2, STEPS_PERFORM.DOWNLOAD_MESSAGE_DATA);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} XÓA bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                        this.PRINTER1_LINE2.PRINTER_STATES = PRINTER_STATES.CONNECTED;
                        this.StepsPerform(this.PRINTER1_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                        this.PRINTER1_LINE2.NotifyMsgOfPrinter = $"Đã kết nối {PRINTERSelected.PrinterName} thành công";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã kết nối {PRINTERSelected.PrinterName} thành công", Define.SolidColorOK);
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                        this.PRINTER1_LINE2.PRINTER_STATES = PRINTER_STATES.DISCONNECTED;
                        this.PRINTER1_LINE2.CurrentMessageOnPrinter = "[...]";
                        this.PRINTER1_LINE2.PrinterStatusDescription = "[...]";
                        this.PRINTER1_LINE2.NotifyMsgOfPrinter = "[...]";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã ngắt kết nối với {PRINTERSelected.PrinterName}", Define.SolidColorFail);

                        break;
                    default:
                        break;
                }
#endif

            }));
        }
        private async void PRINTER2_LINE2_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER2_LINE2);
#endif

#if !SHORTCUT
                switch (e)
                {
                    case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                        if (obj is bool) return;
                        char[] chs = (char[])obj;
                        if (chs.Length == 0 || chs.Length < 5) return;
                        char commandIdPrinter = chs[4];
                        switch (commandIdPrinter)
                        {
                            case (char)CommandsIDPrinter.RequestMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        await Task.Factory.StartNew(async () =>
                                        {
                                            if (this.PRINTER2_LINE2.MessagesDetailModel == null)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER2_LINE2.MessagePrintCount = decValue;
                                                return;
                                            }
                                            if (this.PRINTER2_LINE2.PRINTER_STATES == PRINTER_STATES.PRINTING)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER2_LINE2.MessagePrintCount = decValue;
                                                await Task.Delay(2);

                                            }
                                        });

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER2_LINE2.Stop();
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} không thể đọc được số đếm của bản tin", Define.SolidColorFail);
                                }
                                break;
#if !DEBUG
                            case (char)CommandsIDPrinter.RequestTotalPrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                          + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                        int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code

                                }
                                break;
                            case (char)CommandsIDPrinter.SetMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        if (this.PRINTER1_LINE3.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                }
                                break;
                            case (char)CommandsIDPrinter.DownloadRemoteFieldData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    this.PRINTER1_LINE3.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                    this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.SET_MESSAGE_PRINT_COUNT);
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin thành công";

                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Nạp bản tin thành công", Define.SolidColorOK);
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]";
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]", Define.SolidColorFail);
                                }
                                break;
#endif

                            case (char)CommandsIDPrinter.RequestPrinterStatus:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        this.PRINTER2_LINE2.PrinterStatusDescription = String.Empty;
                                        this.PRINTER2_LINE2.PrinterStatusDescription += JetStateList.ToDescription(chs[5]);
                                        this.PRINTER2_LINE2.PrinterStatusDescription += PrintState.ToDescription(chs[6]);
                                        this.PRINTER2_LINE2.PrinterStatusDescription += ErrorMask.ToDescription(new char[] { chs[8], chs[9], chs[10], chs[11], });

                                        //ADD-NEW
                                        if (chs[5] == (char)0x00 && chs[6] == (char)0x04)
                                        {
                                            this.PRINTER2_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER2_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            this.SelectMessageDetaiNewestlForPrinter(this.PRINTER2_LINE2);
                                            this.PRINTERSelected = this.PRINTER2_LINE2;

                                            if (this.PRINTER2_LINE2.MessagesDetailModel != null)
                                            {
                                                if (this.PRINTER2_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER2_LINE2.MessagesDetailModel.NumberBags)
                                                {
                                                    if (!this.PRINTER2_LINE2.MessagesDetailModel.Trough.Equals(""))
                                                    {
                                                        TroughViewModel.Instance.TROUGHList.Where(x => x.TroughName.Equals(this.PRINTER2_LINE2.MessagesDetailModel.Trough) && x.PrinterLead == this.PRINTER2_LINE2)
                                                                                           .ToList().ForEach((x) => { this.PRINTER2_LINE2.TroughByUse = x; x.IsSelected = true; x.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE; });
                                                        TroughViewModel.Instance.TROUGHSelected = this.PRINTER2_LINE2.TroughByUse;
                                                    }
                                                    if (MaterialMessageBox.ShowWithCancel($"Bạn có muốn tiếp tục in bản tin của khách hàng {this.PRINTER2_LINE2.MessagesDetailModel.CustomerName} không?", "THÔNG BÁO") == MessageBoxResult.OK)
                                                    {
                                                        this.PRINTER2_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                        MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER2_LINE2.MessagesDetailModel);
                                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} đang in", Define.SolidColorOK);

                                                        this.PRINTER2_LINE2.NumberBag = (this.PRINTER2_LINE2.MessagesDetailModel.NumberBags);
                                                        this.PRINTER2_LINE2.MessagePrintCountShow = this.PRINTER2_LINE2.NumberBag;
                                                        this.PRINTER2_LINE2.Start(500);
                                                        return;
                                                    }
                                                }
                                            }

                                        }
                                        bool isCanPrint = (chs[5] == (char)0x00 && chs[6] == (char)0x02);
                                        if (isCanPrint)
                                        {

                                            this.PRINTER2_LINE2.PRINTER_STATES = PRINTER_STATES.IS_CAN_PRINT;
                                            if (this.PRINTER2_LINE2.TroughByUse != null)
                                            {
                                                this.PRINTER2_LINE2.TroughByUse.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                                //Add-new [22-11-2022]
                                                TROUGH tr = TroughViewModel.Instance.TROUGHList.Where(x => x.Tag == this.PRINTER2_LINE2.TroughByUse.Tag &&
                                                                                                           x.PrinterLead != this.PRINTER2_LINE2.TroughByUse.PrinterLead)
                                                                                               .FirstOrDefault();
                                                if (tr != null)
                                                    tr.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                            }
                                            this.StepsPerform(PRINTER2_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Đã bật tia thành công", Define.SolidColorOK);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Máy in có thể in", Define.SolidColorOK);
                                        }
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Không thể đọc được trạng thái {this.PRINTER2_LINE2.PrinterName}", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartJetID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} Bắt đầu khởi động tia mực...", Define.SolidColorOK);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} bật tia thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    try
                                    {
                                        // code
                                        if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                        {
                                            // code
                                            if (this.IsManual)
                                            {
                                                MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                                this.IsManual = false;
                                                return;
                                            }
                                            if (this.PRINTER2_LINE2.PRINTER_STATES != PRINTER_STATES.PRINT_STOP)
                                            {
                                                this.UpdateGuidMsgNewest();
                                                MainViewModel.Instance.UpdateMessageDetail();
                                                MainViewModel.Instance.SendMsgToServer("Printing");
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            else
                                            {
                                                this.PRINTER2_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER2_LINE2.MessagesDetailModel);
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            this.PRINTER2_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER2_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} bật in thành công", Define.SolidColorOK);

                                            this.PRINTER2_LINE2.Start(500);
                                        }
                                        else
                                        {
                                            this.PRINTER2_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} bật in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StopPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        // code
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER2_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_STOP; //add new 22-11-2022

                                        this.PRINTER2_LINE2.Stop();
                                        if (this.PRINTER2_LINE2.MessagesDetailModel != null)
                                        {
                                            this.PRINTER2_LINE2.MessagesDetailModel.NumberBagsPrinted += this.PRINTER2_LINE2.MessagePrintCount;
                                            this.PRINTER2_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.NOT_PRINT_DONE);
                                            MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER2_LINE2.MessagesDetailModel);
                                        }
                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);

                                        //Kiểm tra số lượng đã in và hiển thị bảng chỉnh sửa - 22/11/2022
                                        if (this.PRINTER2_LINE2.MessagesDetailModel.NumberBagsPrinted > this.PRINTER2_LINE2.MessagesDetailModel.NumberBags ||
                                           this.PRINTER2_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER2_LINE2.MessagesDetailModel.NumberBags)
                                        {
                                            MainViewModel.Instance.EditMessagePrintedViewModel.MessagesDetailModel = (MessagesDetailModel)this.PRINTER2_LINE2.MessagesDetailModel.Clone();
                                            EditMessagePrintedView editMessagePrintedView = new EditMessagePrintedView();
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView = editMessagePrintedView;
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.Title = $"{this.PRINTER2_LINE2.PrinterName} - Bản tin khách hàng {this.PRINTER2_LINE2.MessagesDetailModel.CustomerName}";
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.ShowDialog();
                                        }

                                        //Hiển thị bảng LED sau khi ấn DỪNG in - 22/11/2022
                                        await TroughViewModel.Instance.SetTextToAllLEDPanel(this.PRINTER2_LINE2, this.PRINTER2_LINE2.LEDTableFixed, this.PRINTER2_LINE2.TroughByUse,
                                                                                            this.PRINTER2_LINE2.PrinterName,
                                                                                            this.PRINTER2_LINE2.TroughByUse?.TroughName,
                                                                                            "HT");
                                        this.StepsPerform(this.PRINTER2_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);

                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} tắt in thành công", Define.SolidColorOK);


                                    }
                                    else
                                    {
                                        this.PRINTER2_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} tắt in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.RequestMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        char[] msg = new char[chs.Length - 10];
                                        Array.Copy(chs, 5, msg, 0, msg.Length);
                                        String s = new String(msg);
                                        this.PRINTER2_LINE2.CurrentMessageOnPrinter = s;
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Bản tin hiện tại trên máy in là {s}", Define.SolidColorOK);
                                        if (this.PRINTER2_LINE2.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(this.PRINTER2_LINE2, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} yêu cầu lấy tên bản tin thất bại", Define.SolidColorFail);
                                }

                                break;



                            case (char)CommandsIDPrinter.DownloadMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {

                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} TẠO bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER2_LINE2, STEPS_PERFORM.LOAD_PRINT_MESSAGE);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} TẠO bản tin thất bại [Lỗi máy in: {PrinterFaultList.ToDescription(chs[2])}  Lỗi command: {CommandFaultCodeList.ToDescription(chs[3])}]", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.LoadPrintMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} TẢI bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER2_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                        this.StepsPerform(PRINTER2_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} TẢI bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.DeleteMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} XÓA bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER2_LINE2, STEPS_PERFORM.DOWNLOAD_MESSAGE_DATA);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} XÓA bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                        this.PRINTER2_LINE2.PRINTER_STATES = PRINTER_STATES.CONNECTED;
                        this.StepsPerform(this.PRINTER2_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                        this.PRINTER2_LINE2.NotifyMsgOfPrinter = $"Đã kết nối {PRINTERSelected.PrinterName} thành công";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã kết nối {PRINTERSelected.PrinterName} thành công", Define.SolidColorOK);
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                        this.PRINTER2_LINE2.PRINTER_STATES = PRINTER_STATES.DISCONNECTED;
                        this.PRINTER2_LINE2.CurrentMessageOnPrinter = "[...]";
                        this.PRINTER2_LINE2.PrinterStatusDescription = "[...]";
                        this.PRINTER2_LINE2.NotifyMsgOfPrinter = "[...]";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã ngắt kết nối với {PRINTERSelected.PrinterName}", Define.SolidColorFail);

                        break;
                    default:
                        break;
                }
#endif

            }));
        }
        private async void PRINTER3_LINE2_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER3_LINE2);
#endif

#if !SHORTCUT
                switch (e)
                {
                    case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                        if (obj is bool) return;
                        char[] chs = (char[])obj;
                        if (chs.Length == 0 || chs.Length < 5) return;
                        char commandIdPrinter = chs[4];
                        switch (commandIdPrinter)
                        {
                            case (char)CommandsIDPrinter.RequestMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        await Task.Factory.StartNew(async () =>
                                        {
                                            if (this.PRINTER3_LINE2.MessagesDetailModel == null)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER3_LINE2.MessagePrintCount = decValue;
                                                return;
                                            }
                                            if (this.PRINTER3_LINE2.PRINTER_STATES == PRINTER_STATES.PRINTING)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER3_LINE2.MessagePrintCount = decValue;
                                                await Task.Delay(2);
                                            }
                                        });
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER3_LINE2.Stop();
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} không thể đọc được số đếm của bản tin", Define.SolidColorFail);
                                }
                                break;
#if !DEBUG
                            case (char)CommandsIDPrinter.RequestTotalPrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                          + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                        int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code

                                }
                                break;
                            case (char)CommandsIDPrinter.SetMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        if (this.PRINTER1_LINE3.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                }
                                break;
                            case (char)CommandsIDPrinter.DownloadRemoteFieldData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    this.PRINTER1_LINE3.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                    this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.SET_MESSAGE_PRINT_COUNT);
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin thành công";

                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Nạp bản tin thành công", Define.SolidColorOK);
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]";
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]", Define.SolidColorFail);
                                }
                                break;
#endif

                            case (char)CommandsIDPrinter.RequestPrinterStatus:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        this.PRINTER3_LINE2.PrinterStatusDescription = String.Empty;
                                        this.PRINTER3_LINE2.PrinterStatusDescription += JetStateList.ToDescription(chs[5]);
                                        this.PRINTER3_LINE2.PrinterStatusDescription += PrintState.ToDescription(chs[6]);
                                        this.PRINTER3_LINE2.PrinterStatusDescription += ErrorMask.ToDescription(new char[] { chs[8], chs[9], chs[10], chs[11], });


                                        if (chs[5] == (char)0x00 && chs[6] == (char)0x04)
                                        {
                                            this.PRINTER3_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER3_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            this.SelectMessageDetaiNewestlForPrinter(this.PRINTER3_LINE2);
                                            this.PRINTERSelected = this.PRINTER3_LINE2;

                                            if (this.PRINTER3_LINE2.MessagesDetailModel != null)
                                            {
                                                if (this.PRINTER3_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER3_LINE2.MessagesDetailModel.NumberBags)
                                                {
                                                    if (!this.PRINTER3_LINE2.MessagesDetailModel.Trough.Equals(""))
                                                    {
                                                        TroughViewModel.Instance.TROUGHList.Where(x => x.TroughName.Equals(this.PRINTER3_LINE2.MessagesDetailModel.Trough) && x.PrinterLead == this.PRINTER3_LINE2)
                                                                                           .ToList().ForEach((x) => { this.PRINTER3_LINE2.TroughByUse = x; x.IsSelected = true; x.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE; });
                                                        TroughViewModel.Instance.TROUGHSelected = this.PRINTER3_LINE2.TroughByUse;
                                                    }
                                                    if (MaterialMessageBox.ShowWithCancel($"Bạn có muốn tiếp tục in bản tin của khách hàng {this.PRINTER3_LINE2.MessagesDetailModel.CustomerName} không?", "THÔNG BÁO") == MessageBoxResult.OK)
                                                    {
                                                        this.PRINTER3_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                        MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER3_LINE2.MessagesDetailModel);
                                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} đang in", Define.SolidColorOK);

                                                        this.PRINTER3_LINE2.NumberBag = (this.PRINTER3_LINE2.MessagesDetailModel.NumberBags);
                                                        this.PRINTER3_LINE2.MessagePrintCountShow = this.PRINTER3_LINE2.NumberBag;
                                                        this.PRINTER3_LINE2.Start(500);
                                                        return;
                                                    }
                                                }
                                            }

                                        }
                                        bool isCanPrint = (chs[5] == (char)0x00 && chs[6] == (char)0x02);
                                        if (isCanPrint)
                                        {
                                            this.PRINTER3_LINE2.PRINTER_STATES = PRINTER_STATES.IS_CAN_PRINT;
                                            if (this.PRINTER3_LINE2.TroughByUse != null)
                                            {
                                                this.PRINTER3_LINE2.TroughByUse.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                                //Add-new [22-11-2022]
                                                TROUGH tr = TroughViewModel.Instance.TROUGHList.Where(x => x.Tag == this.PRINTER3_LINE2.TroughByUse.Tag &&
                                                                                                           x.PrinterLead != this.PRINTER3_LINE2.TroughByUse.PrinterLead)
                                                                                               .FirstOrDefault();
                                                if (tr != null)
                                                    tr.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                            }
                                            this.StepsPerform(PRINTER3_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Đã bật tia thành công", Define.SolidColorOK);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Máy in có thể in", Define.SolidColorOK);
                                        }
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Không thể đọc được trạng thái {this.PRINTER3_LINE2.PrinterName}", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartJetID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} Bắt đầu khởi động tia mực...", Define.SolidColorOK);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} bật tia thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    try
                                    {
                                        // code
                                        if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                        {
                                            // code
                                            if (this.IsManual)
                                            {
                                                MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                                this.IsManual = false;
                                                return;
                                            }
                                            if (this.PRINTER3_LINE2.PRINTER_STATES != PRINTER_STATES.PRINT_STOP)
                                            {
                                                this.UpdateGuidMsgNewest();
                                                MainViewModel.Instance.UpdateMessageDetail();
                                                MainViewModel.Instance.SendMsgToServer("Printing");
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            else
                                            {
                                                this.PRINTER3_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER3_LINE2.MessagesDetailModel);
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            this.PRINTER3_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER3_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} bật in thành công", Define.SolidColorOK);

                                            this.PRINTER3_LINE2.Start(500);
                                        }
                                        else
                                        {
                                            this.PRINTER3_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} bật in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StopPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        // code
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER3_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_STOP; //add new 22-11-2022
                                        this.PRINTER3_LINE2.Stop();
                                        if (this.PRINTER3_LINE2.MessagesDetailModel != null)
                                        {
                                            this.PRINTER3_LINE2.MessagesDetailModel.NumberBagsPrinted += this.PRINTER3_LINE2.MessagePrintCount;
                                            this.PRINTER3_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.NOT_PRINT_DONE);
                                            MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER3_LINE2.MessagesDetailModel);
                                        }
                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);

                                        //Kiểm tra số lượng đã in và hiển thị bảng chỉnh sửa
                                        if (this.PRINTER3_LINE2.MessagesDetailModel.NumberBagsPrinted > this.PRINTER3_LINE2.MessagesDetailModel.NumberBags ||
                                           this.PRINTER3_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER3_LINE2.MessagesDetailModel.NumberBags)
                                        {
                                            MainViewModel.Instance.EditMessagePrintedViewModel.MessagesDetailModel = (MessagesDetailModel)this.PRINTER3_LINE2.MessagesDetailModel.Clone();
                                            EditMessagePrintedView editMessagePrintedView = new EditMessagePrintedView();
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView = editMessagePrintedView;
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.Title = $"{this.PRINTER3_LINE2.PrinterName} - Bản tin khách hàng {this.PRINTER3_LINE2.MessagesDetailModel.CustomerName}";
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.ShowDialog();
                                        }

                                        //Hiển thị bảng LED sau khi ấn DỪNG in
                                        await TroughViewModel.Instance.SetTextToAllLEDPanel(this.PRINTER3_LINE2, this.PRINTER3_LINE2.LEDTableFixed, this.PRINTER3_LINE2.TroughByUse,
                                                                                            this.PRINTER3_LINE2.PrinterName,
                                                                                            this.PRINTER3_LINE2.TroughByUse?.TroughName,
                                                                                            "HT");
                                        this.StepsPerform(this.PRINTER3_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} tắt in thành công", Define.SolidColorOK);


                                    }
                                    else
                                    {
                                        this.PRINTER3_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} tắt in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.RequestMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        char[] msg = new char[chs.Length - 10];
                                        Array.Copy(chs, 5, msg, 0, msg.Length);
                                        String s = new String(msg);
                                        this.PRINTER3_LINE2.CurrentMessageOnPrinter = s;
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Bản tin hiện tại trên máy in là {s}", Define.SolidColorOK);
                                        if (this.PRINTER3_LINE2.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(this.PRINTER3_LINE2, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} yêu cầu lấy tên bản tin thất bại", Define.SolidColorFail);
                                }

                                break;



                            case (char)CommandsIDPrinter.DownloadMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {

                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} TẠO bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER3_LINE2, STEPS_PERFORM.LOAD_PRINT_MESSAGE);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} TẠO bản tin thất bại [Lỗi máy in: {PrinterFaultList.ToDescription(chs[2])}  Lỗi command: {CommandFaultCodeList.ToDescription(chs[3])}]", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.LoadPrintMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} TẢI bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER3_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                        this.StepsPerform(PRINTER3_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} TẢI bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.DeleteMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} XÓA bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER3_LINE2, STEPS_PERFORM.DOWNLOAD_MESSAGE_DATA);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} XÓA bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                        this.PRINTER3_LINE2.PRINTER_STATES = PRINTER_STATES.CONNECTED;
                        this.StepsPerform(this.PRINTER3_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                        this.PRINTER3_LINE2.NotifyMsgOfPrinter = $"Đã kết nối {PRINTERSelected.PrinterName} thành công";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã kết nối {PRINTERSelected.PrinterName} thành công", Define.SolidColorOK);
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                        this.PRINTER3_LINE2.PRINTER_STATES = PRINTER_STATES.DISCONNECTED;
                        this.PRINTER3_LINE2.CurrentMessageOnPrinter = "[...]";
                        this.PRINTER3_LINE2.PrinterStatusDescription = "[...]";
                        this.PRINTER3_LINE2.NotifyMsgOfPrinter = "[...]";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã ngắt kết nối với {PRINTERSelected.PrinterName}", Define.SolidColorFail);

                        break;
                    default:
                        break;
                }
#endif
            }));
        }
        private async void PRINTER4_LINE2_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER4_LINE2);
#endif

#if !SHORTCUT
                switch (e)
                {
                    case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                        if (obj is bool) return;
                        char[] chs = (char[])obj;
                        if (chs.Length == 0 || chs.Length < 5) return;
                        char commandIdPrinter = chs[4];
                        switch (commandIdPrinter)
                        {
                            case (char)CommandsIDPrinter.RequestMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        await Task.Factory.StartNew(async () =>
                                        {
                                            if (this.PRINTER4_LINE2.MessagesDetailModel == null)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER4_LINE2.MessagePrintCount = decValue;
                                                return;
                                            }

                                            if (this.PRINTER4_LINE2.PRINTER_STATES == PRINTER_STATES.PRINTING)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER4_LINE2.MessagePrintCount = decValue;
                                                await Task.Delay(2);

                                            }
                                        });

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER4_LINE2.Stop();
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} không thể đọc được số đếm của bản tin", Define.SolidColorFail);
                                }
                                break;
#if !DEBUG
                            case (char)CommandsIDPrinter.RequestTotalPrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                          + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                        int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code

                                }
                                break;
                            case (char)CommandsIDPrinter.SetMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        if (this.PRINTER1_LINE3.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                }
                                break;
                            case (char)CommandsIDPrinter.DownloadRemoteFieldData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    this.PRINTER1_LINE3.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                    this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.SET_MESSAGE_PRINT_COUNT);
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin thành công";

                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Nạp bản tin thành công", Define.SolidColorOK);
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]";
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]", Define.SolidColorFail);
                                }
                                break;
#endif

                            case (char)CommandsIDPrinter.RequestPrinterStatus:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        this.PRINTER4_LINE2.PrinterStatusDescription = String.Empty;
                                        this.PRINTER4_LINE2.PrinterStatusDescription += JetStateList.ToDescription(chs[5]);
                                        this.PRINTER4_LINE2.PrinterStatusDescription += PrintState.ToDescription(chs[6]);
                                        this.PRINTER4_LINE2.PrinterStatusDescription += ErrorMask.ToDescription(new char[] { chs[8], chs[9], chs[10], chs[11], });

                                        //ADD-NEW
                                        if (chs[5] == (char)0x00 && chs[6] == (char)0x04)
                                        {
                                            this.PRINTER4_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER4_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            this.SelectMessageDetaiNewestlForPrinter(this.PRINTER4_LINE2);
                                            this.PRINTERSelected = this.PRINTER4_LINE2;

                                            if (this.PRINTER4_LINE2.MessagesDetailModel != null)
                                            {
                                                if (this.PRINTER4_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER4_LINE2.MessagesDetailModel.NumberBags)
                                                {
                                                    if (!this.PRINTER4_LINE2.MessagesDetailModel.Trough.Equals(""))
                                                    {
                                                        TroughViewModel.Instance.TROUGHList.Where(x => x.TroughName.Equals(this.PRINTER4_LINE2.MessagesDetailModel.Trough) && x.PrinterLead == this.PRINTER4_LINE2)
                                                                                           .ToList().ForEach((x) => { this.PRINTER4_LINE2.TroughByUse = x; x.IsSelected = true; x.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE; });
                                                        TroughViewModel.Instance.TROUGHSelected = this.PRINTER4_LINE2.TroughByUse;
                                                    }
                                                    if (MaterialMessageBox.ShowWithCancel($"Bạn có muốn tiếp tục in bản tin của khách hàng {this.PRINTER4_LINE2.MessagesDetailModel.CustomerName} không?", "THÔNG BÁO") == MessageBoxResult.OK)
                                                    {
                                                        this.PRINTER4_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                        MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER4_LINE2.MessagesDetailModel);
                                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} đang in", Define.SolidColorOK);

                                                        this.PRINTER4_LINE2.NumberBag = (this.PRINTER4_LINE2.MessagesDetailModel.NumberBags);
                                                        this.PRINTER4_LINE2.MessagePrintCountShow = this.PRINTER4_LINE2.NumberBag;
                                                        this.PRINTER4_LINE2.Start(500);

                                                        return;
                                                    }
                                                }
                                            }

                                        }
                                        bool isCanPrint = (chs[5] == (char)0x00 && chs[6] == (char)0x02);
                                        if (isCanPrint)
                                        {
                                            this.PRINTER4_LINE2.PRINTER_STATES = PRINTER_STATES.IS_CAN_PRINT;
                                            if (this.PRINTER4_LINE2.TroughByUse != null)
                                            {
                                                this.PRINTER4_LINE2.TroughByUse.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                                //Add-new [22-11-2022]
                                                TROUGH tr = TroughViewModel.Instance.TROUGHList.Where(x => x.Tag == this.PRINTER4_LINE2.TroughByUse.Tag &&
                                                                                                           x.PrinterLead != this.PRINTER4_LINE2.TroughByUse.PrinterLead)
                                                                                               .FirstOrDefault();
                                                if (tr != null)
                                                    tr.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                            }
                                            this.StepsPerform(PRINTER4_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Đã bật tia thành công", Define.SolidColorOK);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Máy in có thể in", Define.SolidColorOK);
                                        }
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Không thể đọc được trạng thái {this.PRINTER4_LINE2.PrinterName}", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartJetID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} Bắt đầu khởi động tia mực...", Define.SolidColorOK);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} bật tia thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    try
                                    {
                                        // code
                                        if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                        {
                                            // code
                                            if (this.IsManual)
                                            {
                                                MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                                this.IsManual = false;
                                                return;
                                            }
                                            if (this.PRINTER4_LINE2.PRINTER_STATES != PRINTER_STATES.PRINT_STOP)
                                            {
                                                this.UpdateGuidMsgNewest();
                                                MainViewModel.Instance.UpdateMessageDetail();
                                                MainViewModel.Instance.SendMsgToServer("Printing");
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            else
                                            {
                                                this.PRINTER4_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER4_LINE2.MessagesDetailModel);
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            this.PRINTER4_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER4_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} bật in thành công", Define.SolidColorOK);

                                            this.PRINTER4_LINE2.Start(500);
                                        }
                                        else
                                        {
                                            this.PRINTER4_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} bật in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StopPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        // code
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER4_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_STOP; //add new 22-11-2022
                                        this.PRINTER4_LINE2.Stop();
                                        if (this.PRINTER4_LINE2.MessagesDetailModel != null)
                                        {
                                            this.PRINTER4_LINE2.MessagesDetailModel.NumberBagsPrinted += this.PRINTER4_LINE2.MessagePrintCount;
                                            this.PRINTER4_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.NOT_PRINT_DONE);
                                            MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER4_LINE2.MessagesDetailModel);
                                        }
                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);

                                        //Kiểm tra số lượng đã in và hiển thị bảng chỉnh sửa
                                        if (this.PRINTER4_LINE2.MessagesDetailModel.NumberBagsPrinted > this.PRINTER4_LINE2.MessagesDetailModel.NumberBags ||
                                           this.PRINTER4_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER4_LINE2.MessagesDetailModel.NumberBags)
                                        {
                                            MainViewModel.Instance.EditMessagePrintedViewModel.MessagesDetailModel = (MessagesDetailModel)this.PRINTER4_LINE2.MessagesDetailModel.Clone();
                                            EditMessagePrintedView editMessagePrintedView = new EditMessagePrintedView();
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView = editMessagePrintedView;
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.Title = $"{this.PRINTER4_LINE2.PrinterName} - Bản tin khách hàng {this.PRINTER4_LINE2.MessagesDetailModel.CustomerName}";
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.ShowDialog();
                                        }

                                        //Hiển thị bảng LED sau khi ấn DỪNG in
                                        await TroughViewModel.Instance.SetTextToAllLEDPanel(this.PRINTER4_LINE2, this.PRINTER4_LINE2.LEDTableFixed, this.PRINTER4_LINE2.TroughByUse,
                                                                                            this.PRINTER4_LINE2.PrinterName,
                                                                                            this.PRINTER4_LINE2.TroughByUse?.TroughName,
                                                                                            "HT");
                                        this.StepsPerform(this.PRINTER4_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);

                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} tắt in thành công", Define.SolidColorOK);


                                    }
                                    else
                                    {
                                        this.PRINTER4_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} tắt in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.RequestMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        char[] msg = new char[chs.Length - 10];
                                        Array.Copy(chs, 5, msg, 0, msg.Length);
                                        String s = new String(msg);
                                        this.PRINTER4_LINE2.CurrentMessageOnPrinter = s;
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Bản tin hiện tại trên máy in là {s}", Define.SolidColorOK);
                                        if (this.PRINTER4_LINE2.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(this.PRINTER4_LINE2, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} yêu cầu lấy tên bản tin thất bại", Define.SolidColorFail);
                                }

                                break;



                            case (char)CommandsIDPrinter.DownloadMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {

                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} TẠO bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER4_LINE2, STEPS_PERFORM.LOAD_PRINT_MESSAGE);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} TẠO bản tin thất bại [Lỗi máy in: {PrinterFaultList.ToDescription(chs[2])}  Lỗi command: {CommandFaultCodeList.ToDescription(chs[3])}]", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.LoadPrintMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} TẢI bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER4_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                        this.StepsPerform(PRINTER4_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} TẢI bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.DeleteMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} XÓA bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER4_LINE2, STEPS_PERFORM.DOWNLOAD_MESSAGE_DATA);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} XÓA bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                        this.PRINTER4_LINE2.PRINTER_STATES = PRINTER_STATES.CONNECTED;
                        this.StepsPerform(this.PRINTER4_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                        this.PRINTER4_LINE2.NotifyMsgOfPrinter = $"Đã kết nối {PRINTERSelected.PrinterName} thành công";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã kết nối {PRINTERSelected.PrinterName} thành công", Define.SolidColorOK);
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                        this.PRINTER4_LINE2.PRINTER_STATES = PRINTER_STATES.DISCONNECTED;
                        this.PRINTER4_LINE2.CurrentMessageOnPrinter = "[...]";
                        this.PRINTER4_LINE2.PrinterStatusDescription = "[...]";
                        this.PRINTER4_LINE2.NotifyMsgOfPrinter = "[...]";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã ngắt kết nối với {PRINTERSelected.PrinterName}", Define.SolidColorFail);


                        break;
                    default:
                        break;
                }
#endif
            }));
        }
        private async void PRINTER5_LINE2_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER5_LINE2);
#endif

#if !SHORTCUT
                switch (e)
                {
                    case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                        if (obj is bool) return;
                        char[] chs = (char[])obj;
                        if (chs.Length == 0 || chs.Length < 5) return;
                        char commandIdPrinter = chs[4];
                        switch (commandIdPrinter)
                        {
                            case (char)CommandsIDPrinter.RequestMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        await Task.Factory.StartNew(async () =>
                                        {
                                            if (this.PRINTER5_LINE2.MessagesDetailModel == null)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER5_LINE2.MessagePrintCount = decValue;
                                                return;
                                            }
                                            if (this.PRINTER5_LINE2.PRINTER_STATES == PRINTER_STATES.PRINTING)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER5_LINE2.MessagePrintCount = decValue;
                                                await Task.Delay(2);

                                            }
                                        });
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER5_LINE2.Stop();
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} không thể đọc được số đếm của bản tin", Define.SolidColorFail);
                                }
                                break;
#if !DEBUG
                            case (char)CommandsIDPrinter.RequestTotalPrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                          + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                        int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code

                                }
                                break;
                            case (char)CommandsIDPrinter.SetMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        if (this.PRINTER1_LINE3.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                }
                                break;
                            case (char)CommandsIDPrinter.DownloadRemoteFieldData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    this.PRINTER1_LINE3.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                    this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.SET_MESSAGE_PRINT_COUNT);
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin thành công";

                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Nạp bản tin thành công", Define.SolidColorOK);
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]";
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]", Define.SolidColorFail);
                                }
                                break;
#endif

                            case (char)CommandsIDPrinter.RequestPrinterStatus:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        this.PRINTER5_LINE2.PrinterStatusDescription = String.Empty;
                                        this.PRINTER5_LINE2.PrinterStatusDescription += JetStateList.ToDescription(chs[5]);
                                        this.PRINTER5_LINE2.PrinterStatusDescription += PrintState.ToDescription(chs[6]);
                                        this.PRINTER5_LINE2.PrinterStatusDescription += ErrorMask.ToDescription(new char[] { chs[8], chs[9], chs[10], chs[11], });

                                        //ADD-NEW
                                        if (chs[5] == (char)0x00 && chs[6] == (char)0x04)
                                        {
                                            this.PRINTER5_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER5_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            this.SelectMessageDetaiNewestlForPrinter(this.PRINTER5_LINE2);
                                            this.PRINTERSelected = this.PRINTER5_LINE2;

                                            if (this.PRINTER5_LINE2.MessagesDetailModel != null)
                                            {
                                                if (this.PRINTER5_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER5_LINE2.MessagesDetailModel.NumberBags)
                                                {
                                                    if (!this.PRINTER5_LINE2.MessagesDetailModel.Trough.Equals(""))
                                                    {
                                                        TroughViewModel.Instance.TROUGHList.Where(x => x.TroughName.Equals(this.PRINTER5_LINE2.MessagesDetailModel.Trough) && x.PrinterLead == this.PRINTER5_LINE2)
                                                                                           .ToList().ForEach((x) => { this.PRINTER5_LINE2.TroughByUse = x; x.IsSelected = true; x.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE; });
                                                        TroughViewModel.Instance.TROUGHSelected = this.PRINTER5_LINE2.TroughByUse;
                                                    }
                                                    if (MaterialMessageBox.ShowWithCancel($"Bạn có muốn tiếp tục in bản tin của khách hàng {this.PRINTER5_LINE2.MessagesDetailModel.CustomerName} không?", "THÔNG BÁO") == MessageBoxResult.OK)
                                                    {
                                                        this.PRINTER5_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                        MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER5_LINE2.MessagesDetailModel);
                                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} đang in", Define.SolidColorOK);

                                                        this.PRINTER5_LINE2.NumberBag = (this.PRINTER5_LINE2.MessagesDetailModel.NumberBags);
                                                        this.PRINTER5_LINE2.MessagePrintCountShow = this.PRINTER5_LINE2.NumberBag;
                                                        this.PRINTER5_LINE2.Start(500);

                                                        return;
                                                    }
                                                }
                                            }

                                        }
                                        bool isCanPrint = (chs[5] == (char)0x00 && chs[6] == (char)0x02);
                                        if (isCanPrint)
                                        {
                                            this.PRINTER5_LINE2.PRINTER_STATES = PRINTER_STATES.IS_CAN_PRINT;
                                            if (this.PRINTER5_LINE2.TroughByUse != null)
                                            {
                                                this.PRINTER5_LINE2.TroughByUse.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                                //Add-new [22-11-2022]
                                                TROUGH tr = TroughViewModel.Instance.TROUGHList.Where(x => x.Tag == this.PRINTER5_LINE2.TroughByUse.Tag &&
                                                                                                           x.PrinterLead != this.PRINTER5_LINE2.TroughByUse.PrinterLead)
                                                                                               .FirstOrDefault();
                                                if (tr != null)
                                                    tr.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                            }
                                            this.StepsPerform(PRINTER5_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Đã bật tia thành công", Define.SolidColorOK);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Máy in có thể in", Define.SolidColorOK);
                                        }
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Không thể đọc được trạng thái {this.PRINTER5_LINE2.PrinterName}", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartJetID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} Bắt đầu khởi động tia mực...", Define.SolidColorOK);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} bật tia thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    try
                                    {
                                        // code
                                        if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                        {
                                            // code
                                            if (this.IsManual)
                                            {
                                                MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                                this.IsManual = false;
                                                return;
                                            }
                                            if (this.PRINTER5_LINE2.PRINTER_STATES != PRINTER_STATES.PRINT_STOP)
                                            {
                                                this.UpdateGuidMsgNewest();
                                                MainViewModel.Instance.UpdateMessageDetail();
                                                MainViewModel.Instance.SendMsgToServer("Printing");
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            else
                                            {
                                                this.PRINTER5_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER5_LINE2.MessagesDetailModel);
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            this.PRINTER5_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER5_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} bật in thành công", Define.SolidColorOK);

                                            this.PRINTER5_LINE2.Start(500);
                                        }
                                        else
                                        {
                                            this.PRINTER5_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} bật in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StopPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        // code
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER5_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_STOP; //add new 22-11-2022
                                        this.PRINTER5_LINE2.Stop();
                                        if (this.PRINTER5_LINE2.MessagesDetailModel != null)
                                        {
                                            this.PRINTER5_LINE2.MessagesDetailModel.NumberBagsPrinted += this.PRINTER5_LINE2.MessagePrintCount;
                                            this.PRINTER5_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.NOT_PRINT_DONE);
                                            MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER5_LINE2.MessagesDetailModel);
                                        }
                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);

                                        //Kiểm tra số lượng đã in và hiển thị bảng chỉnh sửa
                                        if (this.PRINTER5_LINE2.MessagesDetailModel.NumberBagsPrinted > this.PRINTER5_LINE2.MessagesDetailModel.NumberBags ||
                                           this.PRINTER5_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER5_LINE2.MessagesDetailModel.NumberBags)
                                        {
                                            MainViewModel.Instance.EditMessagePrintedViewModel.MessagesDetailModel = (MessagesDetailModel)this.PRINTER5_LINE2.MessagesDetailModel.Clone();
                                            EditMessagePrintedView editMessagePrintedView = new EditMessagePrintedView();
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView = editMessagePrintedView;
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.Title = $"{this.PRINTER5_LINE2.PrinterName} - Bản tin khách hàng {this.PRINTER5_LINE2.MessagesDetailModel.CustomerName}";
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.ShowDialog();
                                        }

                                        //Hiển thị bảng LED sau khi ấn DỪNG in
                                        await TroughViewModel.Instance.SetTextToAllLEDPanel(this.PRINTER5_LINE2, this.PRINTER5_LINE2.LEDTableFixed, this.PRINTER5_LINE2.TroughByUse,
                                                                                            this.PRINTER5_LINE2.PrinterName,
                                                                                            this.PRINTER5_LINE2.TroughByUse?.TroughName,
                                                                                            "HT");
                                        this.StepsPerform(this.PRINTER5_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);

                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} tắt in thành công", Define.SolidColorOK);


                                    }
                                    else
                                    {
                                        this.PRINTER5_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} tắt in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.RequestMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        char[] msg = new char[chs.Length - 10];
                                        Array.Copy(chs, 5, msg, 0, msg.Length);
                                        String s = new String(msg);
                                        this.PRINTER5_LINE2.CurrentMessageOnPrinter = s;
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Bản tin hiện tại trên máy in là {s}", Define.SolidColorOK);
                                        if (this.PRINTER5_LINE2.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(this.PRINTER5_LINE2, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} yêu cầu lấy tên bản tin thất bại", Define.SolidColorFail);
                                }

                                break;



                            case (char)CommandsIDPrinter.DownloadMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {

                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} TẠO bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER5_LINE2, STEPS_PERFORM.LOAD_PRINT_MESSAGE);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} TẠO bản tin thất bại [Lỗi máy in: {PrinterFaultList.ToDescription(chs[2])}  Lỗi command: {CommandFaultCodeList.ToDescription(chs[3])}]", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.LoadPrintMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} TẢI bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER5_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                        this.StepsPerform(PRINTER5_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} TẢI bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.DeleteMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} XÓA bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER5_LINE2, STEPS_PERFORM.DOWNLOAD_MESSAGE_DATA);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} XÓA bản tin thất bại", Define.SolidColorFail);
                                }

                                break;
                            default:
                                break;
                        }
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                        this.PRINTER5_LINE2.PRINTER_STATES = PRINTER_STATES.CONNECTED;
                        this.StepsPerform(this.PRINTER5_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                        this.PRINTER5_LINE2.NotifyMsgOfPrinter = $"Đã kết nối {PRINTERSelected.PrinterName} thành công";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã kết nối {PRINTERSelected.PrinterName} thành công", Define.SolidColorOK);
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                        this.PRINTER5_LINE2.PRINTER_STATES = PRINTER_STATES.DISCONNECTED;
                        this.PRINTER5_LINE2.CurrentMessageOnPrinter = "[...]";
                        this.PRINTER5_LINE2.PrinterStatusDescription = "[...]";
                        this.PRINTER5_LINE2.NotifyMsgOfPrinter = "[...]";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã ngắt kết nối với {PRINTERSelected.PrinterName}", Define.SolidColorFail);

                        break;
                    default:
                        break;
                }
#endif
            }));
        }
        private async void PRINTER6_LINE2_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER6_LINE2);
#endif

#if !SHORTCUT
                switch (e)
                {
                    case NClientSocket.EConnectionEventClient.RECEIVEDATA:
                        if (obj is bool) return;
                        char[] chs = (char[])obj;
                        if (chs.Length == 0 || chs.Length < 5) return;
                        char commandIdPrinter = chs[4];
                        switch (commandIdPrinter)
                        {
                            case (char)CommandsIDPrinter.RequestMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        if (this.PRINTER6_LINE2.PRINTER_STATES == PRINTER_STATES.PRINTING)
                                        {
                                            if (this.PRINTER6_LINE2.MessagesDetailModel == null)
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                                  + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER6_LINE2.MessagePrintCount = decValue;
                                                return;
                                            }
                                            await Task.Factory.StartNew(async () =>
                                            {
                                                string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                             + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                                int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                                this.PRINTER6_LINE2.MessagePrintCount = decValue;
                                                await Task.Delay(2);

                                            });

                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER6_LINE2.Stop();
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} không thể đọc được số đếm của bản tin", Define.SolidColorFail);
                                }
                                break;
#if !DEBUG
                            case (char)CommandsIDPrinter.RequestTotalPrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        string hexValue = ((byte)chs[8]).ToString("X") + ((byte)chs[7]).ToString("X")
                                                          + ((byte)chs[6]).ToString("X") + ((byte)chs[5]).ToString("X"); //Convert dec -> hex
                                        int decValue = Convert.ToInt32(hexValue, 16); // Convert hex -> dec
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code

                                }
                                break;
                            case (char)CommandsIDPrinter.SetMessagePrintCount:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        if (this.PRINTER1_LINE3.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                }
                                break;
                            case (char)CommandsIDPrinter.DownloadRemoteFieldData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    this.PRINTER1_LINE3.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                    this.StepsPerform(PRINTER1_LINE3, STEPS_PERFORM.SET_MESSAGE_PRINT_COUNT);
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin thành công";

                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Nạp bản tin thành công", Define.SolidColorOK);
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    this.PRINTER1_LINE3.NotifyMsgOfPrinter = "Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]";
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Nạp bản tin không thành công " + "[" + CommandFaultCodeList.ToDescription(chs[3]) + "]", Define.SolidColorFail);
                                }
                                break;
#endif

                            case (char)CommandsIDPrinter.RequestPrinterStatus:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        this.PRINTER6_LINE2.PrinterStatusDescription = String.Empty;
                                        this.PRINTER6_LINE2.PrinterStatusDescription += JetStateList.ToDescription(chs[5]);
                                        this.PRINTER6_LINE2.PrinterStatusDescription += PrintState.ToDescription(chs[6]);
                                        this.PRINTER6_LINE2.PrinterStatusDescription += ErrorMask.ToDescription(new char[] { chs[8], chs[9], chs[10], chs[11], });

                                        //ADD-NEW
                                        if (chs[5] == (char)0x00 && chs[6] == (char)0x04)
                                        {
                                            this.PRINTER6_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER6_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            this.SelectMessageDetaiNewestlForPrinter(this.PRINTER6_LINE2);
                                            this.PRINTERSelected = this.PRINTER6_LINE2;

                                            if (this.PRINTER6_LINE2.MessagesDetailModel != null)
                                            {
                                                if (this.PRINTER6_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER6_LINE2.MessagesDetailModel.NumberBags)
                                                {
                                                    if (!this.PRINTER6_LINE2.MessagesDetailModel.Trough.Equals(""))
                                                    {
                                                        TroughViewModel.Instance.TROUGHList.Where(x => x.TroughName.Equals(this.PRINTER6_LINE2.MessagesDetailModel.Trough) && x.PrinterLead == this.PRINTER6_LINE2)
                                                                                           .ToList().ForEach((x) => { this.PRINTER6_LINE2.TroughByUse = x; x.IsSelected = true; x.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE; });
                                                        TroughViewModel.Instance.TROUGHSelected = this.PRINTER6_LINE2.TroughByUse;
                                                    }
                                                    if (MaterialMessageBox.ShowWithCancel($"Bạn có muốn tiếp tục in bản tin của khách hàng {this.PRINTER6_LINE2.MessagesDetailModel.CustomerName} không?", "THÔNG BÁO") == MessageBoxResult.OK)
                                                    {
                                                        this.PRINTER6_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                        MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER6_LINE2.MessagesDetailModel);
                                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} đang in", Define.SolidColorOK);

                                                        this.PRINTER6_LINE2.NumberBag = (this.PRINTER6_LINE2.MessagesDetailModel.NumberBags);
                                                        this.PRINTER6_LINE2.MessagePrintCountShow = this.PRINTER6_LINE2.NumberBag;
                                                        this.PRINTER6_LINE2.Start(500);

                                                        return;
                                                    }
                                                }
                                            }

                                        }
                                        bool isCanPrint = (chs[5] == (char)0x00 && chs[6] == (char)0x02);
                                        if (isCanPrint)
                                        {
                                            this.PRINTER6_LINE2.PRINTER_STATES = PRINTER_STATES.IS_CAN_PRINT;
                                            if (this.PRINTER6_LINE2.TroughByUse != null)
                                            {
                                                this.PRINTER6_LINE2.TroughByUse.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                                //Add-new [22-11-2022]
                                                TROUGH tr = TroughViewModel.Instance.TROUGHList.Where(x => x.Tag == this.PRINTER6_LINE2.TroughByUse.Tag &&
                                                                                                           x.PrinterLead != this.PRINTER6_LINE2.TroughByUse.PrinterLead)
                                                                                               .FirstOrDefault();
                                                if (tr != null)
                                                    tr.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                                            }
                                            this.StepsPerform(PRINTER6_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Đã bật tia thành công", Define.SolidColorOK);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, "Máy in có thể in", Define.SolidColorOK);
                                        }
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Không thể đọc được trạng thái {this.PRINTER6_LINE2.PrinterName}", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartJetID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} Bắt đầu khởi động tia mực...", Define.SolidColorOK);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                    if (this.IsManual)
                                    {
                                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                        this.IsManual = false;
                                        return;
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} bật tia thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StartPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    try
                                    {
                                        // code
                                        if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                        {
                                            // code
                                            if (this.IsManual)
                                            {
                                                MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                                this.IsManual = false;
                                                return;
                                            }
                                            if (this.PRINTER6_LINE2.PRINTER_STATES != PRINTER_STATES.PRINT_STOP)
                                            {
                                                this.UpdateGuidMsgNewest();
                                                MainViewModel.Instance.UpdateMessageDetail();
                                                MainViewModel.Instance.SendMsgToServer("Printing");
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            else
                                            {
                                                this.PRINTER6_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
                                                MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER6_LINE2.MessagesDetailModel);
                                                MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                                MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);
                                            }
                                            this.PRINTER6_LINE2.PRINTER_STATES = PRINTER_STATES.PRINTING;
                                            this.PRINTER6_LINE2.MESSAGE_STATE = MESSAGE_STATE.PRINTING;
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} bật in thành công", Define.SolidColorOK);

                                            this.PRINTER6_LINE2.Start(500);
                                        }
                                        else
                                        {
                                            this.PRINTER6_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} bật in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.StopPrintID:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        // code
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER6_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_STOP; //add new 22-11-2022
                                        this.PRINTER6_LINE2.Stop();
                                        if (this.PRINTER6_LINE2.MessagesDetailModel != null)
                                        {
                                            this.PRINTER6_LINE2.MessagesDetailModel.NumberBagsPrinted += this.PRINTER6_LINE2.MessagePrintCount;
                                            this.PRINTER6_LINE2.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.NOT_PRINT_DONE);
                                            MainViewModel.Instance.UpdateNumberBagPrintedAndMessageState(this.PRINTER6_LINE2.MessagesDetailModel);
                                        }
                                        MainViewModel.Instance.SelectMessagesDetail_Printing(DateNow);
                                        MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateNow);

                                        //Kiểm tra số lượng đã in và hiển thị bảng chỉnh sửa
                                        if (this.PRINTER6_LINE2.MessagesDetailModel.NumberBagsPrinted > this.PRINTER6_LINE2.MessagesDetailModel.NumberBags ||
                                           this.PRINTER6_LINE2.MessagesDetailModel.NumberBagsPrinted < this.PRINTER6_LINE2.MessagesDetailModel.NumberBags)
                                        {
                                            MainViewModel.Instance.EditMessagePrintedViewModel.MessagesDetailModel = (MessagesDetailModel)this.PRINTER6_LINE2.MessagesDetailModel.Clone();
                                            EditMessagePrintedView editMessagePrintedView = new EditMessagePrintedView();
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView = editMessagePrintedView;
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.Title = $"{this.PRINTER6_LINE2.PrinterName} - Bản tin khách hàng {this.PRINTER6_LINE2.MessagesDetailModel.CustomerName}";
                                            EditMessagePrintedViewModel.Instance.EditMessagePrintedView.ShowDialog();
                                        }

                                        //Hiển thị bảng LED sau khi ấn DỪNG in
                                        await TroughViewModel.Instance.SetTextToAllLEDPanel(this.PRINTER6_LINE2, this.PRINTER6_LINE2.LEDTableFixed, this.PRINTER6_LINE2.TroughByUse,
                                                                                            this.PRINTER6_LINE2.PrinterName,
                                                                                            this.PRINTER6_LINE2.TroughByUse?.TroughName,
                                                                                            "HT");
                                        this.StepsPerform(this.PRINTER6_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);

                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} tắt in thành công", Define.SolidColorOK);

                                    }
                                    else
                                    {
                                        this.PRINTER6_LINE2.PrinterStatusDescription += PrinterFaultList.ToDescription(chs[2]);
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} tắt in thất bại", Define.SolidColorFail);
                                }

                                break;
                            case (char)CommandsIDPrinter.RequestMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        char[] msg = new char[chs.Length - 10];
                                        Array.Copy(chs, 5, msg, 0, msg.Length);
                                        String s = new String(msg);
                                        this.PRINTER6_LINE2.CurrentMessageOnPrinter = s;
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Bản tin hiện tại trên máy in là {s}", Define.SolidColorOK);
                                        if (this.PRINTER6_LINE2.PRINTER_STATES == PRINTER_STATES.PRINT_READY)
                                        {
                                            this.StepsPerform(this.PRINTER6_LINE2, STEPS_PERFORM.START_PRINT);
                                        }
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} yêu cầu lấy tên bản tin thất bại", Define.SolidColorFail);
                                }

                                break;



                            case (char)CommandsIDPrinter.DownloadMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {

                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} TẠO bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER6_LINE2, STEPS_PERFORM.LOAD_PRINT_MESSAGE);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} TẠO bản tin thất bại [Lỗi máy in: {PrinterFaultList.ToDescription(chs[2])}  Lỗi command: {CommandFaultCodeList.ToDescription(chs[3])}]", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.LoadPrintMessage:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code

                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} TẢI bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.PRINTER6_LINE2.PRINTER_STATES = PRINTER_STATES.PRINT_READY;
                                        this.StepsPerform(PRINTER6_LINE2, STEPS_PERFORM.REQUEST_MESSAGE);

                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} TẢI bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            case (char)CommandsIDPrinter.DeleteMessageData:
                                if (chs[0] == (char)0x1B && chs[1] == _commandExecuted)
                                {
                                    // code
                                    if (chs[2] == (char)0x0 && chs[3] == (char)0x0)
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} XÓA bản tin thành công", Define.SolidColorOK);
                                        if (this.IsManual)
                                        {
                                            MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                                            this.IsManual = false;
                                            return;
                                        }
                                        this.StepsPerform(this.PRINTER6_LINE2, STEPS_PERFORM.DOWNLOAD_MESSAGE_DATA);
                                    }
                                    else
                                    {
                                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, PrinterFaultList.ToDescription(chs[2]) + CommandFaultCodeList.ToDescription(chs[3]), Define.SolidColorFail);
                                    }
                                }
                                else if (chs[0] == (char)0x1B && chs[1] == _commandNoExecuted)
                                {
                                    // code
                                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} XÓA bản tin thất bại", Define.SolidColorFail);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTCONNECTED:
                        this.PRINTER6_LINE2.PRINTER_STATES = PRINTER_STATES.CONNECTED;
                        this.StepsPerform(this.PRINTER6_LINE2, STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                        this.PRINTER6_LINE2.NotifyMsgOfPrinter = $"Đã kết nối {PRINTERSelected.PrinterName} thành công";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã kết nối {PRINTERSelected.PrinterName} thành công", Define.SolidColorOK);
                        break;
                    case NClientSocket.EConnectionEventClient.CLIENTDISCONNECTED:
                        this.PRINTER6_LINE2.PRINTER_STATES = PRINTER_STATES.DISCONNECTED;
                        this.PRINTER6_LINE2.CurrentMessageOnPrinter = "[...]";
                        this.PRINTER6_LINE2.PrinterStatusDescription = "[...]";
                        this.PRINTER6_LINE2.NotifyMsgOfPrinter = "[...]";
                        MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"Đã ngắt kết nối với {PRINTERSelected.PrinterName}", Define.SolidColorFail);

                        break;
                    default:
                        break;
                }
#endif
            }));
        }
#endif

        
#if LINE3
        private async void PRINTER1_LINE3_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER1_LINE3);
#endif

            }));
        }
        private async void PRINTER2_LINE3_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER2_LINE3);
#endif

            }));
        }
        private async void PRINTER3_LINE3_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER3_LINE3);
#endif

            }));
        }
        private async void PRINTER4_LINE3_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER4_LINE3);
#endif

            }));
        }
        private async void PRINTER5_LINE3_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER5_LINE3);
#endif

            }));
        }
        private async void PRINTER6_LINE3_ConnectionEventCallback(NClientSocket.EConnectionEventClient e, object obj)
        {
            await this._dispatcher.BeginInvoke(new Action(async () =>
            {

#if SHORTCUT
                await PRINTER_ConnectionEventCallbackHandler(e, obj, this.PRINTER6_LINE3);
#endif

            }));
        }
#endif

#endregion

            #region ErrorEventCallback

#if !LINE1
        private void PRINTER1_LINE1_ClientErrorEventCallback(string errorMsg)
        {

        }
        private void PRINTER2_LINE1_ClientErrorEventCallback(string errorMsg)
        {

        }
#endif

#if !LINE2
            private void PRINTER1_LINE2_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE2.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER2_LINE2_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE2.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER3_LINE2_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE2.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER4_LINE2_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE2.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER5_LINE2_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE2.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER6_LINE2_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE2.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
#endif

#if LINE3
        private void PRINTER1_LINE3_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER1_LINE3.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER2_LINE3_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER2_LINE3.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER3_LINE3_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER3_LINE3.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER4_LINE3_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER4_LINE3.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER5_LINE3_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER5_LINE3.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
        private void PRINTER6_LINE3_ClientErrorEventCallback(string errorMsg)
        {
            MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{this.PRINTER6_LINE3.PrinterName} {errorMsg}", Define.SolidColorFail);
            MessageBox.Show(errorMsg);
        }
#endif

        #endregion

        #endregion

        #region Commands
        //public ICommand ConnectPrinterCmd { get; }
        public ICommand PrintCmd { get; }
        public ICommand RequestPrintStatusCmd { get; }
        public ICommand StartJetCmd { get; }
        public ICommand StopPrintCmd { get; }
        public ICommand DownloadMessageDataCmd { get; }
        public ICommand DeleteMessageDataCmd { get; }
        public ICommand LoadPrintMessageCmd { get; }


        #endregion
    }
}
