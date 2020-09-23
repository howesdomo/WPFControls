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
    /// Interaction logic for FrmTestBusyIndicatior.xaml
    /// </summary>
    public partial class FrmTestBusyIndicatior : Window
    {
        public FrmTestBusyIndicatior()
        {
            InitializeComponent();
        }

        System.ComponentModel.BackgroundWorker mBgWorker { get; set; }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                System.Threading.Thread.Sleep(2000);
            };

            mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
            {
                ucWait.IsBusy = false;
                if (bgResult.Error != null)
                {
                    string msg = $"{bgResult.Error.GetFullInfo()}";
                    System.Diagnostics.Debug.WriteLine(msg);
                }
                else
                {

                }
            };

            mBgWorker.WorkerReportsProgress = true;
            mBgWorker.ProgressChanged += (bgSender, bgArgs) =>
            {

            };

            ucWait.IsBusy = true;
            // ucWait.BusyContent = "loading"; // 普通搜索, 无需修改 BusyContent 内容
            mBgWorker.RunWorkerAsync(new object[] { });
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                System.Threading.Thread.Sleep(2000);
            };

            mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
            {
                ucWait.IsBusy = false;
                if (bgResult.Error != null)
                {
                    string msg = $"{bgResult.Error.GetFullInfo()}";
                    System.Diagnostics.Debug.WriteLine(msg);
                }
                else
                {

                }
            };

            mBgWorker.WorkerReportsProgress = true;
            mBgWorker.ProgressChanged += (bgSender, bgArgs) =>
            {

            };

            ucWait.IsBusy = true;
            ucWait.BusyContent = "正在打印, 请稍候..."; // 修改 BusyContent 为指定的内容显示在屏幕上
            mBgWorker.RunWorkerAsync(new object[] { });
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                System.Threading.Thread.Sleep(2000);
            };

            mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
            {
                ucWait1.IsBusy = false;
                if (bgResult.Error != null)
                {
                    string msg = $"{bgResult.Error.GetFullInfo()}";
                    System.Diagnostics.Debug.WriteLine(msg);
                }
                else
                {

                }
            };

            mBgWorker.WorkerReportsProgress = true;
            mBgWorker.ProgressChanged += (bgSender, bgArgs) =>
            {

            };

            ucWait1.IsBusy = true;
            // 在 XAML中已设置 IsResetBusyContentPerExecute = false
            // BusyContent 不会每次执行完毕后进行重置
            mBgWorker.RunWorkerAsync(new object[] { });
        }
    }
}
