using Microsoft.Xaml.Behaviors.Media;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.Models;
using NTech.Xm.Station.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace NTech.Xm.Station.ViewModels
{
    public class TroughViewModel : ViewModelBase
    {
        #region Troughs
        public TROUGH TROUGH1_PRINTER1_LINE1 { get; }
        public TROUGH TROUGH2_PRINTER1_LINE1 { get; }
        public TROUGH TROUGH3_PRINTER1_LINE1 { get; }
        public TROUGH TROUGH1_PRINTER2_LINE1 { get; }
        public TROUGH TROUGH2_PRINTER2_LINE1 { get; }
        public TROUGH TROUGH3_PRINTER2_LINE1 { get; }

        public TROUGH TROUGH1_LINE2 { get; }
        public TROUGH TROUGH2_LINE2 { get; }
        public TROUGH TROUGH3_LINE2 { get; }
        public TROUGH TROUGH4_LINE2 { get; }
        public TROUGH TROUGH5_LINE2 { get; }
        public TROUGH TROUGH6_LINE2 { get; }
        public TROUGH TROUGH7_LINE2 { get; }
        public TROUGH TROUGH8_LINE2 { get; }
        public TROUGH TROUGH9_LINE2 { get; }

        public TROUGH TROUGH1_PRINTER5_LINE3 { get; }
        public TROUGH TROUGH2_PRINTER5_LINE3 { get; }
        public TROUGH TROUGH3_PRINTER5_LINE3 { get; }
        public TROUGH TROUGH1_PRINTER6_LINE3 { get; }
        public TROUGH TROUGH2_PRINTER6_LINE3 { get; }
        public TROUGH TROUGH3_PRINTER6_LINE3 { get; }
        public TROUGH TROUGH4_PRINTER3_LINE3 { get; }
        public TROUGH TROUGH5_PRINTER3_LINE3 { get; }
        public TROUGH TROUGH6_PRINTER3_LINE3 { get; }
        public TROUGH TROUGH4_PRINTER4_LINE3 { get; }
        public TROUGH TROUGH5_PRINTER4_LINE3 { get; }
        public TROUGH TROUGH6_PRINTER4_LINE3 { get; }
        public TROUGH TROUGH7_PRINTER1_LINE3 { get; }
        public TROUGH TROUGH8_PRINTER1_LINE3 { get; }
        public TROUGH TROUGH9_PRINTER1_LINE3 { get; }
        public TROUGH TROUGH7_PRINTER2_LINE3 { get; }
        public TROUGH TROUGH8_PRINTER2_LINE3 { get; }
        public TROUGH TROUGH9_PRINTER2_LINE3 { get; }

        public TROUGH LED_FIXED_1 { get; }
        public TROUGH LED_FIXED_2 { get; }
        public TROUGH LED_FIXED_3 { get; }
        public TROUGH LED_FIXED_4 { get; }
        public TROUGH LED_FIXED_5 { get; }
        public TROUGH LED_FIXED_6 { get; }
        public TROUGH LED_FIXED_7 { get; }
        public TROUGH LED_FIXED_8 { get; }
        public TROUGH LED_FIXED_9 { get; }
        public TROUGH LED_FIXED_10 { get; }
        public TROUGH LED_FIXED_11 { get; }
        public TROUGH LED_FIXED_12 { get; }
        public TROUGH LED_FIXED_13 { get; }
        public TROUGH LED_FIXED_14 { get; }

        #endregion

        #region Variables
        IntPtr pNULL = new IntPtr(0);
        int nErrorCode = -1;
        int nProgramID;
        int nAreaID1, nAreaID2, nAreaID3;
        #endregion

        public PrinterViewModel PrinterViewModel { get; }

        public static TroughViewModel Instance { get; private set; }

        public TroughViewModel(PrinterViewModel printerViewModel)
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                return;
            }

            this.PrinterViewModel = printerViewModel;
            this.IPTroughList = DeserializeTroughXML();

            this.LED_FIXED_1 = new TROUGH() { TroughName = "MA1_LINE1", Tag = 1, IPAddress = IPTroughList.IPAddressTroughList[0], IsLEDFix = true };
            this.LED_FIXED_2 = new TROUGH() { TroughName = "MA2_LINE1", Tag = 2, IPAddress = IPTroughList.IPAddressTroughList[1], IsLEDFix = true };

            this.LED_FIXED_3 = new TROUGH() { TroughName = "MA1_LINE2", Tag = 3, IPAddress = IPTroughList.IPAddressTroughList[2], IsLEDFix = true };
            this.LED_FIXED_4 = new TROUGH() { TroughName = "MA2_LINE2", Tag = 4, IPAddress = IPTroughList.IPAddressTroughList[3], IsLEDFix = true };
            this.LED_FIXED_5 = new TROUGH() { TroughName = "MA3_LINE2", Tag = 5, IPAddress = IPTroughList.IPAddressTroughList[4], IsLEDFix = true };
            this.LED_FIXED_6 = new TROUGH() { TroughName = "MA4_LINE2", Tag = 6, IPAddress = IPTroughList.IPAddressTroughList[5], IsLEDFix = true };
            this.LED_FIXED_7 = new TROUGH() { TroughName = "MA5_LINE2", Tag = 7, IPAddress = IPTroughList.IPAddressTroughList[6], IsLEDFix = true };
            this.LED_FIXED_8 = new TROUGH() { TroughName = "MA6_LINE2", Tag = 8, IPAddress = IPTroughList.IPAddressTroughList[7], IsLEDFix = true };

            this.LED_FIXED_9 = new TROUGH() { TroughName = "MA1_LINE3", Tag = 9, IPAddress = IPTroughList.IPAddressTroughList[8], IsLEDFix = true };
            this.LED_FIXED_10 = new TROUGH() { TroughName = "MA2_LINE3", Tag = 10, IPAddress = IPTroughList.IPAddressTroughList[9], IsLEDFix = true };
            this.LED_FIXED_11 = new TROUGH() { TroughName = "MA3_LINE3", Tag = 11, IPAddress = IPTroughList.IPAddressTroughList[10], IsLEDFix = true };
            this.LED_FIXED_12 = new TROUGH() { TroughName = "MA4_LINE3", Tag = 12, IPAddress = IPTroughList.IPAddressTroughList[11], IsLEDFix = true };
            this.LED_FIXED_13 = new TROUGH() { TroughName = "MA5_LINE3", Tag = 13, IPAddress = IPTroughList.IPAddressTroughList[12], IsLEDFix = true };
            this.LED_FIXED_14 = new TROUGH() { TroughName = "MA6_LINE3", Tag = 14, IPAddress = IPTroughList.IPAddressTroughList[13], IsLEDFix = true };

            this.TROUGH1_PRINTER1_LINE1 = new TROUGH() { TroughName = "MÁNG 1", Tag = 15, IPAddress = IPTroughList.IPAddressTroughList[14], PrinterLead = PrinterViewModel.PRINTER1_LINE1 };
            this.TROUGH2_PRINTER1_LINE1 = new TROUGH() { TroughName = "MÁNG 2", Tag = 16, IPAddress = IPTroughList.IPAddressTroughList[15], PrinterLead = PrinterViewModel.PRINTER1_LINE1 };
            this.TROUGH3_PRINTER1_LINE1 = new TROUGH() { TroughName = "MÁNG 3", Tag = 17, IPAddress = IPTroughList.IPAddressTroughList[16], PrinterLead = PrinterViewModel.PRINTER1_LINE1 };
            this.TROUGH1_PRINTER2_LINE1 = new TROUGH() { TroughName = "MÁNG 1", Tag = 15, IPAddress = IPTroughList.IPAddressTroughList[14], PrinterLead = PrinterViewModel.PRINTER2_LINE1 };
            this.TROUGH2_PRINTER2_LINE1 = new TROUGH() { TroughName = "MÁNG 2", Tag = 16, IPAddress = IPTroughList.IPAddressTroughList[15], PrinterLead = PrinterViewModel.PRINTER2_LINE1 };
            this.TROUGH3_PRINTER2_LINE1 = new TROUGH() { TroughName = "MÁNG 3", Tag = 17, IPAddress = IPTroughList.IPAddressTroughList[16], PrinterLead = PrinterViewModel.PRINTER2_LINE1 };

            this.TROUGH1_LINE2 = new TROUGH() { TroughName = "MÁNG 1", Tag = 18, IPAddress = IPTroughList.IPAddressTroughList[25], PrinterLead = PrinterViewModel.PRINTER6_LINE2 };
            this.TROUGH2_LINE2 = new TROUGH() { TroughName = "MÁNG 2", Tag = 19, IPAddress = IPTroughList.IPAddressTroughList[24], PrinterLead = PrinterViewModel.PRINTER6_LINE2 };
            this.TROUGH3_LINE2 = new TROUGH() { TroughName = "MÁNG 3", Tag = 20, IPAddress = IPTroughList.IPAddressTroughList[23], PrinterLead = PrinterViewModel.PRINTER5_LINE2 };
            this.TROUGH4_LINE2 = new TROUGH() { TroughName = "MÁNG 4", Tag = 21, IPAddress = IPTroughList.IPAddressTroughList[22], PrinterLead = PrinterViewModel.PRINTER5_LINE2 };
            this.TROUGH5_LINE2 = new TROUGH() { TroughName = "MÁNG 5", Tag = 22, IPAddress = IPTroughList.IPAddressTroughList[21], PrinterLead = PrinterViewModel.PRINTER4_LINE2 };
            this.TROUGH6_LINE2 = new TROUGH() { TroughName = "MÁNG 6", Tag = 23, IPAddress = IPTroughList.IPAddressTroughList[20], PrinterLead = PrinterViewModel.PRINTER4_LINE2 };
            this.TROUGH7_LINE2 = new TROUGH() { TroughName = "MÁNG 7", Tag = 24, IPAddress = IPTroughList.IPAddressTroughList[19], PrinterLead = PrinterViewModel.PRINTER3_LINE2 };
            this.TROUGH8_LINE2 = new TROUGH() { TroughName = "MÁNG 8", Tag = 25, IPAddress = IPTroughList.IPAddressTroughList[18], PrinterLead = PrinterViewModel.PRINTER2_LINE2 };
            this.TROUGH9_LINE2 = new TROUGH() { TroughName = "MÁNG 9", Tag = 26, IPAddress = IPTroughList.IPAddressTroughList[17], PrinterLead = PrinterViewModel.PRINTER1_LINE2 };

            this.TROUGH1_PRINTER5_LINE3 = new TROUGH() { TroughName = "MÁNG 1", Tag = 27, IPAddress = IPTroughList.IPAddressTroughList[34], PrinterLead = PrinterViewModel.PRINTER5_LINE3 };
            this.TROUGH2_PRINTER5_LINE3 = new TROUGH() { TroughName = "MÁNG 2", Tag = 28, IPAddress = IPTroughList.IPAddressTroughList[33], PrinterLead = PrinterViewModel.PRINTER5_LINE3 };
            this.TROUGH3_PRINTER5_LINE3 = new TROUGH() { TroughName = "MÁNG 3", Tag = 29, IPAddress = IPTroughList.IPAddressTroughList[32], PrinterLead = PrinterViewModel.PRINTER5_LINE3 };
            this.TROUGH1_PRINTER6_LINE3 = new TROUGH() { TroughName = "MÁNG 1", Tag = 27, IPAddress = IPTroughList.IPAddressTroughList[34], PrinterLead = PrinterViewModel.PRINTER6_LINE3 };
            this.TROUGH2_PRINTER6_LINE3 = new TROUGH() { TroughName = "MÁNG 2", Tag = 28, IPAddress = IPTroughList.IPAddressTroughList[33], PrinterLead = PrinterViewModel.PRINTER6_LINE3 };
            this.TROUGH3_PRINTER6_LINE3 = new TROUGH() { TroughName = "MÁNG 3", Tag = 29, IPAddress = IPTroughList.IPAddressTroughList[32], PrinterLead = PrinterViewModel.PRINTER6_LINE3 };

            this.TROUGH4_PRINTER3_LINE3 = new TROUGH() { TroughName = "MÁNG 4", Tag = 30, IPAddress = IPTroughList.IPAddressTroughList[31], PrinterLead = PrinterViewModel.PRINTER3_LINE3 };
            this.TROUGH5_PRINTER3_LINE3 = new TROUGH() { TroughName = "MÁNG 5", Tag = 31, IPAddress = IPTroughList.IPAddressTroughList[30], PrinterLead = PrinterViewModel.PRINTER3_LINE3 };
            this.TROUGH6_PRINTER3_LINE3 = new TROUGH() { TroughName = "MÁNG 6", Tag = 32, IPAddress = IPTroughList.IPAddressTroughList[29], PrinterLead = PrinterViewModel.PRINTER3_LINE3 };
            this.TROUGH4_PRINTER4_LINE3 = new TROUGH() { TroughName = "MÁNG 4", Tag = 30, IPAddress = IPTroughList.IPAddressTroughList[31], PrinterLead = PrinterViewModel.PRINTER4_LINE3 };
            this.TROUGH5_PRINTER4_LINE3 = new TROUGH() { TroughName = "MÁNG 5", Tag = 31, IPAddress = IPTroughList.IPAddressTroughList[30], PrinterLead = PrinterViewModel.PRINTER4_LINE3 };
            this.TROUGH6_PRINTER4_LINE3 = new TROUGH() { TroughName = "MÁNG 6", Tag = 32, IPAddress = IPTroughList.IPAddressTroughList[29], PrinterLead = PrinterViewModel.PRINTER4_LINE3 };

            this.TROUGH7_PRINTER1_LINE3 = new TROUGH() { TroughName = "MÁNG 7", Tag = 33, IPAddress = IPTroughList.IPAddressTroughList[28], PrinterLead = PrinterViewModel.PRINTER1_LINE3 };
            this.TROUGH8_PRINTER1_LINE3 = new TROUGH() { TroughName = "MÁNG 8", Tag = 34, IPAddress = IPTroughList.IPAddressTroughList[27], PrinterLead = PrinterViewModel.PRINTER1_LINE3 };
            this.TROUGH9_PRINTER1_LINE3 = new TROUGH() { TroughName = "MÁNG 9", Tag = 35, IPAddress = IPTroughList.IPAddressTroughList[26], PrinterLead = PrinterViewModel.PRINTER1_LINE3 };
            this.TROUGH7_PRINTER2_LINE3 = new TROUGH() { TroughName = "MÁNG 7", Tag = 33, IPAddress = IPTroughList.IPAddressTroughList[28], PrinterLead = PrinterViewModel.PRINTER2_LINE3 };
            this.TROUGH8_PRINTER2_LINE3 = new TROUGH() { TroughName = "MÁNG 8", Tag = 34, IPAddress = IPTroughList.IPAddressTroughList[27], PrinterLead = PrinterViewModel.PRINTER2_LINE3 };
            this.TROUGH9_PRINTER2_LINE3 = new TROUGH() { TroughName = "MÁNG 9", Tag = 35, IPAddress = IPTroughList.IPAddressTroughList[26], PrinterLead = PrinterViewModel.PRINTER2_LINE3 };


            this.AddTROUGHList();
            this.AddLEDFIXEDList();
            this.AddSendParam();
            //this.AddTroughsName();
            this.AddTroughInPrinter();

            this._troughsName = new List<string>();
        }
        #region Properties
        public IPTroughList IPTroughList { get; set; }

        List<TROUGH> _troughList = new List<TROUGH>();
        public List<TROUGH> TROUGHList
        {
            get { return _troughList; }
            set
            {
                Set(ref _troughList, value);
            }
        }
        public List<TROUGH> LEDFIXEDList { get; set; }

        private List<string> _troughsName = new List<string>();
        public List<string> TROUGHSName
        {
            get { return _troughsName; }
            set
            {
                Set(ref _troughsName, value);
            }
        }

        private TROUGH _troughSelected;
        public TROUGH TROUGHSelected
        {
            get { return _troughSelected; }
            set
            {
                if (Set(ref _troughSelected, value))
                {
                    //SetTextInitalForLEDTable();
                }
            }
        }
        #endregion

        #region Methods
        public IPTroughList DeserializeTroughXML()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(IPTroughList));
                using (var reader = new StreamReader(Define.IPTroughPath))
                {
                    var config = (IPTroughList)serializer.Deserialize(reader);
                    return config;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        void AddTROUGHList()
        {
            this._troughList = new List<TROUGH>();

            this.TROUGHList.Add(TROUGH1_PRINTER1_LINE1);
            this.TROUGHList.Add(TROUGH2_PRINTER1_LINE1);
            this.TROUGHList.Add(TROUGH3_PRINTER1_LINE1);
            this.TROUGHList.Add(TROUGH1_PRINTER2_LINE1);
            this.TROUGHList.Add(TROUGH2_PRINTER2_LINE1);
            this.TROUGHList.Add(TROUGH3_PRINTER2_LINE1);

            this.TROUGHList.Add(TROUGH1_LINE2);
            this.TROUGHList.Add(TROUGH2_LINE2);
            this.TROUGHList.Add(TROUGH3_LINE2);
            this.TROUGHList.Add(TROUGH4_LINE2);
            this.TROUGHList.Add(TROUGH5_LINE2);
            this.TROUGHList.Add(TROUGH6_LINE2);
            this.TROUGHList.Add(TROUGH7_LINE2);
            this.TROUGHList.Add(TROUGH8_LINE2);
            this.TROUGHList.Add(TROUGH9_LINE2);

            this.TROUGHList.Add(TROUGH1_PRINTER5_LINE3);
            this.TROUGHList.Add(TROUGH2_PRINTER5_LINE3);
            this.TROUGHList.Add(TROUGH3_PRINTER5_LINE3);
            this.TROUGHList.Add(TROUGH1_PRINTER6_LINE3);
            this.TROUGHList.Add(TROUGH2_PRINTER6_LINE3);
            this.TROUGHList.Add(TROUGH3_PRINTER6_LINE3);

            this.TROUGHList.Add(TROUGH4_PRINTER3_LINE3);
            this.TROUGHList.Add(TROUGH5_PRINTER3_LINE3);
            this.TROUGHList.Add(TROUGH6_PRINTER3_LINE3);
            this.TROUGHList.Add(TROUGH4_PRINTER4_LINE3);
            this.TROUGHList.Add(TROUGH5_PRINTER4_LINE3);
            this.TROUGHList.Add(TROUGH6_PRINTER4_LINE3);

            this.TROUGHList.Add(TROUGH7_PRINTER1_LINE3);
            this.TROUGHList.Add(TROUGH8_PRINTER1_LINE3);
            this.TROUGHList.Add(TROUGH9_PRINTER1_LINE3);
            this.TROUGHList.Add(TROUGH7_PRINTER2_LINE3);
            this.TROUGHList.Add(TROUGH8_PRINTER2_LINE3);
            this.TROUGHList.Add(TROUGH9_PRINTER2_LINE3);
        }
        void AddLEDFIXEDList()
        {
            this.LEDFIXEDList = new List<TROUGH>();

            this.LEDFIXEDList.Add(LED_FIXED_1);
            this.LEDFIXEDList.Add(LED_FIXED_2);
            this.LEDFIXEDList.Add(LED_FIXED_3);
            this.LEDFIXEDList.Add(LED_FIXED_4);
            this.LEDFIXEDList.Add(LED_FIXED_5);
            this.LEDFIXEDList.Add(LED_FIXED_6);
            this.LEDFIXEDList.Add(LED_FIXED_7);
            this.LEDFIXEDList.Add(LED_FIXED_8);
            this.LEDFIXEDList.Add(LED_FIXED_9);
            this.LEDFIXEDList.Add(LED_FIXED_10);
            this.LEDFIXEDList.Add(LED_FIXED_11);
            this.LEDFIXEDList.Add(LED_FIXED_12);
            this.LEDFIXEDList.Add(LED_FIXED_13);
            this.LEDFIXEDList.Add(LED_FIXED_14);

        }
        //void AddTroughsName()
        //{
        //    this._troughsName = new List<string>();

        //    this.TROUGHSName.Add(TROUGH1_LINE1.TroughName);
        //    this.TROUGHSName.Add(TROUGH2_LINE1.TroughName);
        //    this.TROUGHSName.Add(TROUGH3_LINE1.TroughName);

        //    this.TROUGHSName.Add(TROUGH1_LINE2.TroughName);
        //    this.TROUGHSName.Add(TROUGH2_LINE2.TroughName);
        //    this.TROUGHSName.Add(TROUGH3_LINE2.TroughName);
        //    this.TROUGHSName.Add(TROUGH4_LINE2.TroughName);
        //    this.TROUGHSName.Add(TROUGH5_LINE2.TroughName);
        //    this.TROUGHSName.Add(TROUGH6_LINE2.TroughName);
        //    this.TROUGHSName.Add(TROUGH7_LINE2.TroughName);
        //    this.TROUGHSName.Add(TROUGH8_LINE2.TroughName);
        //    this.TROUGHSName.Add(TROUGH9_LINE2.TroughName);

        //    this.TROUGHSName.Add(TROUGH1_LINE3.TroughName);
        //    this.TROUGHSName.Add(TROUGH2_LINE3.TroughName);
        //    this.TROUGHSName.Add(TROUGH3_LINE3.TroughName);
        //    this.TROUGHSName.Add(TROUGH4_LINE3.TroughName);
        //    this.TROUGHSName.Add(TROUGH5_LINE3.TroughName);
        //    this.TROUGHSName.Add(TROUGH6_LINE3.TroughName);
        //    this.TROUGHSName.Add(TROUGH7_LINE3.TroughName);
        //    this.TROUGHSName.Add(TROUGH8_LINE3.TroughName);
        //    this.TROUGHSName.Add(TROUGH9_LINE3.TroughName);
        //}
        void AddSendParam()
        {
            foreach (var trough in TROUGHList)
            {
                trough.SendParam = Marshal.StringToHGlobalUni(trough.IPAddress);
            }

            foreach (var item in LEDFIXEDList)
            {
                item.SendParam = Marshal.StringToHGlobalUni(item.IPAddress);
            }
        }
        void AddTroughInPrinter()
        {
            this.PrinterViewModel.PRINTER1_LINE1.Troughs.Add(TROUGH1_PRINTER1_LINE1);
            this.PrinterViewModel.PRINTER1_LINE1.Troughs.Add(TROUGH2_PRINTER1_LINE1);
            this.PrinterViewModel.PRINTER1_LINE1.Troughs.Add(TROUGH3_PRINTER1_LINE1);
            this.PrinterViewModel.PRINTER2_LINE1.Troughs.Add(TROUGH1_PRINTER1_LINE1);
            this.PrinterViewModel.PRINTER2_LINE1.Troughs.Add(TROUGH2_PRINTER1_LINE1);
            this.PrinterViewModel.PRINTER2_LINE1.Troughs.Add(TROUGH3_PRINTER1_LINE1);

            this.PrinterViewModel.PRINTER1_LINE2.Troughs.Add(TROUGH9_LINE2);
            this.PrinterViewModel.PRINTER2_LINE2.Troughs.Add(TROUGH8_LINE2);
            this.PrinterViewModel.PRINTER3_LINE2.Troughs.Add(TROUGH7_LINE2);
            this.PrinterViewModel.PRINTER4_LINE2.Troughs.Add(TROUGH5_LINE2);
            this.PrinterViewModel.PRINTER4_LINE2.Troughs.Add(TROUGH6_LINE2);
            this.PrinterViewModel.PRINTER5_LINE2.Troughs.Add(TROUGH3_LINE2);
            this.PrinterViewModel.PRINTER5_LINE2.Troughs.Add(TROUGH4_LINE2);
            this.PrinterViewModel.PRINTER6_LINE2.Troughs.Add(TROUGH1_LINE2);
            this.PrinterViewModel.PRINTER6_LINE2.Troughs.Add(TROUGH2_LINE2);

            this.PrinterViewModel.PRINTER1_LINE3.Troughs.Add(TROUGH7_PRINTER1_LINE3);
            this.PrinterViewModel.PRINTER1_LINE3.Troughs.Add(TROUGH8_PRINTER1_LINE3);
            this.PrinterViewModel.PRINTER1_LINE3.Troughs.Add(TROUGH9_PRINTER1_LINE3);
            this.PrinterViewModel.PRINTER2_LINE3.Troughs.Add(TROUGH7_PRINTER2_LINE3);
            this.PrinterViewModel.PRINTER2_LINE3.Troughs.Add(TROUGH8_PRINTER2_LINE3);
            this.PrinterViewModel.PRINTER2_LINE3.Troughs.Add(TROUGH9_PRINTER2_LINE3);

            this.PrinterViewModel.PRINTER3_LINE3.Troughs.Add(TROUGH4_PRINTER3_LINE3);
            this.PrinterViewModel.PRINTER3_LINE3.Troughs.Add(TROUGH5_PRINTER3_LINE3);
            this.PrinterViewModel.PRINTER3_LINE3.Troughs.Add(TROUGH6_PRINTER3_LINE3);
            this.PrinterViewModel.PRINTER4_LINE3.Troughs.Add(TROUGH4_PRINTER4_LINE3);
            this.PrinterViewModel.PRINTER4_LINE3.Troughs.Add(TROUGH5_PRINTER4_LINE3);
            this.PrinterViewModel.PRINTER4_LINE3.Troughs.Add(TROUGH6_PRINTER4_LINE3);

            this.PrinterViewModel.PRINTER5_LINE3.Troughs.Add(TROUGH1_PRINTER5_LINE3);
            this.PrinterViewModel.PRINTER5_LINE3.Troughs.Add(TROUGH2_PRINTER5_LINE3);
            this.PrinterViewModel.PRINTER5_LINE3.Troughs.Add(TROUGH3_PRINTER5_LINE3);
            this.PrinterViewModel.PRINTER6_LINE3.Troughs.Add(TROUGH1_PRINTER6_LINE3);
            this.PrinterViewModel.PRINTER6_LINE3.Troughs.Add(TROUGH2_PRINTER6_LINE3);
            this.PrinterViewModel.PRINTER6_LINE3.Troughs.Add(TROUGH3_PRINTER6_LINE3);

            this.PrinterViewModel.PRINTER1_LINE1.LEDTableFixed = this.LED_FIXED_1;
            this.PrinterViewModel.PRINTER2_LINE1.LEDTableFixed = this.LED_FIXED_2;

            this.PrinterViewModel.PRINTER1_LINE2.LEDTableFixed = this.LED_FIXED_3;
            this.PrinterViewModel.PRINTER2_LINE2.LEDTableFixed = this.LED_FIXED_4;
            this.PrinterViewModel.PRINTER3_LINE2.LEDTableFixed = this.LED_FIXED_5;
            this.PrinterViewModel.PRINTER4_LINE2.LEDTableFixed = this.LED_FIXED_6;
            this.PrinterViewModel.PRINTER5_LINE2.LEDTableFixed = this.LED_FIXED_7;
            this.PrinterViewModel.PRINTER6_LINE2.LEDTableFixed = this.LED_FIXED_8;

            this.PrinterViewModel.PRINTER1_LINE3.LEDTableFixed = this.LED_FIXED_9;
            this.PrinterViewModel.PRINTER2_LINE3.LEDTableFixed = this.LED_FIXED_10;
            this.PrinterViewModel.PRINTER3_LINE3.LEDTableFixed = this.LED_FIXED_11;
            this.PrinterViewModel.PRINTER4_LINE3.LEDTableFixed = this.LED_FIXED_12;
            this.PrinterViewModel.PRINTER5_LINE3.LEDTableFixed = this.LED_FIXED_13;
            this.PrinterViewModel.PRINTER6_LINE3.LEDTableFixed = this.LED_FIXED_14;

        }
        private void SetTextInitalForLEDTable(TROUGH trough, string text)
        {
            Task.Run(() =>
            {
                IntPtr pNULL = new IntPtr(0);

                int nErrorCode = -1;
                // 1. Create a screen
                int nWidth = 160;
                int nHeight = 48;
                int nColor = 1;
                int nGray = 1;
                int nCardType = 0;

                int nRe = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                // 2. Add program to screen
                int nProgramID = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0);
                if (nProgramID == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                int nX = 0;
                int nY = 0;
                int nAreaWidth = 160;
                int nAreaHeight = 16;

                // 3. Add Area to program
                int nAreaID = CSDKExport.Hd_AddArea(nProgramID, nX, nY, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                IntPtr pText = Marshal.StringToHGlobalUni(text);
                IntPtr pFontName = Marshal.StringToHGlobalUni("Arial");
                int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);
                int nAreaItemID = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID, pText, nTextColor, 0, 4, pFontName, 16, 1, 30, 201, 0, pNULL, 0);

                if (nAreaItemID == -1)
                {
                    Marshal.FreeHGlobal(pText);
                    Marshal.FreeHGlobal(pFontName);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText);
                Marshal.FreeHGlobal(pFontName);

                // 5. Send to device 
                nRe = CSDKExport.Hd_SendScreen(0, trough.SendParam, pNULL, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{trough.TroughName} đã sẵn sàng hoạt động", Define.SolidColorOK);
            });
            //await Task.Factory.StartNew(() =>
            //{

            //}); 
        }
        private async Task SetTextToTrough(TROUGH trough, string text1, string text2, string text3)
        {
            await Task.Run(() =>
            {
                IntPtr pNULL = new IntPtr(0);

                // 1. Create a screen
                int nWidth = 96;
                int nHeight = 48;
                int nColor = 1;
                int nGray = 1;
                int nCardType = 0;
                int nErrorCode = -1;

                int nRe = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                // 2. Add program to screen
                int nProgramID = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0);
                if (nProgramID == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                int nX = 0;
                int nY = 0;
                int nAreaWidth = 96;
                int nAreaHeight = 16;

                // 3. Add Area to program

                int nAreaID1 = CSDKExport.Hd_AddArea(nProgramID, nX, nY + 16, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID1 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                int nAreaID2 = CSDKExport.Hd_AddArea(nProgramID, nX, nY + 32, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID2 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                int nAreaID3 = CSDKExport.Hd_AddArea(nProgramID, nX + 25, nY, 50, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID3 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                // 4.Add text AreaItem to Area

                IntPtr nFontName = Marshal.StringToHGlobalUni("Arial");
                int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);

                IntPtr pText1 = Marshal.StringToHGlobalUni(text1);
                int nAreaItemID1 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID1, pText1, nTextColor, 0, 4, nFontName, 16, 202, 40, 201, 0, pNULL, 0);

                IntPtr pText2 = Marshal.StringToHGlobalUni(text2);
                int nAreaItemID2 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID2, pText2, nTextColor, 0, 4, nFontName, 16, 202, 40, 201, 0, pNULL, 0);

                IntPtr pText3 = Marshal.StringToHGlobalUni(text3);
                int nAreaItemID3 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID3, pText3, nTextColor, 0, 4, nFontName, 16, 0, 40, 201, 0, pNULL, 0);

                if (nAreaItemID1 == -1)
                {
                    Marshal.FreeHGlobal(pText1);
                    Marshal.FreeHGlobal(nFontName);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText1);
                Marshal.FreeHGlobal(nFontName);
                if (nAreaItemID2 == -1)
                {
                    Marshal.FreeHGlobal(pText2);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText2);
                if (nAreaItemID3 == -1)
                {
                    Marshal.FreeHGlobal(pText3);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText3);

                // 5. Send to device 

                nRe = CSDKExport.Hd_SendScreen(0, trough.SendParam, pNULL, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                }
                trough.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE;

            });
            //await Task.Factory.StartNew(() =>
            //{

            //});
        }
        private async Task SetTextToLEDFix(TROUGH ledFix, string text1, string text2, string text3)
        {
            await Task.Run(() =>
            {
                IntPtr pNULL = new IntPtr(0);

                // 1. Create a screen
                int nWidth = 96;
                int nHeight = 48;
                int nColor = 1;
                int nGray = 1;
                int nCardType = 0;
                int nErrorCode = -1;

                int nRe = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                // 2. Add program to screen
                int nProgramID = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0);
                if (nProgramID == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                int nX = 0;
                int nY = 0;
                int nAreaWidth = 96;
                int nAreaHeight = 16;

                // 3. Add Area to program

                int nAreaID1 = CSDKExport.Hd_AddArea(nProgramID, nX, nY + 16, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID1 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                int nAreaID2 = CSDKExport.Hd_AddArea(nProgramID, nX, nY + 32, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID2 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                int nAreaID3 = CSDKExport.Hd_AddArea(nProgramID, nX + 25, nY, 50, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID3 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                // 4.Add text AreaItem to Area

                IntPtr nFontName = Marshal.StringToHGlobalUni("Arial");
                int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);

                IntPtr pText1 = Marshal.StringToHGlobalUni(text1);
                int nAreaItemID1 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID1, pText1, nTextColor, 0, 4, nFontName, 16, 202, 40, 201, 0, pNULL, 0);

                IntPtr pText2 = Marshal.StringToHGlobalUni(text2);
                int nAreaItemID2 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID2, pText2, nTextColor, 0, 4, nFontName, 16, 202, 40, 201, 0, pNULL, 0);

                IntPtr pText3 = Marshal.StringToHGlobalUni(text3);
                int nAreaItemID3 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID3, pText3, nTextColor, 0, 4, nFontName, 16, 0, 40, 201, 0, pNULL, 0);

                if (nAreaItemID1 == -1)
                {
                    Marshal.FreeHGlobal(pText1);
                    Marshal.FreeHGlobal(nFontName);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText1);
                Marshal.FreeHGlobal(nFontName);
                if (nAreaItemID2 == -1)
                {
                    Marshal.FreeHGlobal(pText2);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText2);
                if (nAreaItemID3 == -1)
                {
                    Marshal.FreeHGlobal(pText3);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText3);

                // 5. Send to device 

                nRe = CSDKExport.Hd_SendScreen(0, ledFix.SendParam, pNULL, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                }
                ledFix.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE;

            });
        }

        public async Task SetTextToAllLEDPanel(PRINTER printer, TROUGH ledFix, TROUGH trough, bool isDone, string text1, string text2, string text3)
        {
            await Task.Run(() =>
            {
                IntPtr pNULL = new IntPtr(0);

                // 1. Create a screen
                int nWidth = 96;
                int nHeight = 48;
                int nColor = 1;
                int nGray = 1;
                int nCardType = 0;
                int nErrorCode = -1;

                int nRe = CSDKExport.Hd_CreateScreen(nWidth, nHeight, nColor, nGray, nCardType, pNULL, 0);
                if (nRe != 0)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    if (ledFix != null)
                        ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough != null)
                        trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                // 2. Add program to screen
                int nProgramID = CSDKExport.Hd_AddProgram(pNULL, 0, 0, pNULL, 0);
                if (nProgramID == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    if (ledFix != null)
                        ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough != null)
                        trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                int nX = 0;
                int nY = 0;
                int nAreaWidth = 96;
                int nAreaHeight = 16;

                // 3. Add Area to program

                int nAreaID1 = CSDKExport.Hd_AddArea(nProgramID, nX, nY + 16, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID1 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    if (ledFix != null)
                        ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough != null)
                        trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                int nAreaID2 = CSDKExport.Hd_AddArea(nProgramID, nX, nY + 32, nAreaWidth, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID2 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    if (ledFix != null)
                        ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough != null)
                        trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                int nAreaID3 = CSDKExport.Hd_AddArea(nProgramID, nX + 25, nY, 50, nAreaHeight, pNULL, 0, 0, pNULL, 0);
                if (nAreaID3 == -1)
                {
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    if (ledFix != null)
                        ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough != null)
                        trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }

                // 4.Add text AreaItem to Area

                IntPtr nFontName = Marshal.StringToHGlobalUni("Arial");
                int nTextColor = CSDKExport.Hd_GetColor(255, 0, 0);

                IntPtr pText1 = Marshal.StringToHGlobalUni(text1);
                int nAreaItemID1 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID1, pText1, nTextColor, 0, 4, nFontName, 16, 202, 40, 201, 0, pNULL, 0);

                IntPtr pText2 = Marshal.StringToHGlobalUni(text2);
                int nAreaItemID2 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID2, pText2, nTextColor, 0, 4, nFontName, 16, 202, 40, 201, 0, pNULL, 0);

                //IntPtr pText3 = Marshal.StringToHGlobalUni(text3);
                //int nAreaItemID3 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID3, pText3, nTextColor, 0, 4, nFontName, 16, 0, 40, 201, 0, pNULL, 0);
                int nAreaItemID3;
                if (!isDone)
                {
                    int count_number = Convert.ToInt32(text3.Trim());
                    nAreaItemID3 = CSDKExport.Hd_AddDigitAreaItem(nAreaID3, 1, 2, 0, 0, count_number, 0, 0, CSDKExport.Hd_GetColor(255, 0, 0), 1, 0, 0, pNULL, 0, pNULL, 0, 0, 0, nFontName, 16, pNULL, 0);
                }
                else
                {
                    IntPtr pText3 = Marshal.StringToHGlobalUni(text3);
                    nAreaItemID3 = CSDKExport.Hd_AddSimpleTextAreaItem(nAreaID3, pText3, nTextColor, 0, 4, nFontName, 16, 0, 40, 201, 0, pNULL, 0);
                }

                if (nAreaItemID1 == -1)
                {
                    Marshal.FreeHGlobal(pText1);
                    Marshal.FreeHGlobal(nFontName);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    if (ledFix != null)
                        ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough != null)
                        trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText1);
                Marshal.FreeHGlobal(nFontName);
                if (nAreaItemID2 == -1)
                {
                    Marshal.FreeHGlobal(pText2);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    if (ledFix != null)
                        ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough != null)
                        trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                Marshal.FreeHGlobal(pText2);
                if (nAreaItemID3 == -1)
                {
                    //Marshal.FreeHGlobal(pText3);
                    nErrorCode = CSDKExport.Hd_GetSDKLastError();
                    if (ledFix != null)
                        ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough != null)
                        trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    return;
                }
                //Marshal.FreeHGlobal(pText3);

                // 5. Send to device 
                if (printer.IsUseTrough)
                {
                    nRe = CSDKExport.Hd_SendScreen(0, ledFix.SendParam, pNULL, pNULL, 0);
                    if (nRe != 0)
                    {
                        nErrorCode = CSDKExport.Hd_GetSDKLastError();
                        if (ledFix != null)
                            ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    }
                    if (trough != null)
                    {
                        nRe = CSDKExport.Hd_SendScreen(0, trough.SendParam, pNULL, pNULL, 0);
                        if (nRe != 0)
                        {
                            nErrorCode = CSDKExport.Hd_GetSDKLastError();
                            if (trough != null)
                                trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                        }
                    }
                }
                else
                {
                    nRe = CSDKExport.Hd_SendScreen(0, ledFix.SendParam, pNULL, pNULL, 0);
                    if (nRe != 0)
                    {
                        nErrorCode = CSDKExport.Hd_GetSDKLastError();
                        if (ledFix != null)
                            ledFix.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    }
                }

                ledFix.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE;
                if (trough != null)
                {
                    trough.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE;
                    if (trough.TROUGH_STATES == TROUGH_STATES.IS_ACTIVE)
                    {
                        TROUGH tr = this.TROUGHList.Where(x => x.Tag == trough.Tag && x.PrinterLead != trough.PrinterLead).FirstOrDefault();
                        if (tr != null)
                            tr.TROUGH_STATES = TROUGH_STATES.IS_ACTIVE;
                    }
                }
            });
        }

        private async Task CheckCardStatus(TROUGH trough, int delay)
        {
            if (trough == null)
                return;
            trough.TROUGH_STATES = TROUGH_STATES.CHECKING;
            await Task.Delay(delay);
            await Task.Run(() =>
            {
                IntPtr pNULL = new IntPtr(0);
                int nRe = CSDKExport.Cmd_IsCardOnline(0, trough.SendParam, pNULL);
                if (nRe == 0)
                {
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                    TROUGH tr = this.TROUGHList.Where(x => x.Tag == trough.Tag && x.PrinterLead != trough.PrinterLead).FirstOrDefault();
                    if (tr != null)
                        tr.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{trough.TroughName} có thể hoạt động", Define.SolidColorOK);
                }
                else
                {
                    trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                    if (trough.IsLEDFix)
                        MessageBox.Show($"{trough.TroughName} không thể hoạt động! Hãy CHÚ Ý!");
                    MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{trough.TroughName} không thể hoạt động", Define.SolidColorFail);
                }
            });
        }
        private void CheckCardStatus(TROUGH trough)
        {
            trough.TROUGH_STATES = TROUGH_STATES.CHECKING;
            Thread.Sleep(200);

            IntPtr pNULL = new IntPtr(0);
            int nRe = CSDKExport.Cmd_IsCardOnline(0, trough.SendParam, pNULL);
            if (nRe == 0)
            {
                trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_READY;
                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{trough.TroughName} có thể hoạt động", Define.SolidColorOK);
            }
            else
            {
                trough.TROUGH_STATES = TROUGH_STATES.ACTIVE_NOREADY;
                MainViewModel.Instance.WirteLogSystem(MainViewModel.Instance.MainView.paraLog, $"{trough.TroughName} không thể hoạt động", Define.SolidColorFail);
            }

        }

        public async Task CheckAllCardStatusForLEDTableFixed()
        {
            foreach (var led in LEDFIXEDList)
            {
                await CheckCardStatus(led, 0);
            }
        }
        public async Task CheckSingleStatusLEDTableFixed(TROUGH trough, int delay)
        {
            await CheckCardStatus(trough, 0);
        }
        public void CheckSingleStatusLEDTableFixed(TROUGH trough)
        {
            this.CheckCardStatus(trough);
        }
        #endregion
    }
}
