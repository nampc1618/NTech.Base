using NTech.Xm.Station.Commons.Defines;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NTech.Xm.Station.Converters.Trough
{
    public class IsCheckedReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((TROUGH_STATES)value)
                {
                    case TROUGH_STATES.CHECKING:
                    case TROUGH_STATES.ACTIVE_READY:
                    case TROUGH_STATES.ACTIVE_NOREADY:
                    case TROUGH_STATES.IS_ACTIVE:
                        return Visibility.Visible;
                    case TROUGH_STATES.NO_DEFINE:
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
