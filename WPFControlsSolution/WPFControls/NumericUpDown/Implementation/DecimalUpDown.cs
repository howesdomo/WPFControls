using System;
using System.Windows;
using System.Windows.Controls;

using WPFControls.BaseControl;

namespace Client.Components
{
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_UP", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DOWN", Type = typeof(Button))]
    public class DecimalUpDown : NumericUpDown<decimal>
    {
        public DecimalUpDown() : base()
        {
            this.Minimum = 0m;
            this.Maximum = 100m;
            this.Value = this.Minimum;
            this.Increment = 1m;
        }

        protected override decimal IncrementValue(decimal value, decimal increment)
        {
            return value + increment;
        }

        protected override decimal DecrementValue(decimal value, decimal increment)
        {
            return value - increment;
        }

        protected override decimal ParseValue(string value)
        {
            decimal temp = 0;
            if (decimal.TryParse(value, out temp))
            {
                return temp;
            }
            else
            {
                return decimal.MinValue;
            }
        }
    }
}
