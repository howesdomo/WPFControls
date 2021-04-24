using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

/// <summary>
/// V 1.0.0 - 2021-04-24 17:04:43
/// 用于 WPF 的 XAML转换空值为其实
/// </summary>
namespace Client.ValueConverters
{
    public class ExpectNull2CollapsedConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility r = System.Windows.Visibility.Visible;

            if (value is null)
            {
                r = System.Windows.Visibility.Collapsed;
            }

            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ExpectNull2HiddenConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility r = System.Windows.Visibility.Visible;

            if (value is null)
            {
                r = System.Windows.Visibility.Hidden;
            }

            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ExpectNotNull2CollapsedConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility r = System.Windows.Visibility.Visible;

            if (value is null == false)
            {
                r = System.Windows.Visibility.Collapsed;
            }

            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ExpectNotNull2HiddenConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility r = System.Windows.Visibility.Visible;

            if (value is null == false)
            {
                r = System.Windows.Visibility.Hidden;
            }

            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
