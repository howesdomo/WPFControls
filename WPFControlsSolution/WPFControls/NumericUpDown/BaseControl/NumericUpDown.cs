﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFControls.BaseControl
{
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_UP", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DOWN", Type = typeof(Button))]
    public abstract class NumericUpDown<T> : NumericUpDownBase where T : struct, IComparable
    {
        private Button PART_UP;
        private Button PART_DOWN;

        protected abstract T IncrementValue(T value, T increment);
        protected abstract T DecrementValue(T value, T increment);
        protected abstract T ParseValue(string value);

        #region 事件

        public delegate void UpButtonClickHandler();
        public UpButtonClickHandler UpButtonClick;

        public delegate void DownButtonClickHandler();
        public DownButtonClickHandler DownButtonClick;

        public delegate void ValueChangedHandler(object newValue);
        public ValueChangedHandler ValueChanged;

        #endregion

        #region 依赖属性

        #region [DP] Maximum

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum"
            , typeof(T), typeof(NumericUpDown<T>), new UIPropertyMetadata(default(T)));

        /// <summary>
        /// 最大值
        /// </summary>
        public T Maximum
        {
            get { return (T)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static object OnCoerceMaximum(DependencyObject d, object baseValue)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                return numericUpDown.OnCoerceMaximum((T)((object)baseValue));
            }
            return baseValue;
        }

        protected virtual T OnCoerceMaximum(T baseValue)
        {
            return baseValue;
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region [DP] Minimum

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum"
            , typeof(T), typeof(NumericUpDown<T>), new UIPropertyMetadata(default(T)));
        /// <summary>
        /// 最小值
        /// </summary>
        public T Minimum
        {
            get { return (T)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        #endregion

        #region [DP] Increment

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment"
            , typeof(T), typeof(NumericUpDown<T>), new UIPropertyMetadata(default(T)));
        /// <summary>
        /// 增减量
        /// </summary>
        public T Increment
        {
            get { return (T)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        #endregion

        #region [DP] Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value"
            , typeof(T), typeof(NumericUpDown<T>));
        /// <summary>
        /// 当前值
        /// </summary>
        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region [DP] IsShowTips

        public static readonly DependencyProperty IsShowTipsProperty = DependencyProperty.Register("IsShowTips"
            , typeof(bool), typeof(NumericUpDown<T>));
        /// <summary>
        /// 是否显示异常提示
        /// </summary>
        public bool IsShowTips
        {
            get { return (bool)GetValue(IsShowTipsProperty); }
            set { SetValue(IsShowTipsProperty, value); }
        }

        #endregion

        #region [DP] TipsText

        public static readonly DependencyProperty TipsTextProperty = DependencyProperty.Register("TipsText"
            , typeof(string), typeof(NumericUpDown<T>));
        /// <summary>
        /// 提示内容
        /// </summary>
        public string TipsText
        {
            get { return (string)GetValue(TipsTextProperty); }
            set { SetValue(TipsTextProperty, value); }
        }

        #endregion

        #region [DP] TipsBackground

        public static readonly DependencyProperty TipsBackgroundProperty = DependencyProperty.Register("TipsBackground"
            , typeof(Brush), typeof(NumericUpDown<T>), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Red)));
        /// <summary>
        /// 提示内容背景色
        /// </summary>
        public Brush TipsBackground
        {
            get { return (Brush)GetValue(TipsBackgroundProperty); }
            set { SetValue(TipsBackgroundProperty, value); }
        }

        #endregion

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_UP = WPFControlsUtils.FindChildOfType<Button>(this, "PART_UP"); // Edit By Howe
            this.PART_DOWN = WPFControlsUtils.FindChildOfType<Button>(this, "PART_DOWN"); // Edit By Howe

            if (this.PART_UP != null)
            {
                this.PART_UP.Click += BtnUp_Click;
            }

            if(this.PART_DOWN != null)
            {
                this.PART_DOWN.Click += BtnDown_Click;
            }
            
            this.TextChanged += NumericUpDown_TextChanged;
            this.KeyUp += NumericUpDown_KeyUp;

            this.SetBtnEnabled(this.Value.ToString());
            this.MoveCursorToEnd();
            this.Value = this.CoreValueCompareMinMax(this.Value);
        }

        private void NumericUpDown_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!this.IsFocused) return;

            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    if (this.UpButtonClick != null)
                    {
                        this.UpButtonClick();
                    }
                    break;
                case System.Windows.Input.Key.Down:
                    if (this.DownButtonClick != null)
                    {
                        this.DownButtonClick();
                    }
                    break;
                default:
                    break;
            }
        }

        private void NumericUpDown_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(this.IsReadOnly)
            {
                return;
            }

            IsShowTips = false;
            string newValue = ((TextBox)sender).Text;
            EnumCompare type;

            this.Value = this.CoreValueCompareMinMax(this.ParseValue(newValue), out type);
            switch (type)
            {
                case EnumCompare.Less:
                    IsShowTips = true;
                    TipsText = string.Format("校验失败，小于最小值{1}", newValue, this.Minimum);
                    break;
                case EnumCompare.Large:
                    IsShowTips = true;
                    TipsText = string.Format("校验失败，大于最大值{1}", newValue, this.Maximum);
                    break;
            }

            //if (this.ValueChanged != null)
            //{
            //    ValueChanged(newValue);
            //}
            this.SetBtnEnabled(newValue);
            this.MoveCursorToEnd();
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            //if(this.UpButtonClick != null)
            //{
            //    this.UpButtonClick();
            //}

            T value = this.IncrementValue(this.Value, this.Increment);            
            this.Value = this.CoreValueCompareMinMax(value);

            this.MoveCursorToEnd();
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            //if (this.DownButtonClick != null)
            //{
            //    this.DownButtonClick();
            //}

            T value = this.DecrementValue(this.Value, this.Increment);
            this.Value = this.CoreValueCompareMinMax(value);

            this.MoveCursorToEnd();
        }

        private T CoreValueCompareMinMax(T value)
        {
            T result = value;

            if (this.IsLowerThan(value, this.Minimum))
            {
                result = this.Minimum;
            }
            else
            {
                if (this.IsLagerThan(value, this.Maximum))
                {
                    result = this.Maximum;
                }
            }

            return result;
        }

        private T CoreValueCompareMinMax(T value, out EnumCompare type)
        {
            T result = value;
            type = EnumCompare.None;

            if (this.IsLowerThan(value, this.Minimum))
            {
                result = this.Minimum;
                type = EnumCompare.Less;
            }
            else
            {
                if (this.IsLagerThan(value, this.Maximum))
                {
                    result = this.Maximum;
                    type = EnumCompare.Large;
                }
            }

            return result;
        }

        private bool IsLowerThan(T value1, T value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        private bool IsLagerThan(T value1, T value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        private void SetBtnEnabled(string value)
        {
            if (this.PART_UP != null)
            {
                this.PART_UP.IsEnabled = this.Maximum.ToString() != value;
            }
            if (this.PART_DOWN != null)
            {
                this.PART_DOWN.IsEnabled = this.Minimum.ToString() != value;
            }
        }

        /// <summary>
        /// 将光标移动到数字最后面
        /// </summary>
        private void MoveCursorToEnd()
        {
            this.SelectionStart = Convert.ToString(this.Value).Length;
        }
    }
}
