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

        private Thread _threadReadPLC;
        public Thread ThreadReadPLC
        {
            get => _threadReadPLC;
        }

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
        private int _count1;
        public int Count1
        {
            get { return _count1; }
            set
            {
                SetProperty(ref _count1, value);
            }
        }
        private int _count2;
        public int Count2
        {
            get { return _count2; }
            set
            {
                SetProperty(ref _count2, value);
            }
        }
        private int _count3;
        public int Count3
        {
            get { return _count3; }
            set
            {
                SetProperty(ref _count3, value);
            }
        }
        private int _count4;
        public int Count4
        {
            get { return _count4; }
            set
            {
                SetProperty(ref _count4, value);
            }
        }
        private int _count5;
        public int Count5
        {
            get { return _count5; }
            set
            {
                SetProperty(ref _count5, value);
            }
        }
        private int _count6;
        public int Count6
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
        public List<Dictionary<string, string>> ListDicRegister { get; set; }
        public List<Dictionary<string, string>> ListDicBitReset { get; set; }
        public Plc PLCInstance { get; set; }
        public PLC()
        {
            CheckLineWithIPPC();
            LoadPLCXml();
#if TEST
            ConnectPLC();
#endif
            InitServer();
        }
        ~PLC()
        {

        }
        ErrorCode ConnectPLC()
        {
            PLCInstance = new Plc(CpuType.S71200, IP, Rack, Slot);
            ErrorCode errorCode = PLCInstance.Open();
            if (PLCInstance.IsConnected)
            {
                _plcConnected = true;
            }
            else
            {
                _plcConnected = false;
                MessageBox.Show(errorCode.ToString());
                return errorCode;
            }
            return errorCode;
        }
        void ReadPLC()
        {
            while (true)
            {

            }
        }
        void InitServer()
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
                    break;
                default:
                    break;
            }
        }

        void CheckLineWithIPPC()
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
            ListDicRegister = new List<Dictionary<string, string>>();
            foreach (XmlNode node in nodelistRegister)
            {
                string key = node.Attributes["id"].Value.ToString();
                string value = node.Attributes["name"].Value.ToString();
                Dictionary<string, string> dic1 = new Dictionary<string, string>();
                dic1.Add(key, value);
                ListDicRegister.Add(dic1);
            }

            //Get list bit reset counter
            XmlNodeList nodelistBitReset = xml.DocumentElement.SelectNodes($"//PLC//PLCLine{indexLINE}//BitReset");
            ListDicBitReset = new List<Dictionary<string, string>>();
            foreach (XmlNode node in nodelistBitReset)
            {
                string key = node.Attributes["id"].Value.ToString();
                string value = node.Attributes["name"].Value.ToString();
                Dictionary<string, string> dic2 = new Dictionary<string, string>();
                dic2.Add(key, value);
                ListDicBitReset.Add(dic2);
            }
        }
        void LoadPLCXml()
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
    }
}
