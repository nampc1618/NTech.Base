#define TEST

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using NTech.Base.Commons.BaseModel;
using NTech.Base.Resources.NNetSocket;
using NTech.Xm.Commons.Defines;
using PLCCommunication.ViewModel;
using S7.Net;

namespace PLCCommunication
{
    public class PLCModel : BaseModel
    {
        public log4net.ILog Logger { get; } = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool _plcConnected;
        public bool PLCConnected
        {
            get { return _plcConnected; }
            set
            {
                SetProperty(ref _plcConnected, value);
            }
        }
        private string _statusServer;
        public string StatusServer
        {
            get => _statusServer;
            set
            {
               if(SetProperty(ref _statusServer, value))
                {

                }
            }
        }
        private uint _count1;
        public uint Count1
        {
            get { return _count1; }
            set
            {
                if(SetProperty(ref _count1, value))
                {
                    MainViewModel.Instance.ShowData(MainViewModel.Instance.MainView.lbCountNumberPLC1, _count1.ToString());
                }
            }
        }
        private uint _count2;
        public uint Count2
        {
            get { return _count2; }
            set
            {
                if(SetProperty(ref _count2, value))
                {
                    MainViewModel.Instance.ShowData(MainViewModel.Instance.MainView.lbCountNumberPLC2, _count2.ToString());
                }
            }
        }
        private uint _count3;
        public uint Count3
        {
            get { return _count3; }
            set
            {
                if (SetProperty(ref _count3, value))
                {
                    MainViewModel.Instance.ShowData(MainViewModel.Instance.MainView.lbCountNumberPLC3, _count3.ToString());
                }
            }
        }
        private uint _count4;
        public uint Count4
        {
            get { return _count4; }
            set
            {
                if (SetProperty(ref _count4, value))
                {
                    MainViewModel.Instance.ShowData(MainViewModel.Instance.MainView.lbCountNumberPLC4, _count4.ToString());
                }
            }
        }
        private uint _count5;
        public uint Count5
        {
            get { return _count5; }
            set
            {
                if (SetProperty(ref _count5, value))
                {
                    MainViewModel.Instance.ShowData(MainViewModel.Instance.MainView.lbCountNumberPLC5, _count5.ToString());
                }
            }
        }
        private uint _count6;
        public uint Count6
        {
            get { return _count6; }
            set
            {
                if (SetProperty(ref _count6, value))
                {
                    MainViewModel.Instance.ShowData(MainViewModel.Instance.MainView.lbCountNumberPLC6, _count6.ToString());
                }
            }
        }

        public string IP { get; set; }
        public short Slot { get; set; }
        public short Rack { get; set; }
        private bool _usePLC;
        public bool UsePLC
        {
            get => _usePLC;
            set
            {
                SetProperty(ref _usePLC, value);
            }
        }
        public string PLCLine { get; set; }
        public Dictionary<string, string> DicRegister { get; set; }
        public Dictionary<string, string> DicBitReset { get; set; }
        public Plc PLCs71200 { get; set; }
        public PLCModel()
        {
        }

        public Task SetBit(string addressBit)
        {
            return Task.Run(async () =>
            {
                if (PLCs71200.IsConnected)
                {
                    PLCs71200.Write(addressBit, 1);
                    await Task.Delay(2);
                }
            });
        }

        private CancellationTokenSource _cancellationTokenSource;
        private Task _taskRun;
        public void StartProcess(int delay)
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
                        Count1 = (uint)PLCs71200.Read(DicRegister["1"]);
                        Count2 = (uint)PLCs71200.Read(DicRegister["2"]);
                        Count3 = (uint)PLCs71200.Read(DicRegister["3"]);
                        Count4 = (uint)PLCs71200.Read(DicRegister["4"]);
                        Count5 = (uint)PLCs71200.Read(DicRegister["5"]);
                        Count6 = (uint)PLCs71200.Read(DicRegister["6"]);

                        string data = Count1 + "," + Count2+ "," + Count3 + "," + Count4 + "," + Count5 + "," + Count6;
                        MainViewModel.Instance.MainServerSocket.SendMsg(data);

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
        public void StopProcess()
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
    }
}
