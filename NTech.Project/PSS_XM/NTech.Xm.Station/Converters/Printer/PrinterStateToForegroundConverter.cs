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
    public class PrinterStateToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((PRINTER_STATES)value)
                {
                    case PRINTER_STATES.IS_CAN_PRINT:
                        return Brushes.DarkMagenta;
                    case PRINTER_STATES.CONNECTED:
                    case PRINTER_STATES.PRINT_READY:
                    case PRINTER_STATES.PRINT_DONE:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3");
                    case PRINTER_STATES.PRINTING:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#00C853");
                    case PRINTER_STATES.PRINT_STOP:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#F44336");
                    case PRINTER_STATES.DISCONNECTED:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
                    default:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
                }
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MessageStateToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((MESSAGE_STATE)value)
                {
                    case MESSAGE_STATE.NO_DEFINE:
                        return Brushes.Transparent;
                    case MESSAGE_STATE.NOT_YET_PRINT:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#F44336");
                    case MESSAGE_STATE.NOT_PRINT_DONE:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#F44336");
                    case MESSAGE_STATE.PRINTING:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3");
                    case MESSAGE_STATE.PRINT_DONE:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#4CAF50");
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
