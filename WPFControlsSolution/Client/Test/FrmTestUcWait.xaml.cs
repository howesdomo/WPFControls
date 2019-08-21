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
    /// FrmTestUcWait.xaml 的交互逻辑
    /// </summary>
    public partial class FrmTestUcWait : Window
    {
        public FrmTestUcWait()
        {
            InitializeComponent();
            initEvent();
        }

        private void initEvent()
        {
            this.btnBuild.Click += BtnWait_Click;
            this.btnBuildWithAbis.Click += BtnWait_Click;

            this.btnClean.Click += BtnWait_ConsolePart_Click;
            this.btnReBuild.Click += BtnWait_ConsolePart_Click;
            this.btnInstall.Click += BtnWait_ConsolePart_Click;
        }

        System.ComponentModel.BackgroundWorker mBgWorker { get; set; }

        private void BtnWait_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                System.Threading.Thread.Sleep(5000);
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
            ucWait.BusyContent = "loading"; // 修改 BusyContent 为指定的内容显示在屏幕上
            mBgWorker.RunWorkerAsync(new object[] { });
        }

        private void BtnWait_ConsolePart_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                System.Threading.Thread.Sleep(5000);
            };

            mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
            {
                ucWait_Console.IsBusy = false;
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

            ucWait_Console.IsBusy = true;
            mBgWorker.RunWorkerAsync(new object[] { });
        }
    }
}
