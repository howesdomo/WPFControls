using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WPFControls.ValueConverters
{
    public class ContentTextFilterConditionConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is Util.Model.ContentTextFilterCondition v)
            {
                switch (v)
                {
                    case Util.Model.ContentTextFilterCondition.None:
                        return string.Empty;
                    default:
                        return v.ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
