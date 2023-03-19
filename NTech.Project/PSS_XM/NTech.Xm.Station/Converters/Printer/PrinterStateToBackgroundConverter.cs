using NTech.Xm.Station.Commons.Defines;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace NTech.Xm.Station.Converters.Printer
{
    public class PrinterStateToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((PRINTER_STATES)value)
                {
                    case PRINTER_STATES.CONNECTED:
                        if (string.Equals((string)parameter, "1"))
                            return (SolidColorBrush)new BrushConverter().ConvertFromString("#D32F2F");
                        else
                            return (SolidColorBrush)new BrushConverter().ConvertFromString("#00C853");
                    case PRINTER_STATES.PRINTING:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#00C853");
                    case PRINTER_STATES.PRINT_STOP:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#F44336");
                    case PRINTER_STATES.IS_CAN_PRINT:
                    case PRINTER_STATES.PRINT_DONE:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3");
                    case PRINTER_STATES.DISCONNECTED:
                        if (string.Equals((string)parameter, "1"))
                            return (SolidColorBrush)new BrushConverter().ConvertFromString("#00C853");
                        else
                            return (SolidColorBrush)new BrushConverter().ConvertFromString("#D32F2F");
                    default:
                        return Brushes.Transparent;
                }
            }
            else
            {
                return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
