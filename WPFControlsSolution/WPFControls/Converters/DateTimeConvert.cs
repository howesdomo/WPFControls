using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Client.ValueConvert // TODO 加上er
{
    public class DateTimeConvert : System.Windows.Data.IValueConverter // TODO 加上er
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueStr = (value ?? string.Empty).ToString();
            return ((DateTime)value).ToString("MM-dd HH:mm:ss.fff");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
