using NTech.Xm.Station.Commons.Defines;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NTech.Xm.Station.Converters.Printer
{
    public class PrinterStateToBoolean2Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((PRINTER_STATES)value)
                {
                    case PRINTER_STATES.PRINTING:
                        return true;
                    case PRINTER_STATES.PRINT_STOP:
                    case PRINTER_STATES.IS_CAN_PRINT:
                    case PRINTER_STATES.PRINT_READY:
                    case PRINTER_STATES.PRINT_DONE:
                        return false;
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
