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

namespace WPFControls.MessageBoxExtraContent
{
    /// <summary>
    /// Interaction logic for CcAccountPassword.xaml
    /// </summary>
    public partial class CcAccountPassword : ContentControl
    {
        private ViewModels.AccountPasswordViewModel _ViewModel;
        public ViewModels.AccountPasswordViewModel ViewModel
        {
            get { return _ViewModel; }
            private set { _ViewModel = value; }
        }

        public CcAccountPassword(ViewModels.AccountPasswordViewModel vm)
        {
            InitializeComponent();
            this.DataContext = this.ViewModel = vm;

            this.Loaded += (s,e)=> 
            {
                // CcSingleTextBox 需要对预设值进行 Foucs 与 SelectAll
                // 在 Loaded 事件，TextBox.Text 仍然是空值，故无法对全选 TextBox
                // 但 账号密码 这个控件无需进行全选操作

                var dataContext = this.DataContext as ViewModels.AccountPasswordViewModel;

                if (string.IsNullOrEmpty(dataContext.LoginAccount))
                {
                    txtLoginAccount.Focus();
                }
                else
                {
                    txtPassword.Focus();
                }
            };            
        }
    }
}
