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
            InitializeComponent();

            this.DataContext = this;

            this.ConsoleList = new System.Collections.ObjectModel.ObservableCollection<ConsoleData>();
            ConsoleList.Add(new ConsoleData("测试"));
            ConsoleList.Add(new ConsoleData("异常321", ConsoleMsgType.Error));
            ConsoleList.Add(new ConsoleData("正常123", ConsoleMsgType.Info));

            this.ConsoleList.CollectionChanged += ConsoleList_CollectionChanged;
        }

        private void ConsoleList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("ConsoleList");
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string msg = this.txtLog.Text;
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            ucConsoleAddInfo();
        }

        private void ucConsoleAddInfo()
        {
            ConsoleList.Add(new ConsoleData("测试"));
            ConsoleList.Add(new ConsoleData("异常321", ConsoleMsgType.Error));
            ConsoleList.Add(new ConsoleData("正常123", ConsoleMsgType.Info));

            ucConsole.ScrollIntoView(ConsoleList[ConsoleList.Count - 1]);
        }

        public System.Collections.ObjectModel.ObservableCollection<ConsoleData> ConsoleList { get; set; }
    }

    public class ConsoleData
    {
        public ConsoleData(string content)
        {
            this.Content = content;
            this.ConsoleMsgType = 0;
            this.EntryTime = DateTime.Now;
        }

        public ConsoleData(string content, ConsoleMsgType consoleMsgType)
        {
            this.Content = content;
            this.ConsoleMsgType = consoleMsgType;
            this.EntryTime = DateTime.Now;
        }

        public ConsoleData(string content, DateTime entryTime)
        {
            this.Content = content;
            this.ConsoleMsgType = 0;
            this.EntryTime = entryTime;

            // ConsoleData(content, 0, entryTime);
        }

        public ConsoleData(string content, ConsoleMsgType consoleMsgType, DateTime entryTime)
        {
            this.Content = content;
            this.ConsoleMsgType = consoleMsgType;
            this.EntryTime = entryTime;
            getForeground();
        }

        private void getForeground()
        {
            switch (ConsoleMsgType)
            {
                case ConsoleMsgType.Info:
                    Foreground = Colors.Green;
                    break;
                case ConsoleMsgType.Question:
                    Foreground = Colors.Purple;
                    break;
                case ConsoleMsgType.Warning:
                    Foreground = Colors.Orange;
                    break;
                case ConsoleMsgType.Error:
                    Foreground = Colors.Red;
                    break;
            }
        }

        public ConsoleMsgType ConsoleMsgType { get; set; }

        public string Content { get; set; }

        public DateTime EntryTime { get; set; }

        public Color Foreground { get; set; }


    }

    public enum ConsoleMsgType
    {
        Info = 0,
        Question = 1,
        Warning = 2,
        Error = 3
    }
}
