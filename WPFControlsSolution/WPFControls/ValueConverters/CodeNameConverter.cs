using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.ValueConverters
{
    public class CodeNameConverter : System.Windows.Data.IValueConverter
    {
        public const string _CODE_ = "Code";
        public const string _NAME_ = "Name";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            string code;
            string name;

            Type t = value.GetType();

            try
            {
                // .net framework 4.6.1 的写法可以一句搞掂
                //code = t.GetProperty(_Code_)?.GetValue(value).ToString();

                var prop0 = t.GetProperty(_CODE_);
                if (prop0 != null)
                {
                    // .net framework 4.0 的反射写法真麻烦
                    code = prop0.GetValue(value, prop0.GetIndexParameters()).ToString();
                }
                else
                {
                    var field0 = t.GetField(_CODE_);
                    code = field0.GetValue(value).ToString();
                }

                // .net framework 4.6.1 的写法可以一句搞掂

                var prop1 = t.GetProperty(_NAME_);
                if (prop1 != null)
                {
                    // .net framework 4.0 的反射写法真麻烦
                    name = prop1.GetValue(value, prop1.GetIndexParameters()).ToString();
                }
                else
                {
                    var field1 = t.GetField(_NAME_);
                    name = field1.GetValue(value).ToString();
                }

                return $"{code}-{name}";
            }
            catch (Exception)
            {
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UserCodeNameValueConvert : System.Windows.Data.IValueConverter
    {
        public const string _CODE_ = "LoginAccount";
        public const string _NAME_ = "UserName";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            string code;
            string name;

            Type t = value.GetType();

            try
            {
                // .net framework 4.6.1 的写法可以一句搞掂
                //code = t.GetProperty(_Code_)?.GetValue(value).ToString();

                var prop0 = t.GetProperty(_CODE_);
                if (prop0 != null)
                {
                    // .net framework 4.0 的反射写法真麻烦
                    code = prop0.GetValue(value, prop0.GetIndexParameters()).ToString();
                }
                else
                {
                    var field0 = t.GetField(_CODE_);
                    code = field0.GetValue(value).ToString();
                }

                // .net framework 4.6.1 的写法可以一句搞掂

                var prop1 = t.GetProperty(_NAME_);
                if (prop1 != null)
                {
                    // .net framework 4.0 的反射写法真麻烦
                    name = prop1.GetValue(value, prop1.GetIndexParameters()).ToString();
                }
                else
                {
                    var field1 = t.GetField(_NAME_);
                    name = field1.GetValue(value).ToString();
                }

                return $"{code}-{name}";
            }
            catch (Exception)
            {
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
