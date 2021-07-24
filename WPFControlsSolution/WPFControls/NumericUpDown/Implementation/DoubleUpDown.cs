using System;
using System.Windows;
using System.Windows.Controls;

using WPFControls.BaseControl;

namespace Client.Components
{

    /// <summary>
    /// 不推荐使用 DoubleUpDown, 当 Increment 等于 0.1 时, 0.2 + 0.1 = 0.3, 但由于实际是使用 NumericUpDown _T_ 
    /// 0.3d 转型时等于 0.30000000000000004d, 从而产生异常的数据
    /// </summary>
    [Obsolete]
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_UP", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DOWN", Type = typeof(Button))]
    public class DoubleUpDown : NumericUpDown<double>
    {
        public DoubleUpDown() : base()
        {
            this.Minimum = 0d;
            this.Maximum = 100d;
            this.Value = this.Minimum;
            this.Increment = 1d;
        }

        protected override double IncrementValue(double value, double increment)
        {
            return value + increment;
        }

        protected override double DecrementValue(double value, double increment)
        {
            return value - increment;
        }

        protected override double ParseValue(string value)
        {
            double temp = 0;
            if (double.TryParse(value, out temp))
            {
                return temp;
            }
            else
            {
                return double.MinValue;
            }
        }
    }
}
