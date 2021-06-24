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
    /// Interaction logic for FrmTest_MessageBox_Unblock.xaml
    /// </summary>
    public partial class FrmTest_MessageBox_Unblock : Window
    {
        public FrmTest_MessageBox_Unblock()
        {
            InitializeComponent();
            initEvent();
        }

        void initEvent()
        {
            this.btnShow.Click += BtnShow_Click;
            this.btnShowDialog.Click += BtnShowDialog_Click;

            this.btnShow_AutoCloseTimeSpan.Click += BtnShow_AutoCloseTimeSpan_Click;
            this.btnShowDialog_AutoCloseTimeSpan.Click += BtnShowDialog_AutoCloseTimeSpan_Click;

            this.btnStop.Click += btnStop_Click;
            this.Closed += FrmClosed;
        }

        private void FrmClosed(object sender, EventArgs e)
        {
            this.IsRunning = false;
        }

        System.ComponentModel.BackgroundWorker mBgWorker { get; set; }

        public bool IsRunning { get; set; }

        public Random Rand { get; set; } = new Random();

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                ucConsole.Add(new Util.Model.ConsoleData($"启动失败。\r\nmBgWorker != null && mBgWorker.IsBusy == true", Util.Model.ConsoleMsgType.ERROR));
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                while (IsRunning)
                {
                    try
                    {
                        execute();
                    }
                    catch (Exception ex)
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            ucConsole.Add(new Util.Model.ConsoleData(ex.GetInfo(), Util.Model.ConsoleMsgType.ERROR));
                            WPFControls.MessageBox.ShowError(owner: this, exception: ex);
                        }));
                    }

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ucConsole.Add(new Util.Model.ConsoleData("等待5秒继续执行execute", Util.Model.ConsoleMsgType.DEFAULT));
                    }));

                    for (int i = 0; i < 5; i++)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        if (this.IsRunning == false)
                        {
                            break;
                        }
                    }

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ucConsole.Add(new Util.Model.ConsoleData("等待完毕", Util.Model.ConsoleMsgType.INFO));
                    }));
                }
            };

            mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
            {
                ucConsole.Add(new Util.Model.ConsoleData("RunWorkerCompleted", Util.Model.ConsoleMsgType.DEFAULT));
            };

            IsRunning = true;
            mBgWorker.RunWorkerAsync(new object[] { });
        }

        private void BtnShowDialog_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                ucConsole.Add(new Util.Model.ConsoleData($"启动失败。\r\nmBgWorker != null && mBgWorker.IsBusy == true", Util.Model.ConsoleMsgType.ERROR));
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                while (IsRunning)
                {
                    try
                    {
                        execute();
                    }
                    catch (Exception ex)
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            ucConsole.Add(new Util.Model.ConsoleData(ex.GetInfo(), Util.Model.ConsoleMsgType.ERROR));
                            WPFControls.MessageBox.ShowErrorDialog(owner: this, exception: ex); // 程序被阻塞了
                        }));
                    }

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ucConsole.Add(new Util.Model.ConsoleData("等待5秒继续执行execute", Util.Model.ConsoleMsgType.DEFAULT));
                    }));

                    for (int i = 0; i < 5; i++)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        if (this.IsRunning == false)
                        {
                            break;
                        }
                    }

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ucConsole.Add(new Util.Model.ConsoleData("等待完毕", Util.Model.ConsoleMsgType.INFO));
                    }));
                }
            };

            mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
            {
                ucConsole.Add(new Util.Model.ConsoleData("RunWorkerCompleted", Util.Model.ConsoleMsgType.DEFAULT));
            };

            IsRunning = true;
            mBgWorker.RunWorkerAsync(new object[] { });
        }

        private void BtnShow_AutoCloseTimeSpan_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                ucConsole.Add(new Util.Model.ConsoleData($"启动失败。\r\nmBgWorker != null && mBgWorker.IsBusy == true", Util.Model.ConsoleMsgType.ERROR));
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                while (IsRunning)
                {
                    try
                    {
                        execute();
                    }
                    catch (Exception ex)
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            ucConsole.Add(new Util.Model.ConsoleData(ex.GetInfo(), Util.Model.ConsoleMsgType.ERROR));
                            WPFControls.MessageBox.ShowError(owner: this, exception: ex, autoCloseTimeSpan: TimeSpan.FromSeconds(4));
                        }));
                    }

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ucConsole.Add(new Util.Model.ConsoleData("等待5秒继续执行execute", Util.Model.ConsoleMsgType.DEFAULT));
                    }));

                    for (int i = 0; i < 5; i++)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        if (this.IsRunning == false)
                        {
                            break;
                        }
                    }

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ucConsole.Add(new Util.Model.ConsoleData("等待完毕", Util.Model.ConsoleMsgType.INFO));
                    }));
                }
            };

            mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
            {
                ucConsole.Add(new Util.Model.ConsoleData("RunWorkerCompleted", Util.Model.ConsoleMsgType.DEFAULT));
            };

            IsRunning = true;
            mBgWorker.RunWorkerAsync(new object[] { });
        }

        private void BtnShowDialog_AutoCloseTimeSpan_Click(object sender, RoutedEventArgs e)
        {
            if (mBgWorker != null && mBgWorker.IsBusy == true)
            {
                ucConsole.Add(new Util.Model.ConsoleData($"启动失败。\r\nmBgWorker != null && mBgWorker.IsBusy == true", Util.Model.ConsoleMsgType.ERROR));
                return;
            }

            mBgWorker = new System.ComponentModel.BackgroundWorker();
            mBgWorker.DoWork += (bgSender, bgArgs) =>
            {
                while (IsRunning)
                {
                    try
                    {
                        execute();
                    }
                    catch (Exception ex)
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            ucConsole.Add(new Util.Model.ConsoleData(ex.GetInfo(), Util.Model.ConsoleMsgType.ERROR));
                            // 加上了 autoCloseTimeSpan
                            WPFControls.MessageBox.ShowErrorDialog(owner: this, exception: ex, autoCloseTimeSpan: TimeSpan.FromSeconds(4)); // 程序阻塞                            
                        }));
                    }

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ucConsole.Add(new Util.Model.ConsoleData("等待5秒继续执行execute", Util.Model.ConsoleMsgType.DEFAULT));
                    }));

                    for (int i = 0; i < 5; i++)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        if (this.IsRunning == false)
                        {
                            break;
                        }
                    }

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ucConsole.Add(new Util.Model.ConsoleData("等待完毕", Util.Model.ConsoleMsgType.INFO));
                    }));
                }
            };

            mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
            {
                ucConsole.Add(new Util.Model.ConsoleData("RunWorkerCompleted", Util.Model.ConsoleMsgType.DEFAULT));
            };

            IsRunning = true;
            mBgWorker.RunWorkerAsync(new object[] { });
        }

        void execute()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                ucConsole.Add(new Util.Model.ConsoleData("开始执行", Util.Model.ConsoleMsgType.DEFAULT));
            }));

            System.Threading.Thread.Sleep(3000); // Do something
            int rand = Rand.Next(5);

            if (rand <= 3)
            {
                throw new BusinessException("抛出异常");
            }

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                ucConsole.Add(new Util.Model.ConsoleData("成功执行完毕", Util.Model.ConsoleMsgType.INFO));
            }));
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            IsRunning = false;
            ucConsole.Add(new Util.Model.ConsoleData("点击停止按钮", Util.Model.ConsoleMsgType.DEFAULT));
        }
    }
}
