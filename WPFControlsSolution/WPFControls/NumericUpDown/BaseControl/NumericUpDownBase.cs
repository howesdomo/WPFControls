using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFControls.BaseControl
{
    public class NumericUpDownBase : Client.Components.TextBoxAdv_V0
    {
        // TODO 使用 Client.Components.TextBoxAdv 时, 无法显示 + - Button 按钮

        static NumericUpDownBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDownBase), new FrameworkPropertyMetadata(typeof(NumericUpDownBase)));
        }

        public enum UpDownOrientationEnum
        {
            Vertical,
            Horizontal,
        }

        #region [DP] UpDownOrientation

        public static readonly DependencyProperty UpDownOrientationProperty = DependencyProperty.Register("UpDownOrientation"
            , typeof(UpDownOrientationEnum), typeof(Client.Components.TextBoxAdv_V0));
        /// <summary>
        /// 加减按钮布局方向
        /// </summary>
        public UpDownOrientationEnum UpDownOrientation
        {
            get { return (UpDownOrientationEnum)GetValue(UpDownOrientationProperty); }
            set { SetValue(UpDownOrientationProperty, value); }
        }

        #endregion
    }
}
