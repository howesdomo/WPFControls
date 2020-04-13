﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Client.ValueConvert // TODO 加上er
{
    public class BrushConvert : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueStr = (value ?? string.Empty).ToString();
            return new SolidColorBrush(Client.Common.WPFColorUtils.String2Color(valueStr));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
