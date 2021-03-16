using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Components
{
    /// <summary>
    /// 此控件可以解决 PasswordBox 无法在 MVVM 中绑定 Password 属性的问题 ( 此绑定问题亦可以用附加属性解决, 详细见 FrmTest_PasswordBoxBingding )
    /// 
    /// V 1.0.0 - 2021-03-16 15:22:46
    /// 首次创建
    /// </summary>
    public sealed class BindablePasswordBox : UserControl
    {
        PasswordBox uiPasswordBox;

        public BindablePasswordBox()
        {
            InitializeComponent();
            this.initEvent();
        }

        private void InitializeComponent()
        {
            this.uiPasswordBox = new PasswordBox();
            this.Content = uiPasswordBox;
        }

        void initEvent()
        {
            this.uiPasswordBox.PasswordChanged += uiPasswordBox_PasswordChanged;
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register
        (
            name: "Password",
            propertyType: typeof(string),
            ownerType: typeof(BindablePasswordBox),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: string.Empty,
                propertyChangedCallback: onPassword_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private static void onPassword_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox target)
            {
                target.updatePassword();
            }
        }

        bool mIsChanging;

        void updatePassword()
        {
            if (mIsChanging == false)
            {
                uiPasswordBox.Password = this.Password;
            }
        }

        void uiPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.mIsChanging = true;
            this.Password = uiPasswordBox.Password;
            this.mIsChanging = false;
        }

    }
}
