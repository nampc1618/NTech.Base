using MahApps.Metro.IconPacks;
using NTech.Base.Commons.BaseModel;
using NTech.Base.Resources.NNetSocket;
using NTech.Xm.Database.Models;
using NTech.Xm.Station.Command;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NTech.Xm.Station.Models
{
    internal class LineSetting
    {
    }
    public class LINE : BaseModel
    {
        public LINE()
        {
            this.Printers = new List<PRINTER>();
        }
        public string LineName { get; set; }
        public List<PRINTER> Printers { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }
        public int Tag { get; set; }
    }
    public class PRINTER : BaseModel
    {
        public readonly int PORT_PRINTER = 29043;
        public PRINTER()
        {
            this.Troughs = new List<TROUGH>();
            this._messagesDetailModel = new MessagesDetailModel();
            this.ConnectPrinterCmd = new ConnectPrinterCmd(PrinterViewModel.Instance);
            this.DisconnectPrinterCmd = new DisconnectPrinterCmd(PrinterViewModel.Instance);
            this.ToggleCheckedCmd = new ToggleCheckedCmd(PrinterViewModel.Instance);
            this.ToggleUncheckedCmd = new ToggleUncheckedCmd(PrinterViewModel.Instance);
            this.PreviewMouseLeftButtonDownCmd = new PreviewMouseLeftButtonDownCmd(PrinterViewModel.Instance);
        }
        public NClientSocket PRINTERClientSocket { get; set; }
        public Guid GuidMsgNewest { get; set; }
        public string GuidMsgNewestName { get; set; }
        public string IPAddress { get; set; }
        public string PrinterName { get; set; }
        public TROUGH TroughByUse { get; set; }
        public TROUGH LEDTableFixed { get; set; }
        public List<TROUGH> Troughs { get; set; }
        public bool IsChangedPrintCount { get; set; }
        public bool IsDone { get; set; }
        public int Tag { get; set; }
        private MessagesDetailModel _messagesDetailModel;
        public MessagesDetailModel MessagesDetailModel
        {
            get { return _messagesDetailModel; }
            set
            {
                if(SetProperty(ref _messagesDetailModel, value))
                {
                    if (_messagesDetailModel != null)
                    {
                        if (this.LEDTableFixed != null)
                        {
                            this.LEDTableFixed.Text1Info = _messagesDetailModel.CustomerName.ToUpper() + " - " + _messagesDetailModel.LicensePlate.ToUpper();
                            this.LEDTableFixed.Text2Info = _messagesDetailModel.Brand.ToUpper() + " - " + _messagesDetailModel.CementType.ToUpper() + " - " + _messagesDetailModel.BagCoverType.ToUpper();
                            this.LEDTableFixed.Text3Info = _messagesDetailModel.NumberBags + "";
                        }
                        if (this.TroughByUse != null)
                        {
                            this.TroughByUse.Text1Info = this.LEDTableFixed.Text1Info;
                            this.TroughByUse.Text2Info = this.LEDTableFixed.Text2Info;
                            this.TroughByUse.Text3Info = this.LEDTableFixed.Text3Info;
                        }
                        this.TextInfoSendToPrinter = _messagesDetailModel.CustomerCode + " " +
                                                     _messagesDetailModel.ManufacturingDate + " " +
                                                     _messagesDetailModel.LicensePlate;
                    }
                }
            }
        }
        public string StrTag { get; set; }
        private byte _lenghtMsgInRaster;
        public byte LenghtMsgInRaster
        {
            get => _lenghtMsgInRaster;
            set
            {
                SetProperty(ref _lenghtMsgInRaster, value);
            }
        }
        private byte _heightMsg;
        public byte HeightMsg
        {
            get => _heightMsg;
            set
            {
                SetProperty(ref _heightMsg, value);
            }
        }
        private byte _widthMsg;
        public byte WidthMsg
        {
            get => _widthMsg;
            set
            {
                SetProperty(ref _widthMsg, value);
            }
        }
        private byte _delay;
        public byte Delay
        {
            get => _delay;
            set
            {
                SetProperty(ref _delay, value);
            }
        }
        private byte _fieldOrientation;
        public byte FieldOrientation
        {
            get => _fieldOrientation;
            set
            {
                if(SetProperty(ref _fieldOrientation, value))
                {

                }
            }
        }
        private string _strFieldOrientation;
        public string StrFieldOrientation
        {
            get => _strFieldOrientation;
            set
            {
                if (SetProperty(ref _strFieldOrientation, value))
                {
                    switch (_strFieldOrientation)
                    {
                        case "Bình thường":
                            this.FieldOrientation = 0;
                            break;
                        case "Lật ngang":
                            this.FieldOrientation = 4;
                            break;
                        case "Lật dọc":
                            this.FieldOrientation = 8;
                            break;
                        case "Lật ngang + dọc":
                            this.FieldOrientation = 12;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }
        private bool _isUseTrough = true;
        public bool IsUseTrough
        {
            get => _isUseTrough;
            set
            {
                if (SetProperty(ref _isUseTrough, value))
                {
                    MainViewModel.Instance.MessagesDetailModel = null;
                    MainViewModel.Instance.MainView.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
                    }));
                    if (this._isUseTrough == false)
                    {
                        if (MainViewModel.Instance.TroughViewModel.TROUGHSelected != null)
                        {
                            MainViewModel.Instance.TroughViewModel.TROUGHSelected.IsSelected = false;
                            MainViewModel.Instance.TroughViewModel.TROUGHSelected = null;
                        }
                    }
                    if (this._isUseTrough == true)
                        MainViewModel.Instance.SetStateIsCanStart();
                }
            }
        }
        private PRINTER_STATES _printerState = PRINTER_STATES.DISCONNECTED;
        public PRINTER_STATES PRINTER_STATES
        {
            get { return _printerState; }
            set
            {
                if (SetProperty(ref _printerState, value))
                {
                    switch (_printerState)
                    {
                        case PRINTER_STATES.CONNECTED:
                            this.IsDone = false;
                            break;
                        case PRINTER_STATES.DISCONNECTED:
                            this.IsDone = false;
                            break;
                        case PRINTER_STATES.IS_CAN_PRINT:
                            this.IsDone = false;
                            break;
                        case PRINTER_STATES.PRINT_READY:
                            this.IsDone = false;
                            break;
                        case PRINTER_STATES.PRINTING:
                            this.IsDone = false;
                            break;
                        case PRINTER_STATES.PRINT_STOP:
                            this.IsDone = false;
                            break;
                        case PRINTER_STATES.PRINT_DONE:
                            this.IsDone = true;
                            this.MessagePrintCount = 0;
                            this.MessagePrintCountShow = 0;
                            this.NumberBag = 0;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private MESSAGE_STATE _messageState = MESSAGE_STATE.NO_DEFINE;
        public MESSAGE_STATE MESSAGE_STATE
        {
            get => _messageState;
            set
            {
                if (SetProperty(ref _messageState, value))
                {

                }
            }
        }

        private string _textInfoSendToPrinter;
        /// <summary>
        /// Thông tin gửi tới máy in để in
        /// </summary>
        public string TextInfoSendToPrinter
        {
            get { return _textInfoSendToPrinter; }
            set
            {
                SetProperty(ref _textInfoSendToPrinter, value);
            }
        }

        private int _messagePrintCount = 0;
        /// <summary>
        /// Số đếm của bản tin hiện tại
        /// </summary>
        public int MessagePrintCount
        {
            get => _messagePrintCount;
            set
            {
                if (SetProperty(ref _messagePrintCount, value))
                {
                    MessagePrintCountShow = _numberBag - _messagePrintCount;
                    this.IsChangedPrintCount = true;
                }
                else
                {
                    this.IsChangedPrintCount = false;
                }
            }
        }
        private uint _plcCount;
        public uint PlcCount
        {
            get => _plcCount;
            set
            {
                if(SetProperty(ref _plcCount, value))
                {

                }
            }
        }

        private int _messagePrintCountShow;
        /// <summary>
        /// Số đếm sẽ hiển thị lên giao diện
        /// </summary>
        public int MessagePrintCountShow
        {
            get => _messagePrintCountShow;
            set
            {
                SetProperty(ref _messagePrintCountShow, value);
            }
        }

        private int _numberBag;
        /// <summary>
        /// Số bao cần phải in
        /// </summary>
        public int NumberBag
        {
            get => _numberBag;
            set
            {
                SetProperty(ref _numberBag, value);
            }
        }

        private string _currentMessageOnPrinter;
        /// <summary>
        /// Bản tin hiện tại của máy in
        /// </summary>
        public string CurrentMessageOnPrinter
        {
            get => _currentMessageOnPrinter;
            set
            {
                SetProperty(ref _currentMessageOnPrinter, value);
            }
        }

        private string _notifyMsgOfPrinter = "[...]";
        /// <summary>
        /// Tin nhắn thông báo cho máy in
        /// </summary>
        public string NotifyMsgOfPrinter
        {
            get => _notifyMsgOfPrinter;
            set
            {
                SetProperty(ref _notifyMsgOfPrinter, value);
            }
        }

        private string _printerStatusDescription;
        /// <summary>
        /// Các trạng thái đọc được từ máy in
        /// </summary>
        public string PrinterStatusDescription
        {
            get => _printerStatusDescription;
            set
            {
                if (SetProperty(ref _printerStatusDescription, value))
                {

                }
            }
        }

        public string MessageName { get; set; } = @"THANHTHANGMSG";

        private CancellationTokenSource _cancellationTokenSource;
        private Task _taskRun;
        public void Start(int delay)
        {
            if (_taskRun != null && !_taskRun.IsCompleted)
                return;
            SemaphoreSlim initializationSemaphore = new SemaphoreSlim(0, 1);
            _cancellationTokenSource = new CancellationTokenSource();
            _taskRun = Task.Run(async () =>
            {
                try
                {
                    while (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        if (this.PRINTERClientSocket != null)
                        {
                            this.PRINTERClientSocket.SendMsg(CommandPrinter.CmdRequestMessagePrintCount);
                        }
                        if (initializationSemaphore != null)
                            initializationSemaphore.Release();
                        await Task.Delay(delay);
                    }
                }
                finally
                {
                    if (initializationSemaphore != null)
                        initializationSemaphore.Release();
                }
            }, _cancellationTokenSource.Token);

            //await initializationSemaphore.WaitAsync();
            initializationSemaphore.Dispose();
            initializationSemaphore = null;

            if (_taskRun.IsFaulted)
            {
                //await _taskRun;
            }
        }
        public void Stop()
        {
            try
            {
                if (_cancellationTokenSource == null) return;
                if (_cancellationTokenSource.IsCancellationRequested)
                    return;
                if (!_taskRun.IsCompleted)
                {
                    _cancellationTokenSource.Cancel();
                    //await _taskRun;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand ConnectPrinterCmd { get; }
        public ICommand DisconnectPrinterCmd { get; }
        public ICommand ToggleCheckedCmd { get; }
        public ICommand ToggleUncheckedCmd { get; }
        public ICommand PreviewMouseLeftButtonDownCmd { get; }
    }
    public class TROUGH : BaseModel
    {
        public TROUGH()
        {

        }
        public int PORT_TROUGH { get; set; }
        public string IPAddress { get; set; }
        public IntPtr SendParam { get; set; }
        public string TroughName { get; set; }
        public PRINTER PrinterLead { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }
        public int Tag { get; set; }
        public bool IsLEDFix { get; set; }

        private TROUGH_STATES _troughState = TROUGH_STATES.NO_DEFINE;
        public TROUGH_STATES TROUGH_STATES
        {
            get { return _troughState; }
            set
            {
                if (SetProperty(ref _troughState, value))
                {
                    if (this.TROUGH_STATES == TROUGH_STATES.ACTIVE_NOREADY && this.IsLEDFix == false)
                    {
                        if (MessageBox.Show("Bảng LED mất kết nối hoặc đã hỏng!\nBạn có muốn BỎ CHẠY chế độ LIÊN ĐỘNG?", "", MessageBoxButton.YesNo,
                                           MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (TroughViewModel.Instance.TROUGHSelected != null)
                                TroughViewModel.Instance.TROUGHSelected.PrinterLead.IsUseTrough = false;
                        }
                    }
                }
            }
        }
        private string _text1Info;
        public string Text1Info
        {
            get { return _text1Info; }
            set
            {
                SetProperty(ref _text1Info, value);
            }
        }
        private string _text2Info;
        public string Text2Info
        {
            get { return _text2Info; }
            set
            {
                SetProperty(ref _text2Info, value);
            }
        }
        private string _text3Info;
        public string Text3Info
        {
            get { return _text3Info; }
            set
            {
                SetProperty(ref _text3Info, value);
            }
        }
    }
}
