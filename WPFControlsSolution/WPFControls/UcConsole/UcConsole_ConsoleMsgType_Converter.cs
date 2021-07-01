using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WPFControls.ValueConverters
{
    /// <summary>
    /// 用于 UcConsole 控件显示 ConsoleMsgType 信息
    /// </summary>
    public class UcConsole_ConsoleMsgType_Converter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return string.Empty; }

            if (value is Util.Model.ConsoleMsgType v)
            {
                string valueStr = string.Empty;
                switch (v)
                {
                    case Util.Model.ConsoleMsgType.BUSINESSERROR:
                        valueStr = "BERROR";
                        break;
                    default:
                        valueStr = v.ToString();
                        break;
                }

                if (string.IsNullOrWhiteSpace(valueStr) == false)
                {
                    valueStr = $"[{valueStr}]";
                }

                return valueStr.PadRight(10, ' '); // 为了对齐内容, 增加 PadRight
            }
            else
            {
                throw new Exception("value isn't Util.Model.ConsoleMsgType");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 用于过滤控件 ComboBoxItem
    /// </summary>
    public class UcConsole_ConsoleMsgType_ComboBoxItem_Converter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return string.Empty; }

            if (value is Util.Model.ConsoleMsgType v)
            {
                switch (v)
                {
                    case Util.Model.ConsoleMsgType.NONE:
                        return string.Empty;
                    case Util.Model.ConsoleMsgType.BUSINESSERROR:
                        return "BERROR";
                    default:
                        return v.ToString();
                }
            }
            else
            {
                throw new Exception("value isn't Util.Model.ConsoleMsgType");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
