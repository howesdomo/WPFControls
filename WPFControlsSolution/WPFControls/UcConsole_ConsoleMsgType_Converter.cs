using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WPFControls.ValueConverters
{
    public class UcConsole_ConsoleMsgType_Converter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueStr = (value ?? string.Empty).ToString();

            if (valueStr == "BUSINESSERROR")
            {
                valueStr = "BERROR";
            }

            if (string.IsNullOrWhiteSpace(valueStr) == false)
            {
                valueStr = $"[{valueStr}]";
            }

            return valueStr.PadRight(10, ' ');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
