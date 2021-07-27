using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Client.ValueConverters
{
    public class DateTimeConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string format = "yyyy-MM-dd HH:mm:ss.fff";

            if (parameter != null && string.IsNullOrWhiteSpace(parameter.ToString()) == false)
            {
                format = parameter.ToString();
            }

            return ((DateTime)value).ToString(format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


}
