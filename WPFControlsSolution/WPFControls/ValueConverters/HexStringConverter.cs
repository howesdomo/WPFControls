using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Client.ValueConverters
{
    /// <summary>
    /// Textbox输入Hex字符串格式转换器
    /// 输入
    /// 0110AB
    /// 界面显示
    /// 01 10 AB
    /// </summary>
    public class HexStringConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string ret = null;

            if (value != null && value is string)
            {
                var valueAsString = value as string;

                // var parts = valueAsString.ToCharArray();
                // var formatted = parts.Select((p, i) => (++i) % 2 == 0 ? string.Concat(p.ToString(), " ") : p.ToString());

                var formatted = valueAsString
                                .ToCharArray()
                                .Select((p, i) => (++i) % 2 == 0 ? string.Concat(p.ToString(), " ") : p.ToString());

                ret = string.Join(string.Empty, formatted).Trim();
            }

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object ret = null;

            if (value != null && value is string)
            {
                var valueAsString = ((string)value).Replace(" ", string.Empty).ToUpper();
                ret = valueAsString;
            }

            return ret;
        }
    }
}
