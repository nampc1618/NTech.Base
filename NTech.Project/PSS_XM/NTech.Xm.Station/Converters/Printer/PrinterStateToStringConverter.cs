using NTech.Xm.Station.Commons.Defines;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NTech.Xm.Station.Converters.Printer
{
    public class PrinterStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((PRINTER_STATES)value)
                {
                    case PRINTER_STATES.CONNECTED:
                        return "Đã kết nối";
                    case PRINTER_STATES.DISCONNECTED:
                        return string.Empty;
                    case PRINTER_STATES.PRINTING:
                        return "Đang in";
                    case PRINTER_STATES.PRINT_STOP:
                        return "Dừng in";
                    case PRINTER_STATES.PRINT_DONE:
                        return "Đã in xong";
                    default:
                        return string.Empty;
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
