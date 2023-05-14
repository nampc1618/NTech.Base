using ControlzEx.Standard;
using NTech.Xm.Database.Models;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace NTech.Xm.Station.Commons.Defines
{
    public enum PRINTER_STATES
    {
        [Description("Đã kết nối")]
        CONNECTED,
        [Description("")]
        DISCONNECTED,
        //[Description("Chưa bật tia")]
        //NOT_YET_START_JET,
        //[Description("Đã bật tia")]
        //STARTED_JET,
        [Description("Có thể in")]
        IS_CAN_PRINT,
        [Description("Sẵn sàng in")]
        PRINT_READY,
        [Description("Đang in")]
        PRINTING,
        [Description("Dừng in")]
        PRINT_STOP,
        [Description("Đã in xong")]
        PRINT_DONE
    }
    public enum TROUGH_STATES
    {
        NO_DEFINE,
        [Description("Đang kiểm tra...")]
        CHECKING,
        [Description("Có thể hoạt động")]
        ACTIVE_READY,
        [Description("Không thể hoạt động")]
        ACTIVE_NOREADY,
        [Description("Đang hoạt động")]
        IS_ACTIVE
    }
    public enum CommandsIDPrinter
    {
        RequestMessagePrintCount = 0x8D,
        SetMessagePrintCount = 0x8C,
        RequestTotalPrintCount = 0x08,
        StartJetID = 0x0F,
        StartPrintID = 0x11,
        StopPrintID = 0x12,
        RequestPrinterStatus = 0x14,
        RequestMessage = 0x1F,
        DownloadRemoteFieldData = 0x1D,
        DownloadMessageData = 0x19,
        DeleteMessageData = 0x1B,
        LoadPrintMessage = 0x1E

    }
    public enum STEPS_PERFORM
    {
        CONNECT_PRINTER,
        DISCONNECT_PRINTER,
        REQUEST_PRINTER_STATUS,
        REQUEST_MESSAGE,
        DOWNLOAD_REMOTE_FIELD_DATA,
        DOWNLOAD_MESSAGE_DATA,
        DELETE_MESSAGE_DATA,
        LOAD_PRINT_MESSAGE,
        SET_MESSAGE_PRINT_COUNT,
        START_JET,
        START_PRINT,
        REQUEST_MESSAGE_PRINT_COUNT,
        STOP_PRINT
    }
    public enum MESSAGE_STATE
    {
        [Description("")]
        NO_DEFINE,
        [Description("Chưa in")]
        NOT_YET_PRINT,
        [Description("Chưa hoàn thành")]
        NOT_PRINT_DONE,
        [Description("Đang in")]
        PRINTING,
        [Description("Đã hoàn thành")]
        PRINT_DONE
    }
    /// <summary>
    /// Các trường hợp khi phần mềm chạy
    /// </summary>
    public enum CASE_PROG_RUN
    {
        MANUAL,
        REALTIME,
        OTHER
    }

    public static class Define
    {
        public static readonly string IPPrinterPath = Path.Combine(Environment.CurrentDirectory, "IPPrinterList.xml");
        public static readonly string IPTroughPath = Path.Combine(Environment.CurrentDirectory, "IPTroughList.xml");
        public static readonly string MsgParamPath = Path.Combine(Environment.CurrentDirectory, "MsgParam.xml");
        public static readonly string PlcPath = Path.Combine(Environment.CurrentDirectory, "PLC.xml");

        public static readonly SolidColorBrush SolidColorOK = (SolidColorBrush)new BrushConverter().ConvertFromString("#2E7D32");
        public static readonly SolidColorBrush SolidColorFail = (SolidColorBrush)new BrushConverter().ConvertFromString("#DD2C00");

        public static string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            if (fieldInfo != null)
            {
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                if (attribArray != null && attribArray.Length > 0 && attribArray[0] is DescriptionAttribute attrib)
                {
                    return attrib.Description;
                }
            }
            return enumObj.ToString();
        }
    }
    public static class CommandPrinter
    {
        public static byte[] CmdStartJet = new byte[]
        {
             0x1B,
             0x2,
             0x0F,
             0x1B,
             0x3
        };
        public static byte[] CmdStartPrint = new byte[]
        {
             0x1B,
             0x2,
             0x11,
             0x1B,
             0x3
        };
        public static byte[] CmdStopPrint = new byte[]
        {
             0x1B,
             0x2,
             0x12,
             0x1B,
             0x3
        };
        public static byte[] CmdRequestPrinterStatus = new byte[]
        {
             0x1B,
             0x2,
             0x14,
             0x1B,
             0x3
        };
        public static byte[] CmdRequestTotalPrintCount = new byte[]
        {
             0x1B,
             0x2,
             0x8,
             0x1B,
             0x3
        };
        public static byte[] CmdSetTotalPrintCount = new byte[]
        {
             0x1B,
             0x2,
             0x7,
             0x0,
             0x0,
             0x0,
             0x0,
             0x1B,
             0x3
        };
        public static byte[] CmdRequestMessage = new byte[]
        {
             0x1B,
             0x2,
             0x1F,
             0x1B,
             0x3
        };
        public static byte[] CmdRequestMessagePrintCount = new byte[]
        {
             0x1B,
             0x2,
             0x8D,
             0x0, //4
             0x0, //5
             0x0, //6
             0x0, //7
             0x0, //8
             0x0, //9
             0x0, //10
             0x0, //11
             0x0, //12
             0x0, //13
             0x0, //14
             0x0, //15
             0x0, //16
             0x0, //17
             0x0, //18
             0x0, //19
             0x1B,
             0x3
        };
        public static byte[] CmdSetMessagePrintCount = new byte[]
        {
             0x1B,
             0x2,
             0x8C,
             0x0, //4
             0x0, //5
             0x0, //6
             0x0, //7
             0x0, //8
             0x0, //9
             0x0, //10
             0x0, //11
             0x0, //12
             0x0, //13
             0x0, //14
             0x0, //15
             0x0, //16
             0x0, //17
             0x0, //18
             0x0, //19
            0x0, //20
            0x0, //21
             0x0, //22
             0x0, //23
             0x1B,
             0x3
        };
        public static byte[] CmdDownloadMessageData = new byte[]
        {
            0x1B, 0x02, //ESC STX sequence
            0x19, //Command ID - Download Message Data
            0x01, //Number of Messages - 1
            0x71, 0x00, //Length in Bytes - 113
            0x1E, 0x01, //Length in Rasters - 286
            0x06, //EHT setting - 6
            0x37, 0x00, //Width - 55
            0x00, 0x00, //Print delay - 0
            0x54, 0x48, 0x41, 0x4E, 0x48, 0x54, 0x48, 0x41,  //Message name - THANHTHANGMSG
            0x4E, 0x47, 0x4D, 0x53, 0x4E, 0x00, 0x00, 0x00,
            0x32, 0x35, 0x20, 0x51, 0x55, 0x41, 0x4C, 0x49, //Raster name - 25 QUALITY
            0x54, 0x59, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x1C, //Field header
            0x00, //Field type - Text field
            0x48, 0x00, //Field Length - 72
            0x00, //Y Position - 0
            0x00, 0x00, //X Position - 0
            0x70, 0x00, //Length in Rasters - 112
            0x0F, //Height in drops - 15
            0x00, //Format 3 - 00
            0x01, //Bold multiplier - 1
            0x0A, //String length(excl. null) - 10
            0x00, //Format 1 - 0
            0x00, //Format 2 - 0
            0x00, //Linkage - 0
            0x31, 0x35, 0x20, 0x46, 0x48, 0x20, 0x43, 0x41, //Data set name - 15 FH CAPS
            0x50, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, //Data - 1234567890
            0x39, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00,
            0x1B, 0x03
        };
        public static byte[] DownloadRemoteFieldData(string data)
        {
            char[] chs = data.ToCharArray();
            byte[] byteArr2 = new byte[50];
            byte[] chArr = Encoding.ASCII.GetBytes(chs);
            Array.Copy(chArr, byteArr2, chArr.Length);

            byte[] byteArr1 = new byte[] { 0x1B, 0x2, 0x1D, 0x32, 0x0 };
            byte[] byteArr3 = new byte[] { 0x1B, 0x3 };

            byte[] byteArr = byteArr1.Concat(byteArr2).Concat(byteArr3).ToArray();

            return byteArr;
        }
        public static byte[] LoadPrintMessage(string messageName)
        {
            char[] ch = messageName.ToCharArray();
            byte[] bDes = new byte[16];
            byte[] bSource = Encoding.ASCII.GetBytes(ch);
            Array.Copy(bSource, bDes, bSource.Length);

            byte[] bArr1 = new byte[] { 0x1B, 0x2, (byte)CommandsIDPrinter.LoadPrintMessage };
            byte[] bArr2 = new byte[] { 0x0, 0x0, 0x1B, 0x3 };

            byte[] bEnd = bArr1.Concat(bDes).Concat(bArr2).ToArray();

            return bEnd;
        }
        public static byte[] DeleteMessageData(string messageName)
        {
            char[] ch = messageName.ToCharArray();
            byte[] bDes = new byte[16];
            byte[] bSource = Encoding.ASCII.GetBytes(ch);
            Array.Copy(bSource, bDes, bSource.Length);

            byte[] bArr1 = new byte[] { 0x1B, 0x2, (byte)CommandsIDPrinter.DeleteMessageData, (byte)CommandsIDPrinter.DeleteMessageData, 0x01 };
            byte[] bArr2 = new byte[] { 0x1B, 0x3 };

            byte[] bEnd = bArr1.Concat(bDes).Concat(bArr2).ToArray();

            return bEnd;
        }
        public static byte[] DownloadMessageData(string messageName, string data)
        {


            //Create Field Header
            char[] ch2 = data.ToCharArray();
            byte[] bSourceData = Encoding.ASCII.GetBytes(ch2);
            byte[] bDesData = new byte[bSourceData.Length + 1];
            Array.Copy(bSourceData, bDesData, bSourceData.Length);

            if (bSourceData.Length == 27)
            {
                byte[] bFieldHeader1 = new byte[]
                {
                0x1C, //Field header
                0x00, //Field type - Text field
                (byte)(bDesData.Length + 32), 0x00, //Field Length - 
                0x00, //Y Position - 0
                0x00, 0x00, //X Position - 0
                PrinterViewModel.Instance.PRINTERSelected.LenghtMsgInRaster, 0x00, //Length in Rasters - 112
                0x0F, //Height in drops - 15
                PrinterViewModel.Instance.PRINTERSelected.FieldOrientation, //Format 3 - Bình thường, Lật ngang, Lật dọc, Lật ngang + dọc
                0x01, //Bold multiplier - 1
                (byte)(bSourceData.Length), 0x1B, //String length(excl. null) - do dai chuoi truyen vao
                0x00, //Format 1 - 0
                0x00, //Format 2 - 0
                0x00, //Linkage - 0
                0x31, 0x35, 0x20, 0x46, 0x48, 0x20, 0x43, 0x41, //Data set name - 15 FH CAPS
                0x50, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                };
                byte[] bFieldHeader2 = new byte[] { 0x1B, 0x3 };

                byte[] bFieldHeader = bFieldHeader1.Concat(bDesData).Concat(bFieldHeader2).ToArray();

                //Create Message Header

                char[] ch1 = messageName.ToCharArray();
                byte[] bDesMsgName = new byte[16];
                byte[] bSourceMsgName = Encoding.ASCII.GetBytes(ch1);
                Array.Copy(bSourceMsgName, bDesMsgName, bSourceMsgName.Length);

                byte[] bMessageHeader1 = new byte[]
                {
                0x1B, 0x02, //ESC STX sequence
                0x19, //Command ID - Download Message Data
                0x01, //Number of Messages - 1
                (byte)((bFieldHeader.Length - 3) + 41), 0x00, //Length in Bytes - 
                PrinterViewModel.Instance.PRINTERSelected.LenghtMsgInRaster, 0x00, //Length in Rasters - 112
                PrinterViewModel.Instance.PRINTERSelected.HeightMsg, //EHT setting - 6
                PrinterViewModel.Instance.PRINTERSelected.WidthMsg, 0x00, //Width - 3
                PrinterViewModel.Instance.PRINTERSelected.Delay, 0x00, //Print delay - 0
                };
                byte[] bMessageHeader2 = new byte[]
                {
                0x31, 0x36, 0x20, 0x51, 0x55, 0x41, 0x4C, 0x49, //Message Type - 16 QUALITY
                0x54, 0x59, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                };
                byte[] bMessageHeader = bMessageHeader1.Concat(bDesMsgName).Concat(bMessageHeader2).ToArray();


                byte[] bMSG = bMessageHeader.Concat(bFieldHeader).ToArray();
                return bMSG;
            }
            else
            {
                byte[] bFieldHeader1 = new byte[]
               {
                0x1C, //Field header
                0x00, //Field type - Text field
                (byte)(bDesData.Length + 32), 0x00, //Field Length - 
                0x00, //Y Position - 0
                0x00, 0x00, //X Position - 0
                PrinterViewModel.Instance.PRINTERSelected.LenghtMsgInRaster, 0x00, //Length in Rasters - 112
                0x0F, //Height in drops - 15
                PrinterViewModel.Instance.PRINTERSelected.FieldOrientation, //Format 3 - Bình thường, Lật ngang, Lật dọc, Lật ngang + dọc
                0x01, //Bold multiplier - 1
                (byte)(bSourceData.Length), //String length(excl. null) - do dai chuoi truyen vao
                0x00, //Format 1 - 0
                0x00, //Format 2 - 0
                0x00, //Linkage - 0
                0x31, 0x35, 0x20, 0x46, 0x48, 0x20, 0x43, 0x41, //Data set name - 15 FH CAPS
                0x50, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
               };
                byte[] bFieldHeader2 = new byte[] { 0x1B, 0x3 };

                byte[] bFieldHeader = bFieldHeader1.Concat(bDesData).Concat(bFieldHeader2).ToArray();

                //Create Message Header

                char[] ch1 = messageName.ToCharArray();
                byte[] bDesMsgName = new byte[16];
                byte[] bSourceMsgName = Encoding.ASCII.GetBytes(ch1);
                Array.Copy(bSourceMsgName, bDesMsgName, bSourceMsgName.Length);

                byte[] bMessageHeader1 = new byte[]
                {
                0x1B, 0x02, //ESC STX sequence
                0x19, //Command ID - Download Message Data
                0x01, //Number of Messages - 1
                (byte)((bFieldHeader.Length - 2) + 41), 0x00, //Length in Bytes - 
                PrinterViewModel.Instance.PRINTERSelected.LenghtMsgInRaster, 0x00, //Length in Rasters - 112
                PrinterViewModel.Instance.PRINTERSelected.HeightMsg, //EHT setting - 6
                PrinterViewModel.Instance.PRINTERSelected.WidthMsg, 0x00, //Width - 3
                PrinterViewModel.Instance.PRINTERSelected.Delay, 0x00, //Print delay - 0
                };
                byte[] bMessageHeader2 = new byte[]
                {
                0x31, 0x36, 0x20, 0x51, 0x55, 0x41, 0x4C, 0x49, //Message Type - 16 QUALITY
                0x54, 0x59, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                };
                byte[] bMessageHeader = bMessageHeader1.Concat(bDesMsgName).Concat(bMessageHeader2).ToArray();


                byte[] bMSG = bMessageHeader.Concat(bFieldHeader).ToArray();
                return bMSG;
            }

        }
    }
}
