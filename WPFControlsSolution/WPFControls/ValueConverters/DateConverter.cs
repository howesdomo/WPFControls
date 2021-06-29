using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Client.ValueConverters
{
    public class DateConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string format = "yyyy-MM-dd";

            if (parameter != null && string.IsNullOrWhiteSpace(parameter.ToString()))
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
