using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NTech.Xm.Gate.Converters
{
    public class TextToEnableMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool res = true;
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (string.IsNullOrEmpty((string)value))
                    {
                        res = false;
                        return res;
                    }
                }
                return res;
            }
            else
            {
                return false;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
