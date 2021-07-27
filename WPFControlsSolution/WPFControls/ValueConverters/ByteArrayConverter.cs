using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Client.ValueConverters
{
    public class ByteArrayConverter : IValueConverter
    {
        public const string DBNull_DisplayValue = "\x02(NULL)\x03";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.DBNull)
            {
                return DBNull_DisplayValue;
            }

            if (value is byte[] bArr)
            {
                int maxLength = 16;

                if (bArr.Length <= maxLength)
                {
                    return ToHex(bArr);
                }
                else
                {
                    // 获取前 maxLength 的值
                    byte[] k = new byte[maxLength];

                    for (int i = 0; i < maxLength; i++)
                    {
                        k[i] = bArr[i];
                    }

                    return $"{ToHex(k)}...";
                }
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

        /// <summary>
        /// 来自HowesDOMO.Utils
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        // public static string ToHex(this IEnumerable<byte> bytes)
        public static string ToHex(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
