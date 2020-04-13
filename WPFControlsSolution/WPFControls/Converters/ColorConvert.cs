using System;
using System.Globalization;


namespace Client.ValueConvert // TODO 加上er
{
    public class ColorConvert : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueStr = (value ?? string.Empty).ToString();
            // 不能使用在 Foreground , 请使用 BrushConvert
            return Client.Common.WPFColorUtils.String2Color(valueStr);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
