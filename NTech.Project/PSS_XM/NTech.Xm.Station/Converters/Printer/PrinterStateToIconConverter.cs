using MahApps.Metro.IconPacks;
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
    public class PrinterStateToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((PRINTER_STATES)value)
                {
                    case PRINTER_STATES.IS_CAN_PRINT:
                    case PRINTER_STATES.PRINTING:
                    case PRINTER_STATES.PRINT_DONE:
                    case PRINTER_STATES.PRINT_STOP:
                    case PRINTER_STATES.CONNECTED:
                        return PackIconMaterialKind.LanDisconnect;
                    case PRINTER_STATES.DISCONNECTED:
                        return PackIconMaterialKind.Connection;
                    default:
                        return PackIconMaterialKind.Connection;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
