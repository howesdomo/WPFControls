using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Common
{
    public class WPFColorUtils
    {
        #region Color 2 String

        public static string Color2HexString(System.Windows.Media.Color c)
        {
            return WPFColorUtils.GetHexString(c.R, c.G, c.B);
        }

        public static string GetHexString(double a_r, double a_g, double a_b)
        {
            int r, g, b = 0;

            r = Convert.ToInt32(a_r * 255d);
            g = Convert.ToInt32(a_g * 255d);
            b = Convert.ToInt32(a_b * 255d);

            string result = string.Format("#{0}{1}{2}",
                    r.ToString("X").PadLeft(2, '0'),
                    g.ToString("X").PadLeft(2, '0'),
                    b.ToString("X").PadLeft(2, '0'));

            return result;
        }

        public static string Color2HexWithAlphaString(System.Windows.Media.Color c)
        {
            return WPFColorUtils.GetHexWithAlphaString(c.R, c.G, c.B, c.A);
        }

        public static string GetHexWithAlphaString(double a_r, double a_g, double a_b, double a_a)
        {
            int r, g, b, a = 0;

            r = Convert.ToInt32(a_r * 255d);
            g = Convert.ToInt32(a_g * 255d);
            b = Convert.ToInt32(a_b * 255d);
            a = Convert.ToInt32(a_a * 255d);

            string result = string.Format
            (
                "#{0}{1}{2}{3}",
                    r.ToString("X").PadLeft(2, '0'),
                    g.ToString("X").PadLeft(2, '0'),
                    b.ToString("X").PadLeft(2, '0'),
                    a.ToString("X").PadLeft(2, '0')
            )
            ;

            return result;
        }

        #endregion

        #region String 2 Color

        private static System.Text.RegularExpressions.Regex sRegexHex = new System.Text.RegularExpressions.Regex("^#[0-9A-Fa-f]{6}$");

        private static System.Text.RegularExpressions.Regex sRegexHexWithAlpha = new System.Text.RegularExpressions.Regex("^#[0-9A-Fa-f]{8}$");

        /// <summary>
        /// 根据字符串获取颜色
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color String2Color(string args)
        {
            System.Windows.Media.Color result = System.Windows.Media.Colors.Black;

            byte intR, intG, intB = 0;
            byte intA = 0;

            try
            {
                if (sRegexHex.IsMatch(args) == true)
                {
                    // 通过 Hex 获取颜色
                    string r = args.Substring(1, 2);
                    string g = args.Substring(3, 2);
                    string b = args.Substring(5, 2);

                    intR = Convert.ToByte(r, 16);
                    intG = Convert.ToByte(g, 16);
                    intB = Convert.ToByte(b, 16);

                    result = System.Windows.Media.Color.FromArgb(255, intR, intG, intB);
                }
                else if (sRegexHexWithAlpha.IsMatch(args) == true)
                {
                    // 通过 Hex With Alpha 获取颜色
                    string a = args.Substring(1, 2);
                    string r = args.Substring(3, 2);
                    string g = args.Substring(5, 2);
                    string b = args.Substring(7, 2);

                    intA = Convert.ToByte(a, 16);
                    intR = Convert.ToByte(r, 16);
                    intG = Convert.ToByte(g, 16);
                    intB = Convert.ToByte(b, 16);

                    result = System.Windows.Media.Color.FromArgb(intA, intR, intG, intB);
                }
                else
                {
                    System.Reflection.PropertyInfo propertyInfo = typeof(System.Windows.Media.Colors).GetProperty(args);
                    if (propertyInfo != null)
                    {
                        result = (System.Windows.Media.Color)propertyInfo.GetValue(null, null);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"{args} not found in System.Windows.Media.Color");
#if DEBUG
                        System.Diagnostics.Debugger.Break();
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif

                result = System.Windows.Media.Colors.Black;
            }

            return result;
        }

        #endregion
    }
}
