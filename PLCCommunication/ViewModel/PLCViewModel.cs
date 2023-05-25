using log4net.Repository.Hierarchy;
using NTech.Base.Commons.BaseViewModel;
using NTech.Xm.Commons.Defines;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace PLCCommunication.ViewModel
{
    public class PLCViewModel : ViewModelBase
    {
        public static readonly string PLCPath = System.IO.Path.Combine(Environment.CurrentDirectory, "PLC.xml");
        public log4net.ILog Logger { get; } = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dispatcher _dispatcher;
        public static PLCViewModel Instance { get; private set; }
        public PLCModel PLCModel { get; private set; }
        public PLCViewModel(Dispatcher dispatcher) 
        {
            if (Instance is null)
                Instance = this;
            else
                return;

            _dispatcher= dispatcher;
            PLCModel = new PLCModel();

            CheckLineWithIPPC();
            LoadPLCXml();
        }

        public void CheckLineWithIPPC()
        {
            if (CommonDefines.GetLocalIPv4Addresses().Equals(CommonDefines.IP_PC_LINE2))
            {
                PLCModel.PLCLine = "PLC LINE2";
            }
            else
            {
                PLCModel.PLCLine = "PLC LINE3";
            }
        }
        public void LoadPLCXml()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(PLCPath);
            XmlNode nodeUsePLC = xml.DocumentElement.SelectSingleNode("//UsePLC");
            if (nodeUsePLC.InnerText.ToUpper().Equals("TRUE"))
            {
                PLCModel.UsePLC = true;
            }
            else
            {
                PLCModel.UsePLC = false;
            }
            if (!PLCModel.UsePLC)
            {
                return;
            }
            Logger.Info("UsePLC = True");
            if (!string.IsNullOrEmpty(PLCModel.PLCLine))
            {
                if (PLCModel.PLCLine.Equals("PLC LINE2"))
                {
                    GetInfosPLC(xml, 2);
                }
                else if (PLCModel.PLCLine.Equals("PLC LINE3"))
                {
                    GetInfosPLC(xml, 3);
                }
                else
                {
                    return;
                }
            }
        }
        void GetInfosPLC(XmlDocument xml, int indexLINE)
        {
            PLCModel.IP = xml.SelectSingleNode("//PLCLine" + indexLINE).Attributes["IP"].Value.ToString();
            PLCModel.Slot = Convert.ToInt16(xml.SelectSingleNode("//PLCLine" + indexLINE).Attributes["Slot"].Value.ToString());
            PLCModel.Rack = Convert.ToInt16(xml.SelectSingleNode("//PLCLine" + indexLINE).Attributes["Rack"].Value.ToString());

            //Get list register for counter
            XmlNodeList nodelistRegister = xml.DocumentElement.SelectNodes($"//PLC//PLCLine{indexLINE}//Register");
            PLCModel.DicRegister = new Dictionary<string, string>();
            foreach (XmlNode node in nodelistRegister)
            {
                string key = node.Attributes["id"].Value.ToString();
                string value = node.Attributes["name"].Value.ToString();
                PLCModel.DicRegister.Add(key, value);
            }

            //Get list bit reset counter
            XmlNodeList nodelistBitReset = xml.DocumentElement.SelectNodes($"//PLC//PLCLine{indexLINE}//BitReset");
            PLCModel.DicBitReset = new Dictionary<string, string>();
            foreach (XmlNode node in nodelistBitReset)
            {
                string key = node.Attributes["id"].Value.ToString();
                string value = node.Attributes["name"].Value.ToString();
                PLCModel.DicBitReset.Add(key, value);
            }
            Logger.Info("Read Infos PLC from Xml success!");
        }

        public bool ConnectPLC()
        {
            PLCModel.PLCs71200 = new Plc(CpuType.S71200, PLCModel.IP, PLCModel.Rack, PLCModel.Slot);
            if (PLCModel.PLCs71200.Open() == ErrorCode.NoError)
            {
                PLCModel.PLCConnected = true;
            }
            else
            {
                PLCModel.PLCConnected = false;
            }
            return PLCModel.PLCConnected;
        }
    }
}
