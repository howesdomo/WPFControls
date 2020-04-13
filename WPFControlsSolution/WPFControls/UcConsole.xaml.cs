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

namespace Client.Components
{
    /// <summary>
    /// UcConsole.xaml 的交互逻辑
    /// </summary>
    public partial class UcConsole : UserControl, System.ComponentModel.INotifyPropertyChanged
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

        public System.Collections.ObjectModel.ObservableCollection<dynamic> ConsoleList { get; set; }

        public UcConsole()
        {
            InitializeComponent();
            this.DataContext = this;
            this.ConsoleList = new System.Collections.ObjectModel.ObservableCollection<dynamic>();
            this.ConsoleList.CollectionChanged += ConsoleList_CollectionChanged;
        }

        private void ConsoleList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("ConsoleList");
        }

        public EventHandler<ABCEventArgs> AddConsole;

        public void Add(Util.Model.ConsoleData d)
        {
            this.ConsoleList.Add(d);

            int lastIndex = this.ConsoleList.Count - 1;
            ucConsole.ScrollIntoView(this.ConsoleList[lastIndex]);
            ucConsole.SelectedItem = this.ConsoleList[lastIndex];
        }

        public void Clear()
        {
            this.ConsoleList.Clear();
        }

        #region 复制

        private void menuItemCopy(object sender, RoutedEventArgs e)
        {
            if (ucConsole.SelectedItems == null || ucConsole.SelectedItems.Count <= 0)
            {
                return;
            }

            copyActualMethod(ucConsole.SelectedItems);
        }

        private void menuItemAllCopy(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyActualMethod(ucConsole.Items);
        }

        private void copyActualMethod(IList list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss");
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
            if (ucConsole.SelectedItems == null || ucConsole.SelectedItems.Count <= 0)
            {
                return;
            }

            copyMsgActualMethod(ucConsole.SelectedItems);
        }

        private void menuItemAllCopyMsg(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyMsgActualMethod(ucConsole.Items);
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
            if (ucConsole.SelectedItems == null || ucConsole.SelectedItems.Count <= 0)
            {
                return;
            }

            copyCSVActualMethod(ucConsole.SelectedItems);
        }

        private void menuItemAllCopyCSV(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyCSVActualMethod(ucConsole.Items);
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
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss");
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
            if (ucConsole.SelectedItems == null || ucConsole.SelectedItems.Count <= 0)
            {
                return;
            }

            copyJSONActualMethod(ucConsole.SelectedItems);
        }

        private void menuItemAllCopyJson(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyJSONActualMethod(ucConsole.Items);
        }

        private void copyJSONActualMethod(IList list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss");
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
            if (ucConsole.SelectedItems == null || ucConsole.SelectedItems.Count <= 0)
            {
                return;
            }

            var dr = MessageBox.Show("确认清理[选中]项？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                List<int> toDelete = new List<int>();

                foreach (var item in ucConsole.SelectedItems)
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

                foreach (var item in ucConsole.SelectedItems)
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
    }

    public class ABCEventArgs : EventArgs
    {

    }

    public static class UcConsoleBehaviour
    {
        public static readonly DependencyProperty AutoCopyProperty = DependencyProperty.RegisterAttached("AutoCopy",
            typeof(bool), typeof(UcConsoleBehaviour), new UIPropertyMetadata(AutoCopyChanged));

        public static bool GetAutoCopy(DependencyObject obj_)
        {
            return (bool)obj_.GetValue(AutoCopyProperty);
        }

        public static void SetAutoCopy(DependencyObject obj_, bool value_)
        {
            obj_.SetValue(AutoCopyProperty, value_);
        }

        private static void AutoCopyChanged(DependencyObject obj_, DependencyPropertyChangedEventArgs e_)
        {
            var listBox = obj_ as ListBox;
            if (listBox != null)
            {
                if ((bool)e_.NewValue)
                {
                    ExecutedRoutedEventHandler handler =
                        (sender_, arg_) =>
                        {
                            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
                            {
                                return;
                            }

                            StringBuilder sb = new StringBuilder();

                            foreach (object obj in listBox.SelectedItems)
                            {
                                if (obj is Util.Model.ConsoleData)
                                {
                                    var item = obj as Util.Model.ConsoleData;
                                    sb.AppendLine(item.Content);
                                }
                            }

                            var msg = sb.ToString();
                            msg = msg.Substring(0, msg.Length - 2);

                            Clipboard.SetText(msg);
                        };

                    var command = new RoutedCommand("Copy", typeof(ListBox));
                    command.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Control, "Copy"));
                    listBox.CommandBindings.Add(new CommandBinding(command, handler));
                }
            }
        }
    }
}
