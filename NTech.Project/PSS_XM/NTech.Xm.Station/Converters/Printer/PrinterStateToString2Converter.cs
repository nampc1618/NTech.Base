using NTech.Xm.Station.Commons.Defines;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NTech.Xm.Station.Converters.Printer
{
    public class PrinterStateToString2Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((PRINTER_STATES)value)
                {
                    case PRINTER_STATES.CONNECTED:
                    case PRINTER_STATES.IS_CAN_PRINT:
                    case PRINTER_STATES.PRINTING:
                    case PRINTER_STATES.PRINT_DONE:
                    case PRINTER_STATES.PRINT_STOP:
                        return "NGẮT KẾT NỐI";
                    case PRINTER_STATES.DISCONNECTED:
                        return "KẾT NỐI";
                    default:
                        return "KẾT NỐI";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
