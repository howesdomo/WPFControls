using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.ValueConverters
{
    /// <summary>
    /// Textbox输入Hex字符串格式转换器
    /// 输入不符合要求的字符串会忽略掉
    /// </summary>
    public class HexStringAutoFixErrorConverter : System.Windows.Data.IValueConverter
    {
        public static System.Text.RegularExpressions.Regex HexRegex = new System.Text.RegularExpressions.Regex("^[0-9A-Fa-f]*$");

        /// <summary>
        /// 上一次成功验证的记录值
        /// </summary>
        private string mLastValidValue;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string ret = null;

            if (value != null && value is string)
            {
                var valueAsString = (string)value;

                //var parts = valueAsString.ToCharArray();
                //var formatted = parts.Select((p, i) => (++i) % 2 == 0 ? string.Concat(p.ToString(), " ") : p.ToString());

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

                // ret = lastValidValue = IsHex(valueAsString) ? valueAsString : lastValidValue;
                if (HexRegex.IsMatch(valueAsString) == true)
                {
                    mLastValidValue = valueAsString;
                }

                ret = valueAsString;
            }

            return ret;
        }
    }
}
