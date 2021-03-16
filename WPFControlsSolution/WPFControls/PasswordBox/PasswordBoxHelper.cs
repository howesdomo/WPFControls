using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Client.Components
{
    /// <summary>
    /// 用附加属性的方式, 使 PasswordBox 能在 MVVM 中绑定 string
    /// 
    /// V 1.0.0 - 2021-03-16 17:05:40
    /// 首次创建
    /// </summary>
    public static class PasswordBoxHelper
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
                ownerType: typeof(PasswordBoxHelper),
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


        #region MyRegion        

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached
            (
                name: "Attach",
                propertyType: typeof(bool),
                ownerType: typeof(PasswordBoxHelper),
                defaultMetadata: new FrameworkPropertyMetadata
                (
                    defaultValue: false,
                    propertyChangedCallback: onHandle_Attach_PropertyChanged
                )
            );


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        private static void onHandle_Attach_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
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
                ownerType: typeof(PasswordBoxHelper)
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
