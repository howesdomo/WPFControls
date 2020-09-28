using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

/// <summary>
/// V 1.0.1 - 2020-9-23 14:38:01
/// 1. 修正 UI 字眼
/// 2. 导出时间信息精确到最后毫秒位数
/// 3. 优化代码结构
/// </summary>
namespace Client.Components
{
    /// <summary>
    /// UcConsole.xaml 的交互逻辑
    /// </summary>
    public partial class UcConsole : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO 创建 FilterBar, 可以根据信息类型 / 信息内容 对 ConsoleList 进行过滤

        public System.Collections.ObjectModel.ObservableCollection<dynamic> ConsoleList { get; set; }

        public UcConsole()
        {
            InitializeComponent();
            initEvent();

            this.DataContext = this;
            this.ConsoleList = new System.Collections.ObjectModel.ObservableCollection<dynamic>();
            this.ConsoleList.CollectionChanged += mConsoleList_CollectionChanged;
        }

        void initEvent()
        {
            listBox_Ctrl_C_Copy();
        }

        /// <summary>
        /// 由于 ListBox 选中一或多项后, 按 Ctrl + C 并不会复制数据到系统剪贴板
        /// 故本方法为其实现复制数据功能
        /// </summary>
        void listBox_Ctrl_C_Copy() 
        {
            ExecutedRoutedEventHandler handler = (sender, args) =>
            {
                if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
                {
                    return;
                }

                copyJSONActualMethod(listBox.SelectedItems);
            };

            var command = new RoutedCommand("Copy", typeof(ListBox));
            command.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Control, "Copy"));
            listBox.CommandBindings.Add(new CommandBinding(command, handler));
        }

        private void mConsoleList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("ConsoleList");
        }
       
        public void Add(Util.Model.ConsoleData d)
        {
            this.ConsoleList.Add(d);

            int lastIndex = this.ConsoleList.Count - 1;
            listBox.ScrollIntoView(this.ConsoleList[lastIndex]);
            listBox.SelectedItem = this.ConsoleList[lastIndex];
        }

        public void Clear()
        {
            this.ConsoleList.Clear();
        }

        #region 复制

        private void menuItemCopy(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            copyActualMethod(listBox.SelectedItems);
        }

        private void menuItemAllCopy(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyActualMethod(listBox.Items);
        }

        private void copyActualMethod(IList list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    if (menuItem_OutputNewLineSymbol.IsChecked == false)
                    {
                        sb.AppendLine($"{dtMsg}\t[{item.ConsoleMsgType}]\t{item.Content}");
                    }
                    else
                    {
                        sb.AppendLine($"{dtMsg}\t[{item.ConsoleMsgType}]\t{item.Content.Replace("\r", "\\r").Replace("\n", "\\n")}");
                    }
                }
            }

            var msg = sb.ToString();
            msg = msg.Substring(0, msg.Length - 2);

            Clipboard.SetText(msg);
        }

        #endregion

        #region 复制内容

        private void menuItemCopyMsg(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            copyMsgActualMethod(listBox.SelectedItems);
        }

        private void menuItemAllCopyMsg(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyMsgActualMethod(listBox.Items);
        }

        private void copyMsgActualMethod(IList list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    if (menuItem_OutputNewLineSymbol.IsChecked == false)
                    {
                        sb.AppendLine(item.Content);
                    }
                    else 
                    {
                        sb.AppendLine(item.Content.Replace("\r", "\\r").Replace("\n", "\\n"));
                    }
                }
            }

            var msg = sb.ToString();
            msg = msg.Substring(0, msg.Length - 2);

            Clipboard.SetText(msg);
        }

        #endregion

        #region 复制CSV

        private void menuItemCopyCSV(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            copyCSVActualMethod(listBox.SelectedItems);
        }

        private void menuItemAllCopyCSV(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyCSVActualMethod(listBox.Items);
        }

        private void copyCSVActualMethod(IList list)
        {
            string separator = menuItem_CSVSeparator.Text;

            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    if (menuItem_OutputNewLineSymbol.IsChecked == false)
                    {
                        sb.AppendLine($"{dtMsg}{separator}[{item.ConsoleMsgType}]{separator}{item.Content}");
                    }
                    else 
                    {
                        sb.AppendLine($"{dtMsg}{separator}[{item.ConsoleMsgType}]{separator}{item.Content.Replace("\r", "\\r").Replace("\n", "\\n")}");
                    }
                }
            }

            var msg = sb.ToString();
            msg = msg.Substring(0, msg.Length - 2);

            Clipboard.SetText(msg);
        }

        #endregion

        #region 复制JSON

        private void menuItemCopyJson(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            copyJSONActualMethod(listBox.SelectedItems);
        }

        private void menuItemAllCopyJson(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyJSONActualMethod(listBox.Items);
        }

        private void copyJSONActualMethod(IList list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    var content = item.Content.Replace("\r", "\\r").Replace("\n", "\\n");
                    sb.Append("{");
                    sb.Append($"\"EntryTime\":\"{dtMsg}\", \"ConsleMsgType\":\"{item.ConsoleMsgType}\", \"Content\":\"{content}\"");
                    sb.Append("},");
                    sb.AppendLine();
                }
            }

            var msg = sb.ToString();
            msg = msg.Substring(0, msg.Length - 3);
            msg = $"[{msg}]";

            Clipboard.SetDataObject(msg);
        }

        #endregion

        private void menuItemClearSelected(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            var dr = MessageBox.Show("确认清理[选中]项？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                List<int> toDelete = new List<int>();

                foreach (var item in listBox.SelectedItems)
                {
                    toDelete.Add(this.ConsoleList.IndexOf(item));
                }

                foreach (int index in toDelete.OrderByDescending(i => i))
                {
                    this.ConsoleList.RemoveAt(index);
                }
            }
        }

        private void menuItemClearUnSelected(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("确认清理[未选中]项？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                List<int> toRemain = new List<int>();
                List<int> toDelete = new List<int>();

                int total = this.ConsoleList.Count;

                foreach (var item in listBox.SelectedItems)
                {
                    toRemain.Add(this.ConsoleList.IndexOf(item));
                }

                for (int index = total - 1; index > -1; index--)
                {
                    if (toRemain.Contains(index) == false)
                    {
                        toDelete.Add(index);
                    }
                }

                foreach (int index in toDelete)
                {
                    this.ConsoleList.RemoveAt(index);
                }
            }
        }

        private void menuItemClearAll(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("确认清理[全部]？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                Clear();
            }
        }

        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
