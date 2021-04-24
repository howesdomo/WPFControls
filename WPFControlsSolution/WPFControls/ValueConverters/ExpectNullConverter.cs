using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Client.ValueConverters
{
    /// <summary>
    /// <para>期望空值</para>
    /// <para>返回 bool</para>
    /// <para>空值 返回 true</para>
    /// <para>非空 返回 false</para>
    /// </summary>
    public class ExpectNullConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool r = false;

            if (value is null)
            {
                r = true;
            }

            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// <para>期望非空</para>
    /// <para>返回 bool</para>
    /// <para>空值 返回 false</para>
    /// <para>非空 返回 true</para>
    /// </summary>
    public class ExpectNotNullConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool r = true;

            if (value is null)
            {
                r = false;
            }

            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
