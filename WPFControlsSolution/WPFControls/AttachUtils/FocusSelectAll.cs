using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Client.Controls.AttachUtils
{
    /// <summary>
    /// V 1.0.0 - 2021-05-27 14:09:27
    /// 首次创建 - 拷贝自 Client.Controls.AttachUtils.Focus
    /// </summary>
    public class FocusSelectAll
    {
        #region IsFocus

        // 附加属性 DependencyProperty.RegisterAttached
        public static readonly DependencyProperty IsFocusProperty = DependencyProperty.RegisterAttached
        (
            name: "IsFocus",
            propertyType: typeof(bool),
            ownerType: typeof(FocusSelectAll),
            validateValueCallback: null,
            defaultMetadata: new FrameworkPropertyMetadata()
            {
                DefaultValue = false,
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                PropertyChangedCallback = new PropertyChangedCallback((d, e) =>
                {
                    if (d != null && d is FrameworkElement element)
                    {
                        if ((bool)e.NewValue == true)
                        {
                            element.Focus();

                            if (d is System.Windows.Controls.Primitives.TextBoxBase control)
                            {
                                control.SelectAll();
                            }
                            else if (d is System.Windows.Controls.PasswordBox password)
                            {
                                password.SelectAll();
                            }
                        }
                    }
                })
                // CoerceValueCallback = new CoerceValueCallback((a, b) => { return null; })
            }
        );

        public static bool GetIsFocus(DependencyObject view)
        {
            return (bool)view.GetValue(IsFocusProperty);
        }

        public static void SetIsFocus(DependencyObject view, bool value)
        {
            view.SetValue(IsFocusProperty, value);
        }

        #endregion

        #region IsEnabled - 启用 IsFocus 附加属性

        // 附加属性 DependencyProperty.RegisterAttached
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached
        (
            name: "IsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(FocusSelectAll),
            defaultMetadata: new FrameworkPropertyMetadata()
            {
                DefaultValue = false,
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                PropertyChangedCallback = new PropertyChangedCallback(onHandle_IsEnabled_PropertyChanged)
                // CoerceValueCallback = new CoerceValueCallback((a, b) => { return null; })
            }
        );

        public static void SetIsEnabled(DependencyObject dp, bool value)
        {
            dp.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsEnabledProperty);
        }

        private static void onHandle_IsEnabled_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement visualElement)
            {
                if (e.OldValue is bool oldValue && oldValue == true)
                {
                    visualElement.GotFocus -= onHandleFocused;
                    visualElement.LostFocus -= onHandleUnfocused;
                }

                if (e.NewValue is bool newValue && newValue == true)
                {
                    visualElement.GotFocus += onHandleFocused;
                    visualElement.LostFocus += onHandleUnfocused;
                }
            }
        }


        private static void onHandleFocused(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                SetIsFocus(element, true);
            }
        }

        private static void onHandleUnfocused(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                SetIsFocus(element, false);
            }
        }

        #endregion
    }
}
