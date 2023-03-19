using MahApps.Metro.Controls;
using Microsoft.SqlServer.Server;
using NTech.Xm.Station.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ControlzEx.Standard;
using System.Windows.Documents;
using System.Collections;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Shapes;

namespace NTech.Xm.Station.Commons
{
    public static class PrinterFaultList
    {
        public static string ToDescription(char ch)
        {
            switch (ch)
            {
                case (char)0x01: return "--> Print Head Temperature" + Environment.NewLine;
                case (char)0x02: return "--> EHT Trip" + Environment.NewLine;
                case (char)0x03: return "--> Phase Failure" + Environment.NewLine;
                case (char)0x04: return "--> Time of Flight" + Environment.NewLine;
                case (char)0x05: return "--> 300V Power Supply" + Environment.NewLine;
                case (char)0x06: return "--> Hardware Safety Trip" + Environment.NewLine;
                case (char)0x07: return "--> Ink Tank Empty" + Environment.NewLine;
                case (char)0x08: return "--> Internal Spillage" + Environment.NewLine;
                case (char)0x0A: return "--> Solvent Tank Empty" + Environment.NewLine;
                case (char)0x0B: return "--> Jet Misaligned" + Environment.NewLine;
                case (char)0x0C: return "--> Pressure Limit" + Environment.NewLine;
                case (char)0x0D: return "--> Viscosity" + Environment.NewLine;
                default:
                    return "";
            }
        }
    }
    public static class CommandFaultCodeList
    {
        public static string ToDescription(char ch)
        {
            switch (ch)
            {
                case (char)0x01: return "--> Parity error\n";
                case (char)0x02: return "--> Framing error\n";
                case (char)0x03: return "--> Data overrun\n";
                case (char)0x04: return "--> Serial break\n";
                case (char)0x05: return "--> Receive buffer overflow\n";
                case (char)0x06: return "--> Command start\n";
                case (char)0x07: return "--> Command end\n";
                case (char)0x08: return "--> Invalid checksum\n";
                case (char)0x11: return "--> Invalid command\n";
                case (char)0x12: return "--> Jet not running\n";
                case (char)0x13: return "--> Jet not idle\n";
                case (char)0x14: return "--> Print not idle\n";
                case (char)0x15: return "--> Message edit in progress\n";
                case (char)0x16: return "--> Number of bytes in command\n";
                case (char)0x17: return "--> Parameter rejected\n";
                case (char)0x18: return "--> Minimum string length\n";
                case (char)0x19: return "--> Maximum string length\n";
                case (char)0x1A: return "--> Minimum value\n";
                case (char)0x1B: return "--> Maximum value\n";
                case (char)0x1C: return "--> Memory full\n";
                case (char)0x1D: return "--> No character sets\n";
                case (char)0x1E: return "--> No barcodes\n";
                case (char)0x1F: return "--> No logos\n";
                case (char)0x20: return "--> No date formats\n";
                case (char)0x21: return "--> PROM - based data set specified\n";
                case (char)0x22: return "--> Unknown data set\n";
                case (char)0x23: return "--> No messages\n";
                case (char)0x24: return "--> Unknown message\n";
                case (char)0x25: return "--> Field too large\n";
                case (char)0x26: return "--> Additional message overwrite\n";
                case (char)0x27: return "--> Non - alphanumeric character\n";
                case (char)0x28: return "--> Positive value\n";
                case (char)0x29: return "--> Trigger print: Photocell mode\n";
                case (char)0x2A: return "--> Trigger print: Print idle\n";
                case (char)0x2B: return "--> Trigger print: Already printing\n";
                case (char)0x2C: return "--> Trigger print: Cover off\n";
                case (char)0x2D: return "--> Print command: Jet not running\n";
                case (char)0x2E: return "--> Print command: No message\n";
                case (char)0x2F: return "--> Jet command: Ink low\n";
                case (char)0x30: return "--> Jet command: Solvent low\n";
                case (char)0x31: return "--> Jet command: Print fail\n";
                case (char)0x32: return "--> Jet command: Print in progress\n";
                case (char)0x33: return "--> Jet command: Phase\n";
                case (char)0x34: return "--> Jet command: Time of flight\n";
                case (char)0x35: return "--> Cal.printhead: Try later\n";
                case (char)0x36: return "--> Cal.printhead: Failed\n";
                case (char)0x37: return "--> Message too large\n";
                case (char)0x38: return "--> Pixel RAM overflow\n";
                case (char)0x39: return "--> Invalid message format\n";
                case (char)0x3A: return "--> Invalid field type\n";
                case (char)0x3B: return "--> No print message loaded\n";
                case (char)0x3C: return "--> Invalid print mode\n";
                case (char)0x3D: return "--> Invalid failure condition\n";
                case (char)0x3E: return "--> Invalid buffer divisor\n";
                case (char)0x3F: return "--> No remote fields in message\n";
                case (char)0x40: return "--> Number of remote characters\n";
                case (char)0x41: return "--> Remote data too large\n";
                case (char)0x42: return "--> Remote buffer now full\n";
                case (char)0x43: return "--> Remote buffer still full\n";
                case (char)0x44: return "--> Field data exceeds message end\n";
                case (char)0x45: return "--> Invalid remote field type\n";
                case (char)0x46: return "--> Invalid while display enabled\n";
                case (char)0x4F: return "--> Too many messages specified\n";
                case (char)0x51: return "--> Printer busy\n";
                case (char)0x52: return "--> Unknown raster\n";
                case (char)0x53: return "--> Invalid field length\n";
                case (char)0x54: return "--> Duplicate name\n";
                case (char)0x55: return "--> Invalid barcode linkage\n";
                case (char)0x56: return "--> Data set in ROM\n";
                case (char)0x57: return "--> Data set in use\n";
                case (char)0x58: return "--> Invalid field height\n";
                case (char)0x59: return "--> Production Schedule: No message schedules\n";
                case (char)0x5A: return "--> Production Schedule: Too many message schedules\n";
                case (char)0x5B: return "--> Production Schedule: Unknown message schedule\n";
                case (char)0x5C: return "--> Production Schedule: Duplicate message schedule\n";
                case (char)0x5D: return "--> Overlapping fields\n";
                case (char)0x5E: return "--> Not Calibrated\n";
                default:
                    return "";
            }


        }
    }
    public static class JetStateList
    {
        public static string ToDescription(char ch)
        {
            switch (ch)
            {
                case (char)0x0: return "--> [Jet Running] - Hệ thống đã khởi động phun xong.\n";
                case (char)0x1: return "--> [Jet Startup] - Hệ thống đang khởi động phun.\n";
                case (char)0x2: return "--> [Jet Shutdown] - Hệ thống đang trong thực hiện trình tự tắt phun\n";
                case (char)0x3: return "--> [Jet Stopped (tia đã dừng)] - Hệ thống đã tắt phun xong, có thể tắt nguồn\n";
                case (char)0x4: return "--> [Fault] - Có lỗi phun xuất hiện, hệ thống sẽ tắt\n";
                default:
                    return "";
            }
        }
    }
    public static class PrintState
    {
        public static string ToDescription(char ch)
        {
            switch (ch)
            {
                case (char)0x00: return "--> Printing Đang in bản tin.\n";
                case (char)0x01: return "--> Undefined Không xác định.\n";
                case (char)0x02: return "--> Idle Sẵn sàng cho lệnh bắt đầu in.\n";
                case (char)0x03: return "--> Generating Pixels Đang tạo mẫu pixel cho lần in tiếp theo.\n";
                case (char)0x04: return "--> Waiting Đang đợi trình kích hoạt in tiếp theo, hoặc đợi thời gian chờ in hết hạn.\n";
                case (char)0x05: return "--> Last In mẫu cuối cùng sau lệnh In Dừng.\n";
                case (char)0x06: return "--> Printing / Generating Pixels In một mẫu và tạo mẫu pixel cho lần in tiếp theo.\n";
                default:
                    return "";
            }
        }
    }
    public static class ErrorMask
    {
        private static string ToDescription(int pos)
        {
            switch (pos)
            {
                case 0:  return "--> No Time of Flight\n";
                case 1:  return "--> Shutdown Incomplete\n";
                case 2:  return "--> Over Speed(Print Trigger)\n";
                case 3:  return "--> Ink Low\n";
                case 4:  return "--> Solvent Low\n";
                case 5:  return "--> Over Speed(No Remote Data)\n";
                case 6:  return "--> Printer Requires Scheduled Maintenance\n";
                case 7:  return "--> Printhead Cover Off\n";
                case 8:  return "--> Over Speed(Synchronous Data)\n";
                case 9:  return "--> Over Speed(Line Speed)\n";
                case 10: return "--> Over Speed(Compensation)\n";
                case 11: return "--> Safety Override Active\n";
                case 12: return "--> Low Pressure\n";
                case 13: return "--> Under Speed(Line Speed)\n";
                case 14: return "--> Over Speed(Asynchronous Data)\n";
                case 15: return "--> Not Used\n";
                case 16: return "--> User Data Corrupt\n";
                case 17: return "--> Memory Corrupt\n";
                case 18: return "--> No Message in Memory\n";
                case 19: return "--> Not Used\n";
                case 20: return "--> Remote Error\n";
                case 21: return "--> Not Used\n";
                case 22: return "--> Corrupt Program Data\n";
                case 23: return "--> Not Used\n";
                case 24: return "--> Not Used\n";
                case 25: return "--> Not Used\n";
                case 26: return "--> Not Used\n";
                case 27: return "--> Print Go After Schedule End\n";
                case 28: return "--> Not Used\n";
                case 29: return "--> Not Used\n";
                case 30: return "--> Not Used\n";
                case 31: return "--> Extended Errors Present\n";
                default:
                    return "";
            }
        }
        public static string ToDescription(char[] arrCh)
        {
            string s = "";
            string description = "";
            for (int i = 0; i < arrCh.Length; i++)
            {
                s += Convert.ToString((byte)arrCh[i], 2);
            }
            char[] ch = s.ToArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] == '1')
                {
                    description += ToDescription(i) + "\n";
                }
            }
            return description;
        }
    }
}
