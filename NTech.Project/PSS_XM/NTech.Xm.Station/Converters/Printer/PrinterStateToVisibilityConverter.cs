using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using NTech.Xm.Station.Commons.Defines;

namespace NTech.Xm.Station.Converters.Printer
{
    public class PrinterStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((PRINTER_STATES)value)
                {
                    case PRINTER_STATES.CONNECTED:
                    case PRINTER_STATES.IS_CAN_PRINT:
                    case PRINTER_STATES.PRINT_READY:
                    case PRINTER_STATES.PRINTING:
                    case PRINTER_STATES.PRINT_STOP:
                    case PRINTER_STATES.PRINT_DONE:
                        return Visibility.Visible;
                    case PRINTER_STATES.DISCONNECTED:
                        return Visibility.Collapsed;
                    default:
                        return Visibility.Collapsed;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
