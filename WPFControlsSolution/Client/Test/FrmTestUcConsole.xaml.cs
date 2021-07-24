using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// FrmTestUcConsole.xaml 的交互逻辑
    /// </summary>
    public partial class FrmTestUcConsole : Window, System.ComponentModel.INotifyPropertyChanged
    {
        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public FrmTestUcConsole()
        {
            // TODO 多次附加资源会报错, 如何在 XAML 中加入文件夹中的字体呢?
            if (Application.Current.Resources["MSYHMONO"] == null)
            {
                // 预先在XAML加载前 微软雅黑等宽字体
                var ttfPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Fonts", "MSYHMONO.ttf");
                Application.Current.Resources.Add("MSYHMONO", Util_Font.FontFamilyUtils.GetFontFamily_TypeOf_System_Windows_Media(ttfPath));
            }

            InitializeComponent();

            initEvent();
            initData();
        }

        private void initEvent()
        {
            btnAdd.Click += btnAdd_Click;
            btnClear.Click += BtnClear_Click;
        }

        void initData()
        {
            prepareUcConsoleData(this.ucConsole);
            prepareUcConsoleData(this.ucConsoleQueue);
        }

        void prepareUcConsoleData(Client.Components.UcConsole uc)
        {
            uc.Add(new Util.Model.ConsoleData
                (
                    consoleMsgType: Util.Model.ConsoleMsgType.DEFAULT,
                    content: $"测试ABC",
                    entryTime: DateTime.Now
                ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.DEBUG,
                content: $"测试ABC",
                entryTime: DateTime.Now
            ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.INFO,
                content: $"测试IJK（测试等宽字体）",
                entryTime: DateTime.Now
            ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.WARNING,
                content: $"测试ABCD（两个英文等于一个汉字）\r\n一二测试DEF（前面4个空格）\r\n测试多行",
                entryTime: DateTime.Now
            ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.ERROR,
                content: $"测试宽字符HQMW001",
                entryTime: DateTime.Now
            ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.BUSINESSERROR,
                content: $"测试窄字符ICXK001",
                entryTime: DateTime.Now
            ));
        }

        void prepareUcConsoleData(Client.Components.UcConsoleQueue uc)
        {
            uc.Add(new Util.Model.ConsoleData
                (
                    consoleMsgType: Util.Model.ConsoleMsgType.DEFAULT,
                    content: $"测试ABC",
                    entryTime: DateTime.Now
                ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.DEBUG,
                content: $"测试ABC",
                entryTime: DateTime.Now
            ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.INFO,
                content: $"测试IJK（测试等宽字体）",
                entryTime: DateTime.Now
            ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.WARNING,
                content: $"测试ABCD（两个英文等于一个汉字）\r\n一二测试DEF（前面4个空格）\r\n测试多行",
                entryTime: DateTime.Now
            ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.ERROR,
                content: $"测试宽字符HQMW001",
                entryTime: DateTime.Now
            ));

            uc.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: Util.Model.ConsoleMsgType.BUSINESSERROR,
                content: $"测试窄字符ICXK001",
                entryTime: DateTime.Now
            ));
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string msg = this.txtLog.Text;
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            string msgTypeStr = this.txtConsoleMsgType.Text;
            int consoleMsgType = 0;
            int.TryParse(msgTypeStr, out consoleMsgType);

            ucConsole.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: (Util.Model.ConsoleMsgType)consoleMsgType,
                content: $"{msg}\r\n{msg}\r\n{msg}",
                entryTime: DateTime.Now
            ));

            ucConsoleQueue.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: (Util.Model.ConsoleMsgType)consoleMsgType,
                content: $"{msg}\r\n{msg}\r\n{msg}",
                entryTime: DateTime.Now
            ));
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ucConsole.Clear();
            ucConsoleQueue.Clear();
        }
    }

}
