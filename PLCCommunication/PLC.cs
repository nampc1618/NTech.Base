#define TEST

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using NTech.Base.Commons.BaseModel;
using NTech.Base.Resources.NNetSocket;
using NTech.Xm.Commons.Defines;
using S7.Net;

namespace PLCCommunication
{
    public class PLC : BaseModel
    {
        public NServerSocket NServerSocket { get; set; }
        private int PORT = 8500;

        public static readonly string PLCPath = Path.Combine(Environment.CurrentDirectory, "PLC.xml");
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
                SetProperty(ref _statusServer, value);
            }
        }
        private uint _count1;
        public uint Count1
        {
            get { return _count1; }
            set
            {
                SetProperty(ref _count1, value);
            }
        }
        private uint _count2;
        public uint Count2
        {
            get { return _count2; }
            set
            {
                SetProperty(ref _count2, value);
            }
        }
        private uint _count3;
        public uint Count3
        {
            get { return _count3; }
            set
            {
                SetProperty(ref _count3, value);
            }
        }
        private uint _count4;
        public uint Count4
        {
            get { return _count4; }
            set
            {
                SetProperty(ref _count4, value);
            }
        }
        private uint _count5;
        public uint Count5
        {
            get { return _count5; }
            set
            {
                SetProperty(ref _count5, value);
            }
        }
        private uint _count6;
        public uint Count6
        {
            get { return _count6; }
            set
            {
                SetProperty(ref _count6, value);
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
        public Plc PLCInstance { get; set; }
        public PLC()
        {
            //CheckLineWithIPPC();
            //LoadPLCXml();
#if TEST
            //if(ConnectPLC() == 0)
            //{
            //    this.Start(200);
            //}
#endif
            //InitServer();
        }
        public bool ConnectPLC()
        {
            PLCInstance = new Plc(CpuType.S71200, IP, Rack, Slot);
            if (PLCInstance.Open() == ErrorCode.NoError)
            {
                PLCConnected = true;
            }
            else
            {
                PLCConnected = false;
            }
            return PLCConnected;
        }

        public void InitServer()
        {
            NServerSocket = new NServerSocket();
            NServerSocket.ConnectionEventCallback += NServerSocket_ConnectionEventCallback;
            NServerSocket.ServerErrorEventCallback += NServerSocket_ServerErrorEventCallback;
            NServerSocket.StartListening(PORT);
        }

        private void NServerSocket_ServerErrorEventCallback(string errorMsg)
        {

        }

        private void NServerSocket_ConnectionEventCallback(NServerSocket.EConnectionEventServer e, object obj)
        {
            
            switch (e)
            {
                case NServerSocket.EConnectionEventServer.SERVER_LISTEN:
                    _statusServer = "ServicePLC Started";
                    break;
                case NServerSocket.EConnectionEventServer.SERVER_RECEIVEDATA:
                    if(NServerSocket.ReceiveString != null)
                    {
                        string dataReceived = NServerSocket.ReceiveString;
                        //syntax: StrTag + "," + "Rst" . For example: L3P1,Rst
                        string[] arr = dataReceived.Split(new char[] { ',' });
                        if (arr[0] != null && arr[1].Equals("Rst"))
                        {
                            switch(arr[0])
                            {
                                //Line 2
                                case "L2P1":
                                    PLCInstance.Write(DicBitReset["1"], 0);
                                    break;
                                case "L2P2":
                                    PLCInstance.Write(DicBitReset["2"], 0);
                                    break;
                                case "L2P3":
                                    PLCInstance.Write(DicBitReset["3"], 0);
                                    break;
                                case "L2P4":
                                    PLCInstance.Write(DicBitReset["4"], 0);
                                    break;
                                case "L2P5":
                                    PLCInstance.Write(DicBitReset["5"], 0);
                                    break;
                                case "L2P6":
                                    PLCInstance.Write(DicBitReset["6"], 0);
                                    break;
                                //Line 3
                                case "L3P1":
                                    PLCInstance.Write(DicBitReset["1"], 0);
                                    break;
                                case "L3P2":
                                    PLCInstance.Write(DicBitReset["2"], 0);
                                    break;
                                case "L3P3":
                                    PLCInstance.Write(DicBitReset["3"], 0);
                                    break;
                                case "L3P4":
                                    PLCInstance.Write(DicBitReset["4"], 0);
                                    break;
                                case "L3P5":
                                    PLCInstance.Write(DicBitReset["5"], 0);
                                    break;
                                case "L3P6":
                                    PLCInstance.Write(DicBitReset["6"], 0);
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void CheckLineWithIPPC()
        {
            if (CommonDefines.GetLocalIPv4Addresses().Equals(CommonDefines.IP_PC_LINE2))
            {
                PLCLine = "PLC LINE2";
            }
            else
            {
                PLCLine = "PLC LINE3";
            }
        }
        void GetInfosPLC(XmlDocument xml, int indexLINE)
        {
            IP = xml.SelectSingleNode("//PLCLine" + indexLINE).Attributes["IP"].Value.ToString();
            Slot = Convert.ToInt16(xml.SelectSingleNode("//PLCLine" + indexLINE).Attributes["Slot"].Value.ToString());
            Rack = Convert.ToInt16(xml.SelectSingleNode("//PLCLine" + indexLINE).Attributes["Rack"].Value.ToString());

            //Get list register for counter
            XmlNodeList nodelistRegister = xml.DocumentElement.SelectNodes($"//PLC//PLCLine{indexLINE}//Register");
            DicRegister = new Dictionary<string, string>();
            foreach (XmlNode node in nodelistRegister)
            {
                string key = node.Attributes["id"].Value.ToString();
                string value = node.Attributes["name"].Value.ToString();
                DicRegister.Add(key, value);
            }

            //Get list bit reset counter
            XmlNodeList nodelistBitReset = xml.DocumentElement.SelectNodes($"//PLC//PLCLine{indexLINE}//BitReset");
            DicBitReset = new Dictionary<string, string>();
            foreach (XmlNode node in nodelistBitReset)
            {
                string key = node.Attributes["id"].Value.ToString();
                string value = node.Attributes["name"].Value.ToString();
                DicBitReset.Add(key, value);
            }
            Logger.Info("Read Infos PLC from Xml success!");
        }
        public void LoadPLCXml()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(PLCPath);
            XmlNode nodeUsePLC = xml.DocumentElement.SelectSingleNode("//UsePLC");
            if (nodeUsePLC.InnerText.ToUpper().Equals("TRUE"))
            {
                UsePLC = true;
            }
            else
            {
                UsePLC = false;
            }
            if (!UsePLC)
            {
                return;
            }
            Logger.Info("UsePLC = True");
            if (!string.IsNullOrEmpty(PLCLine))
            {
                if (PLCLine.Equals("PLC LINE2"))
                {
                    GetInfosPLC(xml, 2);
                }
                else if (PLCLine.Equals("PLC LINE3"))
                {
                    GetInfosPLC(xml, 3);
                }
                else
                {
                    return;
                }
            }
        }
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
                        Logger.Info("Start read PLC");
                        Count1 = (uint)PLCInstance.Read(DicRegister["1"]);
                        Count2 = (uint)PLCInstance.Read(DicRegister["2"]);
                        Count3 = (uint)PLCInstance.Read(DicRegister["3"]);
                        Count4 = (uint)PLCInstance.Read(DicRegister["4"]);
                        Count5 = (uint)PLCInstance.Read(DicRegister["5"]);
                        Count6 = (uint)PLCInstance.Read(DicRegister["6"]);

                        Logger.InfoFormat("Count1 = {0}, Count2 = {1}, Count3 = {2}", Count1, Count2, Count3);
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
    }
}
