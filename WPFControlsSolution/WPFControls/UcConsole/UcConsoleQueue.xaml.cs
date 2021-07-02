using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for UcConsoleQueue.xaml
    /// </summary>
    public partial class UcConsoleQueue : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        #region [DP] ContentFontFamily

        public static readonly DependencyProperty ContentFontFamilyProperty = DependencyProperty.Register
        (
            name: "ContentFontFamily",
            propertyType: typeof(System.Windows.Media.FontFamily),
            ownerType: typeof(UcConsoleQueue),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: new System.Windows.Media.FontFamily(),
                propertyChangedCallback: null, //onContentFontFamily_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.FontFamily ContentFontFamily
        {
            get { return (System.Windows.Media.FontFamily)GetValue(ContentFontFamilyProperty); }
            set { SetValue(ContentFontFamilyProperty, value); }
        }

        #endregion

        #region [DP] AutoScroolToBottom_DiffValue

        public static readonly DependencyProperty AutoScroolToBottom_DiffValueProperty = DependencyProperty.Register
        (
            name: "AutoScroolToBottom_DiffValue",
            propertyType: typeof(double),
            ownerType: typeof(UcConsoleQueue),
            validateValueCallback: new ValidateValueCallback((toValidate) =>
            {
                if (toValidate is null) { return false; }

                return double.TryParse(toValidate.ToString(), out double temp);
            }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Client.Controls.AttachUtils.AutoScrollToBottom._DiffValue_Default_Value_,
                propertyChangedCallback: null, // onAutoScroolToBottom_DiffValue_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public double AutoScroolToBottom_DiffValue
        {
            get { return (double)GetValue(AutoScroolToBottom_DiffValueProperty); }
            set { SetValue(AutoScroolToBottom_DiffValueProperty, value); }
        }

        #endregion

        #region [DP] QueueMaxCapacity

        public static readonly DependencyProperty QueueMaxCapacityProperty = DependencyProperty.Register
        (
            name: "QueueMaxCapacity",
            propertyType: typeof(int),
            ownerType: typeof(UcConsoleQueue),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 1000,
                propertyChangedCallback: onQueueMaxCapacity_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public int QueueMaxCapacity
        {
            get { return (int)GetValue(QueueMaxCapacityProperty); }
            set { SetValue(QueueMaxCapacityProperty, value); }
        }

        public static void onQueueMaxCapacity_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UcConsoleQueue target)
            {
                if (e.NewValue == null)
                {
                    target.ConsoleQueue.QueueMaxCapacity = null;
                }
                else if (int.TryParse(e.NewValue.ToString(), out int t))
                {
                    target.ConsoleQueue.QueueMaxCapacity = t;
                }
            }
        }

        #endregion


        public ObservableQueue<dynamic> ConsoleQueue { get; set; }

        public UcConsoleQueue()
        {
            InitializeComponent();
            initEvent();

            this.DataContext = this;
            this.ConsoleQueue = new ObservableQueue<dynamic>(capacity: 1, maxCapacity: 1);
            this.ConsoleQueue.CollectionChanged += mConsoleQueue_CollectionChanged;
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

        private void mConsoleQueue_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(ConsoleQueue));
        }

        public void Add(Util.Model.ConsoleData d)
        {
            if (HasFilterConditions)
            {
                if (this.SelectedConsoleMsgType != Util.Model.ConsoleMsgType.NONE)
                {
                    if (d.ConsoleMsgType != SelectedConsoleMsgType)
                    {
                        return;
                    }
                }

                if (this.SelectedRegexCondition != Util.Model.ContentTextFilterCondition.None && string.IsNullOrEmpty(this.RegexPattern) == false)
                {
                    switch (this.SelectedRegexCondition)
                    {
                        case Util.Model.ContentTextFilterCondition.Equals:
                            if (System.Text.RegularExpressions.Regex.IsMatch(input: d.Content, pattern: this.RegexPattern) == false)
                            {
                                return;
                            }
                            break;
                        case Util.Model.ContentTextFilterCondition.NotEquals:
                            if (System.Text.RegularExpressions.Regex.IsMatch(input: d.Content, pattern: this.RegexPattern) == true)
                            {
                                return;
                            }
                            break;
                        default:
                            throw new ArgumentException("不属于给定的条件");
                    }
                }

                if (this.SelectedTextCondition != Util.Model.ContentTextFilterCondition.None && string.IsNullOrEmpty(this.TextPattern) == false)
                {
                    switch (this.SelectedTextCondition)
                    {
                        case Util.Model.ContentTextFilterCondition.StartsWith:
                            if (d.Content.StartsWith(this.TextPattern) == false) { return; }
                            break;
                        case Util.Model.ContentTextFilterCondition.EndsWith:
                            if (d.Content.EndsWith(this.TextPattern) == false) { return; }
                            break;
                        case Util.Model.ContentTextFilterCondition.Contains:
                            if (d.Content.Contains(this.TextPattern) == false) { return; }
                            break;
                        case Util.Model.ContentTextFilterCondition.Equals:
                            if (d.Content.Equals(this.TextPattern) == false) { return; }
                            break;
                        case Util.Model.ContentTextFilterCondition.NotEquals:
                            if (d.Content.Equals(this.TextPattern) == true) { return; }
                            break;
                    }
                }
            }

            this.ConsoleQueue.Enqueue(d);
        }

        public void Clear()
        {
            this.ConsoleQueue.Clear();
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
            if (ConsoleQueue == null || ConsoleQueue.Count <= 0)
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
            if (ConsoleQueue == null || ConsoleQueue.Count <= 0)
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
            if (ConsoleQueue == null || ConsoleQueue.Count <= 0)
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
            if (ConsoleQueue == null || ConsoleQueue.Count <= 0)
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

        #region [弃用] Queue 无法轻易地获取选中或未选中项, 暂不实现下列功能

        //private void menuItemClearSelected(object sender, RoutedEventArgs e)
        //{
        //    if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
        //    {
        //        return;
        //    }

        //    var dr = MessageBox.Show("确认清理[选中]项？", "提示", MessageBoxButton.YesNo);
        //    if (dr == MessageBoxResult.Yes)
        //    {
        //        List<int> toDelete = new List<int>();

        //        foreach (var item in listBox.SelectedItems)
        //        {
        //            toDelete.Add(this.ConsoleList.IndexOf(item));
        //        }

        //        foreach (int index in toDelete.OrderByDescending(i => i))
        //        {
        //            this.ConsoleList.RemoveAt(index);
        //        }
        //    }
        //}

        //private void menuItemClearUnSelected(object sender, RoutedEventArgs e)
        //{
        //    var dr = MessageBox.Show("确认清理[未选中]项？", "提示", MessageBoxButton.YesNo);
        //    if (dr == MessageBoxResult.Yes)
        //    {
        //        List<int> toRemain = new List<int>();
        //        List<int> toDelete = new List<int>();

        //        int total = this.ConsoleList.Count;

        //        foreach (var item in listBox.SelectedItems)
        //        {
        //            toRemain.Add(this.ConsoleList.IndexOf(item));
        //        }

        //        for (int index = total - 1; index > -1; index--)
        //        {
        //            if (toRemain.Contains(index) == false)
        //            {
        //                toDelete.Add(index);
        //            }
        //        }

        //        foreach (int index in toDelete)
        //        {
        //            this.ConsoleList.RemoveAt(index);
        //        }
        //    }
        //}

        #endregion

        private void menuItemClearAll(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("确认清理[全部]？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                Clear();
            }
        }

        #region 过滤搜索条件 Advance 新增特性


        public static readonly DependencyProperty FilterConditionsVisibilityProperty = DependencyProperty.Register
        (
            name: "FilterConditionsVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcConsoleQueue),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Collapsed,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public Visibility FilterConditionsVisibility
        {
            get { return (Visibility)GetValue(FilterConditionsVisibilityProperty); }
            set { SetValue(FilterConditionsVisibilityProperty, value); }
        }

        public List<Util.Model.ConsoleMsgType> ConsoleMsgTypeList { get; set; } = new List<Util.Model.ConsoleMsgType>()
        {
            Util.Model.ConsoleMsgType.NONE,
            Util.Model.ConsoleMsgType.DEFAULT,
            Util.Model.ConsoleMsgType.DEBUG ,
            Util.Model.ConsoleMsgType.INFO,
            Util.Model.ConsoleMsgType.WARNING ,
            Util.Model.ConsoleMsgType.ERROR ,
            Util.Model.ConsoleMsgType.BUSINESSERROR
        };

        void filterConditionNotify()
        {
            this.OnPropertyChanged(nameof(HasFilterConditions));
            this.OnPropertyChanged(nameof(HasFilterConditionsInfo));
        }

        private Util.Model.ConsoleMsgType _SelectedConsoleMsgType = Util.Model.ConsoleMsgType.NONE;
        public Util.Model.ConsoleMsgType SelectedConsoleMsgType
        {
            get { return _SelectedConsoleMsgType; }
            set
            {
                _SelectedConsoleMsgType = value;
                this.OnPropertyChanged(nameof(SelectedConsoleMsgType));

                filterConditionNotify();
            }
        }

        public List<Util.Model.ContentTextFilterCondition> RegexConditionList { get; set; } = new List<Util.Model.ContentTextFilterCondition>()
        {
            Util.Model.ContentTextFilterCondition.None,
            Util.Model.ContentTextFilterCondition.Equals,
            Util.Model.ContentTextFilterCondition.NotEquals,
        };

        private Util.Model.ContentTextFilterCondition _SelectedRegexCondition = Util.Model.ContentTextFilterCondition.None;
        public Util.Model.ContentTextFilterCondition SelectedRegexCondition
        {
            get { return _SelectedRegexCondition; }
            set
            {
                _SelectedRegexCondition = value;
                this.OnPropertyChanged(nameof(SelectedRegexCondition));

                filterConditionNotify();
            }
        }


        private string _RegexPattern;
        public string RegexPattern
        {
            get { return _RegexPattern; }
            set
            {
                _RegexPattern = value;
                this.OnPropertyChanged(nameof(RegexPattern));

                filterConditionNotify();
            }
        }

        public List<Util.Model.ContentTextFilterCondition> TextConditionList { get; set; } = new List<Util.Model.ContentTextFilterCondition>()
        {

            Util.Model.ContentTextFilterCondition.None,
            Util.Model.ContentTextFilterCondition.StartsWith,
            Util.Model.ContentTextFilterCondition.EndsWith,
            Util.Model.ContentTextFilterCondition.Contains,
            Util.Model.ContentTextFilterCondition.Equals,
            Util.Model.ContentTextFilterCondition.NotEquals,
        };

        private Util.Model.ContentTextFilterCondition _SelectedTextCondition = Util.Model.ContentTextFilterCondition.None;
        public Util.Model.ContentTextFilterCondition SelectedTextCondition
        {
            get { return _SelectedTextCondition; }
            set
            {
                _SelectedTextCondition = value;
                this.OnPropertyChanged(nameof(SelectedTextCondition));

                filterConditionNotify();
            }
        }


        private string _TextPattern;
        public string TextPattern
        {
            get { return _TextPattern; }
            set
            {
                _TextPattern = value;
                this.OnPropertyChanged(nameof(TextPattern));

                filterConditionNotify();
            }
        }



        public bool HasFilterConditions
        {
            get
            {
                if
                (
                       this.SelectedConsoleMsgType != Util.Model.ConsoleMsgType.NONE
                    || (this.SelectedRegexCondition != Util.Model.ContentTextFilterCondition.None && string.IsNullOrEmpty(this.RegexPattern) == false)
                    || (this.SelectedTextCondition != Util.Model.ContentTextFilterCondition.None && string.IsNullOrEmpty(this.TextPattern) == false)

                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string HasFilterConditionsInfo
        {
            get
            {
                string r = string.Empty;
                if (HasFilterConditions)
                {
                    r = "含过滤条件";
                }
                return r;
            }
        }


        private void btnClearFilterConditions_Click(object sender, RoutedEventArgs e)
        {
            this._SelectedConsoleMsgType = Util.Model.ConsoleMsgType.NONE;
            this._SelectedRegexCondition = Util.Model.ContentTextFilterCondition.None;
            this._RegexPattern = string.Empty;
            this._SelectedTextCondition = Util.Model.ContentTextFilterCondition.None;
            this._TextPattern = string.Empty;

            this.OnPropertyChanged(nameof(SelectedConsoleMsgType));
            this.OnPropertyChanged(nameof(SelectedRegexCondition));
            this.OnPropertyChanged(nameof(RegexPattern));
            this.OnPropertyChanged(nameof(SelectedTextCondition));
            this.OnPropertyChanged(nameof(TextPattern));

            this.filterConditionNotify();
        }


        #endregion

        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion



        #region 内部类 ObservableQueue<T> - 来自 Utils.HowesDOMO

        /// <summary>
        /// V 1.0.1 - 2021-02-22 17:14:00
        /// 修复 CollectionChanged 为 null 却执行通知事件
        /// 
        /// V 1.0.0 - 2021-01-28 17:00:00
        /// 首次创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged
        {
            private int? _QueueMaxCapacity;
            /// <summary>
            /// 队列最大容量 ( 默认为空, 不设定最大容量 )
            /// </summary>
            public int? QueueMaxCapacity
            {
                get { return _QueueMaxCapacity; }
                set
                {
                    _QueueMaxCapacity = value;
                    fix();
                }
            }

            public ObservableQueue(int? maxCapacity = null)
            {
                QueueMaxCapacity = maxCapacity;
            }

            public ObservableQueue(int capacity, int? maxCapacity = null) : base(capacity)
            {
                QueueMaxCapacity = maxCapacity;
            }

            public ObservableQueue(IEnumerable<T> collection, int? maxCapacity = null) : base(collection)
            {
                QueueMaxCapacity = maxCapacity;
            }

            public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;

            public new void Clear()
            {
                base.Clear();
                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }

            public new void Enqueue(T item)
            {
                base.Enqueue(item);
                fix();
                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }

            public new T Dequeue()
            {
                T item = base.Dequeue();
                fix();
                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                return item;
            }

            /// <summary>
            /// 根据队列最大容量, 重新计算队列内的元素
            /// </summary>
            void fix()
            {
                if (QueueMaxCapacity.HasValue == false)
                {
                    return;
                }

                // 被移出队列的数量 = 当前数量 - 最大容量
                int toDequeueCount = this.Count - QueueMaxCapacity.Value;
                if (toDequeueCount <= 0)
                {
                    return;
                }

                for (int i = 0; i < toDequeueCount; i++)
                {
                    base.Dequeue();
                }

                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        #endregion
    }
}