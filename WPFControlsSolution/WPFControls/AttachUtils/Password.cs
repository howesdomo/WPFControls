using System;
using System.Windows;
using System.Windows.Controls;

namespace Client.Controls.AttachUtils
{
    /// <summary>
    /// V 1.0.1 - 2021-05-27 15:23:14
    /// 从 PasswordBox 迁移到本位置，
    /// 修改 Attach ==> IsEnabled, 统一整个 AttachUtils 的启用方式
    /// 
    /// V 1.0.0 - 2021-03-16 17:05:40
    /// 首次创建
    /// 用附加属性的方式, 使 PasswordBox 能在 MVVM 中绑定 string
    /// </summary>
    public class Password
    {
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }

        #region [DPA] Password

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached
            (
                name: "Password",
                propertyType: typeof(string),
                ownerType: typeof(Password),
                defaultMetadata: new FrameworkPropertyMetadata
                (
                    defaultValue: string.Empty,
                    propertyChangedCallback: onHandle_Password_PropertyChanged
                )
            );

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }


        private static void onHandle_Password_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;
            if (!(bool)GetIsUpdating(passwordBox)) // IsUpdating -- 正在输入, 若果正在输入中, 不会更新实际 PasswordBox 的值
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        #endregion

        #region [DPA] IsEnabled        

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached
            (
                name: "IsEnabled",
                propertyType: typeof(bool),
                ownerType: typeof(Password),
                defaultMetadata: new FrameworkPropertyMetadata
                (
                    defaultValue: false,
                    propertyChangedCallback: onHandle_IsEnabled_PropertyChanged
                )
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
            if (sender is PasswordBox passwordBox)
            {
                if ((bool)e.OldValue)
                {
                    passwordBox.PasswordChanged -= PasswordChanged;
                }
                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordChanged;
                }
            }
        }

        #endregion

        #region [DPA] IsUpdating 

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached
            (
                name: "IsUpdating",
                propertyType: typeof(bool),
                ownerType: typeof(Password)
            );

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        #endregion

    }
}
