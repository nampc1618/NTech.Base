using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NTech.Wpf.Modules.Common.Editor.Converters
{
    /// <summary>
    /// [NCS-3103] : Refractive index 창에서 scan image zoom in/out 기능 구현되지 않음
    /// </summary>
    public class RefractiveCoordinateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double d = 0.0d;

            if (values != null)
            {
                var param = (parameter ?? "").ToString();
                switch (param)
                {
                    case "Canvas.Width":
                        {
                            double c = 0d;
                            double z = 0d;

                            if (values.Length > 1)
                            {
                                double.TryParse(values[0].ToString(), out c);
                                double.TryParse(values[1].ToString(), out z);
                            }

                            d = c * z;
                        }
                        break;
                    case "Canvas.Height":
                        {
                            double c = 0d;
                            double z = 0d;

                            if (values.Length > 1)
                            {
                                double.TryParse(values[0].ToString(), out c);
                                double.TryParse(values[1].ToString(), out z);
                            }

                            d = c * z;
                        }
                        break;
                    case "Label.X":
                        {
                            double c = 0d;
                            double z = 0d;
                            double s = 0d;

                            if (values.Length > 2)
                            {
                                double.TryParse(values[0].ToString(), out c);
                                double.TryParse(values[1].ToString(), out z);
                                double.TryParse(values[2].ToString(), out s);

                                if (s < 0) { s = 0; }
                            }

                            d = s * z + c;
                        }
                        break;
                    case "Label.Y":
                        {
                            double c = 0d;
                            double z = 0d;
                            double s = 0d;

                            if (values.Length > 2)
                            {
                                double.TryParse(values[0].ToString(), out c);
                                double.TryParse(values[1].ToString(), out z);
                                double.TryParse(values[2].ToString(), out s);

                                if (s < 0) { s = 0; }
                            }

                            d = s * z + c;
                        }
                        break;
                }

                if (double.IsInfinity(d)) { d = 0.0d; }
            }

            return d;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
