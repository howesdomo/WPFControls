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
    /// Interaction logic for CcPassword.xaml
    /// </summary>
    public partial class CcPassword : ContentControl
    {
        System.ComponentModel.BackgroundWorker mBgWorker { get; set; }


        private ViewModels.AccountPasswordViewModel _ViewModel;
        public ViewModels.AccountPasswordViewModel ViewModel
        {
            get { return _ViewModel; }
            private set { _ViewModel = value; }
        }

        public CcPassword(ViewModels.AccountPasswordViewModel vm)
        {
            InitializeComponent();

            vm.LoginAccount = "Not need input LoginAccount"; // 本界面只需输入密码
            this.DataContext = this.ViewModel = vm;

            this.Loaded += (s, e) =>
            {
                // CcSingleTextBox 需要对预设值进行 Foucs 与 SelectAll
                // 在 Loaded 事件，TextBox.Text 仍然是空值，故无法对全选 TextBox

                // 权宜之计 采用 BackgroundWorker 等待 100 毫秒
                if (mBgWorker != null && mBgWorker.IsBusy == true)
                {
                    return;
                }

                mBgWorker = new System.ComponentModel.BackgroundWorker();
                mBgWorker.DoWork += (bgSender, bgArgs) =>
                {
                    System.Threading.Thread.Sleep(100);
                };

                mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
                {
                    txtPassword.Focus();
                    txtPassword.SelectAll();
                };

                mBgWorker.RunWorkerAsync(new object[] { });
            };
        }

    }
}