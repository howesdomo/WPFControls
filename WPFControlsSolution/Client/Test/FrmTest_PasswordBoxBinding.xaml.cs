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
using System.Windows.Shapes;

namespace Client.Test
{
    /// <summary>
    /// Interaction logic for FrmTest_PasswordBoxBinding.xaml
    /// </summary>
    public partial class FrmTest_PasswordBoxBinding : Window
    {
        public FrmTest_PasswordBoxBinding()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_PasswordBoxBinding_ViewModel : BaseViewModel
    {
        public FrmTest_PasswordBoxBinding_ViewModel()
        {
            this.CMD_Show = new Command((objWindow) =>
            {
                if (objWindow is FrmTest_PasswordBoxBinding owner)
                {
                    string msg = $"P1:{this.P1}\r\nP2:{this.P2}";
                    MessageBox.Show(owner, msg);
                }
            });
        }

        private string _P1;
        public string P1
        {
            get { return _P1; }
            set
            {
                _P1 = value;
                this.OnPropertyChanged("P1");
            }
        }


        private string _P2;
        public string P2
        {
            get { return _P2; }
            set
            {
                _P2 = value;
                this.OnPropertyChanged("P2");
            }
        }

        public Command CMD_Show { get; private set; }

    }
}

//namespace Client.ComponentsHelper
//{
//    /// <summary>
//    /// 为PasswordBox控件的Password增加绑定功能
//    /// </summary>
//    public static class PasswordBoxHelper
//    {
//        public static readonly DependencyProperty PasswordProperty =
//            DependencyProperty.RegisterAttached("Password",
//            typeof(string), typeof(PasswordBoxHelper),
//            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

//        public static readonly DependencyProperty AttachProperty =
//            DependencyProperty.RegisterAttached("Attach",
//            typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, Attach));

//        private static readonly DependencyProperty IsUpdatingProperty =
//           DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
//           typeof(PasswordBoxHelper));

//        public static void SetAttach(DependencyObject dp, bool value)
//        {
//            dp.SetValue(AttachProperty, value);
//        }

//        public static bool GetAttach(DependencyObject dp)
//        {
//            return (bool)dp.GetValue(AttachProperty);
//        }

//        public static string GetPassword(DependencyObject dp)
//        {
//            return (string)dp.GetValue(PasswordProperty);
//        }

//        public static void SetPassword(DependencyObject dp, string value)
//        {
//            dp.SetValue(PasswordProperty, value);
//        }

//        private static bool GetIsUpdating(DependencyObject dp)
//        {
//            return (bool)dp.GetValue(IsUpdatingProperty);
//        }

//        private static void SetIsUpdating(DependencyObject dp, bool value)
//        {
//            dp.SetValue(IsUpdatingProperty, value);
//        }

//        private static void OnPasswordPropertyChanged(DependencyObject sender,
//            DependencyPropertyChangedEventArgs e)
//        {
//            PasswordBox passwordBox = sender as PasswordBox;
//            passwordBox.PasswordChanged -= PasswordChanged;
//            if (!(bool)GetIsUpdating(passwordBox))
//            {
//                passwordBox.Password = (string)e.NewValue;
//            }
//            passwordBox.PasswordChanged += PasswordChanged;
//        }

//        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
//        {
//            if (sender is PasswordBox passwordBox)
//            {
//                if ((bool)e.OldValue)
//                {
//                    passwordBox.PasswordChanged -= PasswordChanged;
//                }
//                if ((bool)e.NewValue)
//                {
//                    passwordBox.PasswordChanged += PasswordChanged;
//                }
//            }
//        }

//        private static void PasswordChanged(object sender, RoutedEventArgs e)
//        {
//            PasswordBox passwordBox = sender as PasswordBox;
//            SetIsUpdating(passwordBox, true);
//            SetPassword(passwordBox, passwordBox.Password);
//            SetIsUpdating(passwordBox, false);
//        }
//    }
//}
