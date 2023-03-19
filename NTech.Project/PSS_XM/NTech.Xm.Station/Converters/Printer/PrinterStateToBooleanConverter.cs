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
    public class PrinterStateToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (((string)parameter).Equals("ForCheckbox"))
                {
                    switch ((PRINTER_STATES)value)
                    {
                        case PRINTER_STATES.CONNECTED:
                        case PRINTER_STATES.DISCONNECTED:
                        case PRINTER_STATES.PRINT_DONE:
                        case PRINTER_STATES.IS_CAN_PRINT:
                        case PRINTER_STATES.PRINT_READY:
                            return true;
                        case PRINTER_STATES.PRINTING:
                        case PRINTER_STATES.PRINT_STOP:
                            return false;
                        default:
                            return false;
                    }
                }
                else
                {
                    switch ((PRINTER_STATES)value)
                    {
                        case PRINTER_STATES.CONNECTED:
                        case PRINTER_STATES.DISCONNECTED:
                        case PRINTER_STATES.PRINT_DONE:
                        case PRINTER_STATES.IS_CAN_PRINT:
                        case PRINTER_STATES.PRINT_READY:
                            return false;
                        case PRINTER_STATES.PRINTING:
                        case PRINTER_STATES.PRINT_STOP:
                            return true;
                        default:
                            return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class PrinterStateToBoolean3Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((PRINTER_STATES)value)
                {
                    case PRINTER_STATES.IS_CAN_PRINT:
                        return true;
                    case PRINTER_STATES.DISCONNECTED:
                    case PRINTER_STATES.CONNECTED:
                    case PRINTER_STATES.PRINT_READY:
                    case PRINTER_STATES.PRINTING:
                    case PRINTER_STATES.PRINT_STOP:
                    case PRINTER_STATES.PRINT_DONE:
                        return false;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MultiBoleanConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (((bool)value[0]) && !((bool)value[1]) && ((bool)value[2]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
