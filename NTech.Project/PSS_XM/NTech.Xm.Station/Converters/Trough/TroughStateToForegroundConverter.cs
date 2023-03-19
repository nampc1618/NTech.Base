using NTech.Xm.Station.Commons.Defines;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace NTech.Xm.Station.Converters.Trough
{
    internal class TroughStateToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((TROUGH_STATES)value)
                {
                    case TROUGH_STATES.CHECKING:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3");
                    case TROUGH_STATES.ACTIVE_READY:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#2196F3");
                    case TROUGH_STATES.ACTIVE_NOREADY:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#F44336");
                    case TROUGH_STATES.IS_ACTIVE:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#00C853");
                    default:
                        return (SolidColorBrush)new BrushConverter().ConvertFromString("#F44336");
                }
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#F44336");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
