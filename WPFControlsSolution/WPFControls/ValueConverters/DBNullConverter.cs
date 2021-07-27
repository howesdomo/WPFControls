using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Client.ValueConverters
{
    public class DBNullConverter : IValueConverter
    {
        public const string DBNull_DisplayValue = "\x02(NULL)\x03";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.DBNull)
            {
                return DBNull_DisplayValue;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
